
// Type: GameManager.Events.EventHandlers.RemoveEntityEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using System;

#nullable enable
namespace GameManager.Events.EventHandlers
{
  public class RemoveEntityEventListener : EventListener<RemoveEntityEvent>
  {
    private readonly IServiceProvider _serviceProvider;

    public RemoveEntityEventListener(IServiceProvider serviceProvider)
    {
      this._serviceProvider = serviceProvider;
    }

    public override void Process(IEvent<RemoveEntityEvent> evt)
    {
      this._serviceProvider.RemoveEntity(evt.Data.Entity);
    }
  }
}
