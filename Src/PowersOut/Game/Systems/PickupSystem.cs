
// Type: GameManager.Systems.PickupSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Container;
using BlueJay.Events.Interfaces;
using BlueJay.Utils;
using GameManager.Addon;
using GameManager.Events;
using GameManager.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable enable
namespace GameManager.Systems
{
  public class PickupSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly GameService _gameService;
    private readonly IEventQueue _eventQueue;
    private readonly IContentManagerContainer _content;
    private readonly IQuery<PositionAddon, BoundsAddon, PickupAddon> _query;
    private readonly IQuery<PositionAddon, BoundsAddon, PlayerAddon> _playerQuery;

    public PickupSystem(
      ProfileService profileService,
      GameService gameService,
      IEventQueue eventQueue,
      IContentManagerContainer content,
      IQuery<PositionAddon, BoundsAddon, PickupAddon> query,
      IQuery<PositionAddon, BoundsAddon, PlayerAddon> playerQuery)
    {
      this._profileService = profileService;
      this._gameService = gameService;
      this._eventQueue = eventQueue;
      this._content = content;
      this._query = query;
      this._playerQuery = playerQuery;
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      IEntity entity1 = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._playerQuery);
      if (entity1 != null)
      {
        PositionAddon addon1 = entity1.GetAddon<PositionAddon>();
        BoundsAddon addon2 = entity1.GetAddon<BoundsAddon>();
        Rectangle rectangle1 = new Rectangle((int) addon1.Position.X + addon2.Bounds.X, (int) addon1.Position.Y + addon2.Bounds.Y, addon2.Bounds.Width, addon2.Bounds.Height);
        foreach (IEntity entity2 in (IEnumerable<IEntity>) this._query.WhereLayer("Triggers_" + this._gameService.CurrentLevel))
        {
          PositionAddon addon3 = entity2.GetAddon<PositionAddon>();
          BoundsAddon addon4 = entity2.GetAddon<BoundsAddon>();
          Rectangle rectangle2 = new Rectangle((int) addon3.Position.X + addon4.Bounds.X, (int) addon3.Position.Y + addon4.Bounds.Y, addon4.Bounds.Width, addon4.Bounds.Height);
          if (rectangle1.IntersectsWith(rectangle2))
          {
            PickupAddon addon5 = entity2.GetAddon<PickupAddon>();
            if (addon5.Pickup == Pickup.FlashLight)
            {
              this._gameService.HasFlashLight = true;
              TextureAddon addon6 = entity1.GetAddon<TextureAddon>() with
              {
                Texture = this._content.Load<ITexture2DContainer>("character/Anna_Sprite_Flashlight-sheet")
              };
              entity1.Update<TextureAddon>(addon6);
            }
            else
              this._gameService.FoundKeys.Add(addon5.Pickup);
            this._eventQueue.DispatchEvent<PickupEvent>(new PickupEvent(addon5.Pickup));
            this._eventQueue.DispatchEvent<RemoveEntityEvent>(new RemoveEntityEvent(entity2));
            if (entity2.Contains<TextAddon>())
            {
              ExpressionAddon addon7 = entity2.GetAddon<ExpressionAddon>();
              this._eventQueue.DispatchEvent<TriggerSpeechBubbleEvent>(new TriggerSpeechBubbleEvent(entity2.GetAddon<TextAddon>().Text, addon7.Expression));
            }
          }
        }
      }
      this._profileService.Profile[nameof (PickupSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
