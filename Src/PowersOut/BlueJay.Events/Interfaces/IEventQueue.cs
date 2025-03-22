
// Type: BlueJay.Events.Interfaces.IEventQueue
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Modded by [M]edia[E]xplorer

using System;

#nullable enable
namespace BlueJay.Events.Interfaces
{
  public interface IEventQueue
  {
    void DispatchEvent<T>(T evt, object? target = null);

    void DispatchEventOnce<T>(T evt);

    IDisposable DispatchDelayedEvent<T>(T evt, int timeout, object? target = null);

    IDisposable Timeout(Action callback, int timeout = -1);

    IDisposable AddEventListener<T>(IEventListener<T> handler, int? weight = null);

    IDisposable AddEventListener<T>(Func<T, bool> callback, int? weight = null);

    IDisposable AddEventListener<T>(Func<T, object?, bool> callback, int? weight = null);

    IDisposable AddEventListener<T>(Func<T, bool> callback, object? target, int? weight = null);

    IDisposable AddEventListener<T>(Func<T, object?, bool> callback, object? target, int? weight = null);

    void Update();

    void Draw();

    void Activate();

    void Deactivate();

    void Exit();

    void Tick(bool excludeUpdate = false);
  }
}
