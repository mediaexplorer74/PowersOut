
// Type: GameManager.Systems.EndTriggerSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using GameManager.Events.EventHandlers;
using GameManager.Services;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Systems
{
  public class EndTriggerSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly IEventQueue _eventQueue;
    private readonly GameService _gameService;
    private readonly IQuery<EndTriggerAddon, BoundsAddon> _endingQuery;
    private readonly IQuery<PlayerAddon, PositionAddon, BoundsAddon> _playerQuery;

    public EndTriggerSystem(
      ProfileService profileService,
      IEventQueue eventQueue,
      GameService gameService,
      IQuery<EndTriggerAddon, BoundsAddon> endingQuery,
      IQuery<PlayerAddon, PositionAddon, BoundsAddon> playerQuery)
    {
      this._profileService = profileService;
      this._eventQueue = eventQueue;
      this._gameService = gameService;
      this._endingQuery = endingQuery;
      this._playerQuery = playerQuery;
    }

    public void OnUpdate()
    {
      IEntity entity1 = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._endingQuery.WhereLayer("Triggers_" + this._gameService.CurrentLevel));
      IEntity entity2 = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._playerQuery);
      if (entity2 == null || entity1 == null)
        return;
      BoundsAddon addon1 = entity1.GetAddon<BoundsAddon>();
      PositionAddon addon2 = entity2.GetAddon<PositionAddon>();
      BoundsAddon addon3 = entity2.GetAddon<BoundsAddon>();
      if (!new Rectangle((int) addon2.Position.X + addon3.Bounds.X, (int) addon2.Position.Y + addon3.Bounds.Y, addon3.Bounds.Width, addon3.Bounds.Height).Intersects(addon1.Bounds))
        return;
      this._gameService.CanControlPlayer = false;
      this._eventQueue.DispatchEvent<EndEvent>(new EndEvent());
    }
  }
}
