
// Type: GameManager.Systems.DirectionRayUpdateSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using GameManager.Services;
using System;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Systems
{
  public class DirectionRayUpdateSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly IQuery<DirectionRayAddon, PositionAddon, DirectionAddon> _query;
    private readonly Direction[] _directions;

    public DirectionRayUpdateSystem(
      ProfileService profileService,
      IQuery<DirectionRayAddon, PositionAddon, DirectionAddon> query)
    {
      this._profileService = profileService;
      this._query = query;
      this._directions = new Direction[8]
      {
        Direction.East,
        Direction.SouthEast,
        Direction.South,
        Direction.SouthWest,
        Direction.West,
        Direction.NorthWest,
        Direction.North,
        Direction.NorthEast
      };
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      foreach (IEntity entity in (IEnumerable<IEntity>) this._query)
      {
        DirectionRayAddon addon1 = entity.GetAddon<DirectionRayAddon>();
        PositionAddon addon2 = entity.GetAddon<PositionAddon>();
        double radians = Math.PI / 4.0 * (double) Array.IndexOf<Direction>(this._directions, entity.GetAddon<DirectionAddon>().Direction) - addon1.Offset;
        double num1 = radians + addon1.Offset * 2.0;
        double num2 = addon1.Offset * 2.0 / 5.0;
        Vector2 start = addon2.Position + addon1.PositionOffset;
        addon1.Rays.Clear();
        for (; radians <= num1; radians += num2)
        {
          Ray2 ray2 = new Ray2(start, radians);
          addon1.Rays.Add(ray2);
        }
        entity.Update<DirectionRayAddon>(addon1);
      }
      this._profileService.Profile[nameof (DirectionRayUpdateSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
