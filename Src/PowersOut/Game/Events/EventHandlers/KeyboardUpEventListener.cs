
// Type: GameManager.Events.EventHandlers.KeyboardUpEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Common.Events.Keyboard;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework.Input;
using GameManager.Addon;
using GameManager.Services;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Events.EventHandlers
{
  public class KeyboardUpEventListener : EventListener<KeyboardUpEvent>
  {
    private readonly GameService _gameService;
    private readonly IQuery<PlayerAddon, VelocityAddon> _query;

    public KeyboardUpEventListener(
      GameService gameService,
      IQuery<PlayerAddon, VelocityAddon> query)
    {
      this._gameService = gameService;
      this._query = query;
    }

    public override void Process(IEvent<KeyboardUpEvent> evt)
    {
      if (!this._gameService.CanControlPlayer)
        return;
      IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._query);
      if (entity == null)
        return;
      VelocityAddon addon = entity.GetAddon<VelocityAddon>();
      switch (evt.Data.Key)
      {
        case Keys.A:
        case Keys.D:
          addon.Velocity.X = 0.0f;
          break;
        case Keys.E:
          this._gameService.ShowFlashLight = false;
          break;
        case Keys.S:
        case Keys.W:
          addon.Velocity.Y = 0.0f;
          break;
      }
      entity.Update<VelocityAddon>(addon);
    }
  }
}
