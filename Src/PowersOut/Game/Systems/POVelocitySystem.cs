
// Type: GameManager.Systems.POVelocitySystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using GameManager.Services;
using System;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Systems
{
  public class POVelocitySystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly IQuery<PositionAddon, VelocityAddon> _query;

    public POVelocitySystem(
      ProfileService profileService,
      IQuery<PositionAddon, VelocityAddon> query)
    {
      this._profileService = profileService;
      this._query = query;
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      foreach (IEntity entity in (IEnumerable<IEntity>) this._query)
      {
        PositionAddon addon1 = entity.GetAddon<PositionAddon>();
        VelocityAddon addon2 = entity.GetAddon<VelocityAddon>();
        addon1.Position += addon2.Velocity;
        entity.Update<PositionAddon>(addon1);
      }
      this._profileService.Profile[nameof (POVelocitySystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
