
// Type: BlueJay.Events.Event`1
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Modded by [M]edia[E]xplorer

using BlueJay.Events.Interfaces;
using System.Reflection;

#nullable enable
namespace BlueJay.Events
{
  internal class Event<T> : IEvent<T>, IEvent, IInternalEvent
  {
    public T Data { get; private set; }

    public string Name { get; private set; }

    public bool IsComplete { get; private set; }

    public object? Target { get; private set; }

    public int Timeout { get; set; }

    public bool IsCancelled { get; set; }

    public Event(T data, object? target = null, int timeout = 0)
    {
      this.Data = data;
      this.Name = (object) data != null ? ((MemberInfo) data.GetType()).Name : string.Empty;
      this.Target = target;
      this.IsComplete = false;
      this.Timeout = timeout;
      this.IsCancelled = false;
    }

    public void StopPropagation() => this.IsComplete = true;
  }
}
