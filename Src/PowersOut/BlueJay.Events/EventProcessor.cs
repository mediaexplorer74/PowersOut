
// Type: BlueJay.Events.EventProcessor
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Assembly location: BlueJay.Events.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Events.Interfaces;

#nullable enable
namespace BlueJay.Events
{
  internal class EventProcessor : IEventProcessor
  {
    private readonly IEventQueue _queue;

    public EventProcessor(IEventQueue queue) => this._queue = queue;

    public void Update()
    {
      this._queue.Tick();
      this._queue.Update();
    }

    public void Draw() => this._queue.Draw();

    public void Activate() => this._queue.Activate();

    public void Deactivate() => this._queue.Deactivate();

    public void Exit() => this._queue.Exit();
  }
}
