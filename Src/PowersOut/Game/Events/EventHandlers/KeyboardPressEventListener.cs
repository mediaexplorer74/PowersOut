
// Type: GameManager.Events.EventHandlers.KeyboardPressEventListener
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
  public class KeyboardPressEventListener : EventListener<KeyboardPressEvent>
  {
    private readonly GameService _gameService;
    private readonly IQuery<PlayerAddon, VelocityAddon> _query;

    public KeyboardPressEventListener(
      GameService gameService,
      IQuery<PlayerAddon, VelocityAddon> query)
    {
      this._gameService = gameService;
      this._query = query;
    }

    public override void Process(IEvent<KeyboardPressEvent> evt)
    {
      if (!this._gameService.CanControlPlayer)
        return;
      IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._query);
      if (entity == null)
        return;
      VelocityAddon addon = entity.GetAddon<VelocityAddon>();
      switch (evt.Data.Key)
      {
        // kbd input - scheme 1
        case Keys.W:
            addon.Velocity.Y = -1f;
            break;
        case Keys.A:
          addon.Velocity.X = -1f;
          break;
        case Keys.S:
          addon.Velocity.Y = 1f;
          break;
        case Keys.D:
            addon.Velocity.X = 1f;
            break;
        case Keys.E:
            if (this._gameService.HasFlashLight)
            {
                this._gameService.ShowFlashLight = true;
                break;
            }
            break;

        // kbd input - scheme 2
        case Keys.Up:
            addon.Velocity.Y = -1f;
            break;
        case Keys.Left:
            addon.Velocity.X = -1f;
            break;
        case Keys.Down:
            addon.Velocity.Y = 1f;
            break;
        case Keys.Right:
            addon.Velocity.X = 1f;
            break;
        case Keys.Space:
            if (this._gameService.HasFlashLight)
            {
                this._gameService.ShowFlashLight = true;
                break;
            }
            break;
        }
        entity.Update<VelocityAddon>(addon);
    }
  }
}
