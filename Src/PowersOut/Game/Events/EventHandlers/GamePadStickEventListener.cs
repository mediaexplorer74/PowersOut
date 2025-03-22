
// Type: GameManager.Events.EventHandlers.GamePadStickEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Common.Events.GamePad;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using GameManager.Services;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Events.EventHandlers
{
  public class GamePadStickEventListener : EventListener<GamePadStickEvent>
  {
    private readonly IQuery<PlayerAddon, VelocityAddon> _query;
    private readonly GameService _gameService;

    public GamePadStickEventListener(
      GameService gameService,
      IQuery<PlayerAddon, VelocityAddon> query)
    {
      this._gameService = gameService;
      this._query = query;
    }

    public override void Process(IEvent<GamePadStickEvent> evt)
    {
      if (!this._gameService.CanControlPlayer || evt.Data.Index != PlayerIndex.One || evt.Data.Type == GamePadStickEvent.ThumbStickType.Right)
        return;
      IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._query);
      if (entity == null)
        return;
      VelocityAddon addon = entity.GetAddon<VelocityAddon>() with
      {
        Velocity = new Vector2(evt.Data.Value.X, -evt.Data.Value.Y)
      };
      entity.Update<VelocityAddon>(addon);
    }
  }
}
