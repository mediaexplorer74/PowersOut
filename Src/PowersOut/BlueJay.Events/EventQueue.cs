
// Type: BlueJay.Events.EventQueue
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Assembly location: BlueJay.Events.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core.Interfaces;
using BlueJay.Events.Interfaces;
using BlueJay.Events.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Reflection;
//using System.Runtime.InteropServices;

#nullable enable
namespace BlueJay.Events
{
    internal class EventQueue : IEventQueue
    {
        private readonly IDeltaService _delta;
        private int _nullWeight = 2147383647;
        private readonly Queue<IEvent?> _current = new Queue<IEvent?>();
        private readonly Queue<IEvent?> _next = new Queue<IEvent?>();
        private Dictionary<string, List<EventListenerWeight>> 
            _handlers = new Dictionary<string, List<EventListenerWeight>>();

        public EventQueue(IDeltaService delta) => this._delta = delta;

        public void DispatchEvent<T>(T evt, object? target = null)
        {
            this.DispatchDelayedEvent<T>(evt, 0, target);
        }

        public void DispatchEventOnce<T>(T evt)
        {
            if (Enumerable.Any<IEvent>((IEnumerable<IEvent>)this._next, (Func<IEvent, bool>)(x => x is IEvent<T>)))
                return;
            this.DispatchDelayedEvent<T>(evt, 0, (object)null);
        }

        public IDisposable DispatchDelayedEvent<T>(T evt, int timeout, object? target = null)
        {
            if ((object)evt != null)
            {
                string name = (evt.GetType()).Name;
                if (this._handlers.ContainsKey(name) && this._handlers[name].Count > 0)
                {
                    object obj;
                    if (typeof(T) == typeof(object))
                        obj = Activator.CreateInstance(typeof(Event<>).MakeGenericType(new Type[1]
                        {
                  evt.GetType()
                        }), new object[3]
                        {
                  (object) evt,
                  target,
                  (object) timeout
                        });
                    else
                        obj = (object)new Event<T>(evt, target, timeout);
                    if (obj != null && obj is IEvent event1 && obj is IInternalEvent event2)
                    {
                        this._next.Enqueue(event1);
                        return (IDisposable)new EventQueue.EventUnsubscriber(event2);
                    }
                }
            }
            return (IDisposable)new EventQueue.EventUnsubscriber((IInternalEvent)null);
        }

        public IDisposable Timeout(Action callback, int timeout = -1)
        {
            IDisposable listener = (IDisposable)null;
            listener = this.AddEventListener<TimeoutEvent>((Func<TimeoutEvent, object, bool>)((x, obj) =>
            {
                if (obj is IDisposable disposable2 && listener == disposable2)
                {
                    callback();
                    disposable2.Dispose();
                }
                return true;
            }), new int?());
            return (IDisposable)new ZipDisposable(new IDisposable[2]
            {
            listener,
            this.DispatchDelayedEvent<TimeoutEvent>(new TimeoutEvent(), timeout, (object) listener)
            });
        }

        public IDisposable AddEventListener<T>(IEventListener<T> handler, int? weight = null)
        {
            string name = (typeof(T)).Name;
            if (!this._handlers.ContainsKey(name))
                this._handlers[name] = new List<EventListenerWeight>();
            this._handlers[name].Add(new EventListenerWeight((IEventListener)handler, weight ?? this._nullWeight++));
            this._handlers[name].Sort((x, y) => x.Weight.CompareTo(y.Weight));
            return (IDisposable)new EventQueue.Unsubscriber(this._handlers, (IEventListener)handler, name);
        }

        public IDisposable AddEventListener<T>(Func<T, bool> callback, int? weight = null)
        {
            return this.AddEventListener<T>((IEventListener<T>)new CallbackListener<T>((Func<T, object, bool>)((x, t) => callback(x)), (object)null, false), weight);
        }

        public IDisposable AddEventListener<T>(Func<T, object?, bool> callback, int? weight = null)
        {
            return this.AddEventListener<T>((IEventListener<T>)new CallbackListener<T>(callback, (object)null, false), weight);
        }

        public IDisposable AddEventListener<T>(Func<T, bool> callback, object? target, int? weight = null)
        {
            return this.AddEventListener<T>((IEventListener<T>)new CallbackListener<T>((Func<T, object, bool>)((x, t) => callback(x)), target, true), weight);
        }

        public IDisposable AddEventListener<T>(
          Func<T, object?, bool> callback,
          object? target,
          int? weight = null)
        {
            return this.AddEventListener<T>((IEventListener<T>)new CallbackListener<T>(callback, target, true), weight);
        }

        public void Update()
        {
            while (this._current.Count > 0)
                this.ProcessEvent(this._current.Dequeue());
        }

        public void Draw() => this.ProcessEvent((IEvent)new Event<DrawEvent>(new DrawEvent()));

        public void Activate()
        {
            this.ProcessEvent((IEvent)new Event<ActivateEvent>(new ActivateEvent()));
        }

        public void Deactivate()
        {
            this.ProcessEvent((IEvent)new Event<DeactivateEvent>(new DeactivateEvent()));
        }

        public void Exit()
        {
            this.ProcessEvent((IEvent)new Event<ExitEvent>(new ExitEvent()));
            this.Tick(true);
            this.Update();
        }

        public void Tick(bool excludeUpdate = false)
        {
            if (!excludeUpdate)
                this.DispatchEvent<UpdateEvent>(new UpdateEvent(), (object)null);
            while (this._next.Count > 0)
                this._current.Enqueue(this._next.Dequeue());
        }

        // ...

        private void ProcessEvent(IEvent? evt)
        {
            if (evt == null || evt is IInternalEvent internalEvent && internalEvent.IsCancelled)
                return;
            if (evt is IInternalEvent evt1 && this.ShouldEventNotProcess(evt1))
            {
                this._next.Enqueue(evt);
            }
            else
            {
                if (!this._handlers.ContainsKey(evt.Name))
                    return;

                //RnD
                Span<EventListenerWeight> span = CollectionsMarshal.AsSpan(this._handlers[evt.Name]);
                for (int index = 0; index < span.Length; ++index)
                {
                    EventListenerWeight eventListenerWeight = span[index];
                    if (eventListenerWeight != null && eventListenerWeight.EventListener.ShouldProcess(evt))
                    {
                        eventListenerWeight.EventListener.Process(evt);
                        if (evt.IsComplete)
                            break;
                    }
                }
            }
        }

        private bool ShouldEventNotProcess(IInternalEvent evt)
        {
            evt.Timeout -= this._delta.Delta;
            return evt.Timeout > 0;
        }

        private class EventUnsubscriber : IDisposable
        {
            private readonly IInternalEvent? _event;

            public EventUnsubscriber(IInternalEvent? @event) => this._event = @event;

            public void Dispose()
            {
                if (this._event == null)
                    return;
                this._event.IsCancelled = true;
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly Dictionary<string, List<EventListenerWeight>> _handlers;
            private readonly IEventListener _handler;
            private readonly string _key;

            internal Unsubscriber(
              Dictionary<string, List<EventListenerWeight>> handlers,
              IEventListener handler,
              string key)
            {
                this._handlers = handlers;
                this._handler = handler;
                this._key = key;
            }

            public void Dispose()
            {
                EventListenerWeight eventListenerWeight = 
                    Enumerable.FirstOrDefault<EventListenerWeight>((IEnumerable<EventListenerWeight>)this._handlers[this._key], (Func<EventListenerWeight, bool>)(x => x.EventListener == this._handler));
                if (eventListenerWeight == null)
                    return;
                this._handlers[this._key].Remove(eventListenerWeight);
            }
        }
    }
}
