
// Type: GameManager.Systems.ExitTriggerSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using GameManager.Events;
using GameManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Systems
{
  public class ExitTriggerSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly IEventQueue _eventQueue;
    private readonly GameService _gameService;
    private readonly IQuery<PlayerAddon> _playerQuery;
    private readonly IQuery<ExitAddon> _exitQuery;

    public ExitTriggerSystem(
      ProfileService profileService,
      IEventQueue eventQueue,
      GameService gameService,
      IQuery<PlayerAddon> playerQuery,
      IQuery<ExitAddon> exitQuery)
    {
      this._profileService = profileService;
      this._eventQueue = eventQueue;
      this._gameService = gameService;
      this._playerQuery = playerQuery;
      this._exitQuery = exitQuery;
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      IEntity entity1 = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._playerQuery);
      if (entity1 != null)
      {
        BoundsAddon addon1 = entity1.GetAddon<BoundsAddon>();
        PositionAddon addon2 = entity1.GetAddon<PositionAddon>();
        Rectangle rectangle = new Rectangle((int) addon2.Position.X + addon1.Bounds.X, (int) addon2.Position.Y + addon1.Bounds.Y, addon1.Bounds.Width, addon1.Bounds.Height);
        foreach (IEntity entity2 in (IEnumerable<IEntity>) this._exitQuery.WhereLayer("Triggers_" + this._gameService.CurrentLevel))
        {
          BoundsAddon addon3 = entity2.GetAddon<BoundsAddon>();
          ExitAddon addon4 = entity2.GetAddon<ExitAddon>();
          if (rectangle.Intersects(addon3.Bounds))
          {
            this._eventQueue.DispatchEvent<LevelTransitionEvent>(new LevelTransitionEvent(addon4.GotoLevel, addon4.SpawnPosition));
            break;
          }
        }
      }
      this._profileService.Profile[nameof (ExitTriggerSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
