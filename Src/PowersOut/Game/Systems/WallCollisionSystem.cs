
// Type: GameManager.Systems.WallCollisionSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
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
  public class WallCollisionSystem : IUpdateSystem, ISystem
  {
    private readonly IEventQueue _eventQueue;
    private readonly ProfileService _profileService;
    private readonly GameService _gameService;
    private readonly IQuery<CollisionAddon, PositionAddon, VelocityAddon, BoundsAddon> _collisionQuery;
    private readonly IQuery<PositionAddon, BoundsAddon, WallAddon> _wallsQuery;

    public WallCollisionSystem(
      IEventQueue eventQueue,
      ProfileService profileService,
      GameService gameService,
      IQuery<CollisionAddon, PositionAddon, VelocityAddon, BoundsAddon> collisionQuery,
      IQuery<PositionAddon, BoundsAddon, WallAddon> wallsQuery)
    {
      this._eventQueue = eventQueue;
      this._profileService = profileService;
      this._gameService = gameService;
      this._collisionQuery = collisionQuery;
      this._wallsQuery = wallsQuery;
    }

    public void OnUpdate()
    {
      foreach (IEntity collision in (IEnumerable<IEntity>) this._collisionQuery)
        this.HandleCollision(collision);
    }

    public void HandleCollision(IEntity collision)
    {
      DateTime now = DateTime.Now;
      BoundsAddon addon1 = collision.GetAddon<BoundsAddon>();
      PositionAddon addon2 = collision.GetAddon<PositionAddon>();
      VelocityAddon cva = collision.GetAddon<VelocityAddon>();
      Rectangle cBounds = new Rectangle((int) addon2.Position.X + addon1.Bounds.X, (int) addon2.Position.Y + addon1.Bounds.Y, addon1.Bounds.Width, addon1.Bounds.Height);
      Rectangle rectangle1 = cBounds.Add(cva.Velocity);
      IQuery query = this._wallsQuery.WhereLayer("Walls_" + this._gameService.CurrentLevel);
      List<Rectangle> rectangleList = new List<Rectangle>();
      foreach (IEntity entity in (IEnumerable<IEntity>) query)
      {
        BoundsAddon addon3 = entity.GetAddon<BoundsAddon>();
        Point point = entity.GetAddon<PositionAddon>().Position.ToPoint();
        Rectangle rectangle2 = new Rectangle(point.X + addon3.Bounds.X, point.Y + addon3.Bounds.Y, addon3.Bounds.Width, addon3.Bounds.Height);
        if (rectangle2.Intersects(rectangle1))
        {
          rectangleList.Add(rectangle2);
          if (entity.Contains<TextAddon>())
          {
            ExpressionAddon addon4 = entity.GetAddon<ExpressionAddon>();
            this._eventQueue.DispatchEvent<TriggerSpeechBubbleEvent>(new TriggerSpeechBubbleEvent(entity.GetAddon<TextAddon>().Text, addon4.Expression));
          }
        }
      }
      if (rectangleList.Count > 0)
      {
        IOrderedEnumerable<Rectangle> iorderedEnumerable = Enumerable.OrderBy<Rectangle, float>((IEnumerable<Rectangle>) rectangleList, (Func<Rectangle, float>) (x =>
        {
          Point center = cBounds.Center;
          Vector2 vector2_1 = center.ToVector2();
          center = x.Center;
          Vector2 vector2_2 = center.ToVector2();
          return Vector2.Distance(vector2_1, vector2_2);
        }));
        Rectangle rectangle3 = Enumerable.FirstOrDefault<Rectangle>((IEnumerable<Rectangle>) iorderedEnumerable, (Func<Rectangle, bool>) (x => x.Intersects(cBounds.AddX(cva.Velocity.X))));
        if (Rectangle.Empty != rectangle3)
        {
          int x = ((double) cva.Velocity.X > 0.0 ? rectangle3.Left - addon1.Bounds.Width : rectangle3.Right) - addon1.Bounds.X;
          addon2.Position = new Vector2((float) x, addon2.Position.Y);
          cva.Velocity = new Vector2(0.0f, cva.Velocity.Y);
          cBounds = new Rectangle((int) addon2.Position.X + addon1.Bounds.X, (int) addon2.Position.Y + addon1.Bounds.Y, addon1.Bounds.Width, addon1.Bounds.Height);
        }
        Rectangle rectangle4 = Enumerable.FirstOrDefault<Rectangle>((IEnumerable<Rectangle>) iorderedEnumerable, (Func<Rectangle, bool>) (x => x.Intersects(cBounds.AddY(cva.Velocity.Y))));
        if (Rectangle.Empty != rectangle4)
        {
          int y = ((double) cva.Velocity.Y > 0.0 ? rectangle4.Top - addon1.Bounds.Height : rectangle4.Bottom) - addon1.Bounds.Y;
          addon2.Position = new Vector2(addon2.Position.X, (float) y);
          cva.Velocity = new Vector2(cva.Velocity.X, 0.0f);
        }
        collision.Update<PositionAddon>(addon2);
        collision.Update<VelocityAddon>(cva);
      }
      this._profileService.Profile[nameof (WallCollisionSystem)] = 
                now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
