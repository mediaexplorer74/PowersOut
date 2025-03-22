
// Type: GameManager.Events.EventHandlers.PickupEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using GameManager.Addon;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Events.EventHandlers
{
  public class PickupEventListener : EventListener<PickupEvent>
  {
    private readonly IEventQueue _eventQueue;
    private readonly IQuery<DoorAddon> _doorQuery;

    public PickupEventListener(IEventQueue eventQueue, IQuery<DoorAddon> doorQuery)
    {
      this._eventQueue = eventQueue;
      this._doorQuery = doorQuery;
    }

    public override void Process(IEvent<PickupEvent> evt)
    {
      Door? nullable1;
      switch (evt.Data.Pickup)
      {
        case Pickup.BrotherRoomKey:
          nullable1 = new Door?(Door.BrotherRoomDoor);
          break;
        case Pickup.ParentsKey:
          nullable1 = new Door?(Door.ParentsDoor);
          break;
        case Pickup.LoftKey:
          nullable1 = new Door?(Door.LoftDoor);
          break;
        case Pickup.KitchenKey:
          nullable1 = new Door?(Door.KitchenDoor);
          break;
        default:
          nullable1 = new Door?();
          break;
      }
      Door? door = nullable1;
      if (!door.HasValue)
        return;
      IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._doorQuery, (Func<IEntity, bool>) (x =>
      {
        int door1 = (int) x.GetAddon<DoorAddon>().Door;
        Door? nullable2 = door;
        int valueOrDefault = (int) nullable2.GetValueOrDefault();
        return door1 == valueOrDefault & nullable2.HasValue;
      }));
      if (entity == null || !entity.Contains<WallAddon>())
        return;
      this._eventQueue.DispatchEvent<RemoveEntityEvent>(new RemoveEntityEvent(entity));
    }
  }
}
