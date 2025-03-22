
// Type: BlueJay.EventListeners.UpdateEventListener
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using BlueJay.Events.Lifecycle;

#nullable enable
namespace BlueJay.EventListeners
{
  internal class UpdateEventListener : EventListener<UpdateEvent>
  {
    private readonly ISystem _system;

    public UpdateEventListener(ISystem system) => this._system = system;

    public override void Process(IEvent<UpdateEvent> evt)
    {
      if (!(this._system is IUpdateSystem))
        return;
      ((IUpdateSystem) this._system).OnUpdate();
    }
  }
}
