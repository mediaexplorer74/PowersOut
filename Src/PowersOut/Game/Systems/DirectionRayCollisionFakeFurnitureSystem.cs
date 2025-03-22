
// Type: GameManager.Systems.DirectionRayCollisionFakeFurnitureSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events.Interfaces;
using GameManager.Addon;
using GameManager.Events;
using GameManager.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Systems
{
  public class DirectionRayCollisionFakeFurnitureSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly GameService _gameService;
    private readonly IEventQueue _eventQueue;
    private readonly IQuery<DirectionRayAddon> _rayQuery;
    private readonly IQuery<FakeFurnitureAddon, BoundsAddon, PositionAddon> _furnitureQuery;

    public DirectionRayCollisionFakeFurnitureSystem(
      ProfileService profileService,
      GameService gameService,
      IEventQueue eventQueue,
      IQuery<DirectionRayAddon> rayQuery,
      IQuery<FakeFurnitureAddon, BoundsAddon, PositionAddon> furnitureQuery)
    {
      this._profileService = profileService;
      this._gameService = gameService;
      this._eventQueue = eventQueue;
      this._rayQuery = rayQuery;
      this._furnitureQuery = furnitureQuery;
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      if (!this._gameService.ShowFlashLight)
        return;
      string[] strArray = new string[1]
      {
        "Walls_" + this._gameService.CurrentLevel
      };
      foreach (IEntity entity1 in (IEnumerable<IEntity>) this._rayQuery)
      {
        DirectionRayAddon dra = entity1.GetAddon<DirectionRayAddon>();
        Line2[] array = Enumerable.ToArray<Line2>(Enumerable.Select<Ray2, Line2>((IEnumerable<Ray2>) dra.Rays, (Func<Ray2, Line2>) (x => new Line2(x.Start, x.GetPosition(dra.Length)))));
        foreach (IEntity entity2 in (IEnumerable<IEntity>) this._furnitureQuery.WhereLayer(strArray))
        {
          BoundsAddon addon1 = entity2.GetAddon<BoundsAddon>();
          PositionAddon addon2 = entity2.GetAddon<PositionAddon>();
          Microsoft.Xna.Framework.Vector2 start = new Vector2((float) addon1.Bounds.X, (float) addon1.Bounds.Y) + addon2.Position;
          Microsoft.Xna.Framework.Vector2 vector2_1 = start + new Vector2((float) addon1.Bounds.Width, 0.0f);
          Microsoft.Xna.Framework.Vector2 vector2_2 = start + new Vector2(0.0f, (float) addon1.Bounds.Height);
          Microsoft.Xna.Framework.Vector2 end = start + new Vector2((float) addon1.Bounds.Width, (float) addon1.Bounds.Height);
          Line2[] line2Array = new Line2[4]
          {
            new Line2(start, vector2_1),
            new Line2(start, vector2_2),
            new Line2(vector2_1, end),
            new Line2(vector2_2, end)
          };
          bool flag = false;
          foreach (Line2 line2 in array)
          {
            foreach (Line2 other in line2Array)
            {
              if (line2.Intersects(other))
              {
                flag = true;
                this._eventQueue.DispatchEvent<RemoveEntityEvent>(new RemoveEntityEvent(entity2));
                break;
              }
            }
            if (flag)
              break;
          }
        }
      }
      this._profileService.Profile[nameof (DirectionRayCollisionFakeFurnitureSystem)] = 
                now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
