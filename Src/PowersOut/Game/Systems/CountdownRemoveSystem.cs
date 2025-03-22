
// Type: GameManager.Systems.CountdownRemoveSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Interfaces;
using BlueJay.Events.Interfaces;
using GameManager.Addon;
using GameManager.Events;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Systems
{
  public class CountdownRemoveSystem : IUpdateSystem, ISystem
  {
    private readonly IEventQueue _eventQueue;
    private readonly IDeltaService _deltaService;
    private readonly IQuery<CountdownAddon> _query;

    public CountdownRemoveSystem(
      IEventQueue eventQueue,
      IDeltaService deltaService,
      IQuery<CountdownAddon> query)
    {
      this._eventQueue = eventQueue;
      this._deltaService = deltaService;
      this._query = query;
    }

    public void OnUpdate()
    {
      foreach (IEntity entity in (IEnumerable<IEntity>) this._query)
      {
        CountdownAddon addon = entity.GetAddon<CountdownAddon>();
        addon.Countdown -= this._deltaService.Delta;
        if (addon.Countdown <= 0)
          this._eventQueue.DispatchEvent<RemoveEntityEvent>(new RemoveEntityEvent(entity));
        entity.Update<CountdownAddon>(addon);
      }
    }
  }
}
