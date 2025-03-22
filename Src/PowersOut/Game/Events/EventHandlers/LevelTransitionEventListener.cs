
// Type: GameManager.Events.EventHandlers.LevelTransitionEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using GameManager.Addon;
using GameManager.Services;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Events.EventHandlers
{
    public class LevelTransitionEventListener : EventListener<LevelTransitionEvent>
    {
        private readonly GameService _gameService;
        private readonly IQuery<PlayerAddon> _playerQuery;

        public LevelTransitionEventListener(GameService gameService, IQuery<PlayerAddon> playerQuery)
        {
            this._gameService = gameService;
            this._playerQuery = playerQuery;
        }

        public override void Process(IEvent<LevelTransitionEvent> evt)
        {
            IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>)this._playerQuery);
            if (entity == null)
                return;
            this._gameService.CurrentLevel = evt.Data.GotoLevel;
            PositionAddon addon1 = entity.GetAddon<PositionAddon>();
            BoundsAddon addon2 = entity.GetAddon<BoundsAddon>();
            addon1.Position = evt.Data.SpawnPosition - new Vector2(addon2.Bounds.X, addon2.Bounds.Y);
            entity.Update<PositionAddon>(addon1);
        }
    }
}
