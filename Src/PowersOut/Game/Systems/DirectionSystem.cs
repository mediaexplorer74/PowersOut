
// Type: GameManager.Systems.DirectionSystem
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
  public class DirectionSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    public IQuery<VelocityAddon, DirectionAddon> _query;

    public DirectionSystem(
      ProfileService profileService,
      IQuery<VelocityAddon, DirectionAddon> query)
    {
      this._profileService = profileService;
      this._query = query;
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      foreach (IEntity entity in (IEnumerable<IEntity>) this._query)
      {
        VelocityAddon addon1 = entity.GetAddon<VelocityAddon>();
        DirectionAddon addon2 = entity.GetAddon<DirectionAddon>();
        if (addon1.Velocity == Vector2.Zero)
          return;
        double num1 = Math.Atan2((double) addon1.Velocity.X, (double) addon1.Velocity.Y);
        if (num1 < 0.0)
          num1 += 2.0 * Math.PI;
        double num2 = Math.PI / 8.0;
        int num3 = (int) Math.Round(num1 / num2);
        addon2.Direction = num3 > 0 ? (num3 > 2 ? (num3 > 4 ? (num3 > 6 ? (num3 > 8 ? (num3 > 10 ? (num3 > 12 ? (num3 > 14 ? Direction.South : Direction.SouthWest) : Direction.West) : Direction.NorthWest) : Direction.North) : Direction.NorthEast) : Direction.East) : Direction.SouthEast) : Direction.South;
        entity.Update<DirectionAddon>(addon2);
      }
      this._profileService.Profile[nameof (DirectionSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
