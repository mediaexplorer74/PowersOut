
// Type: BlueJay.Common.Systems.VelocitySystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class VelocitySystem : IUpdateSystem, ISystem
  {
    private readonly IQuery<PositionAddon, VelocityAddon> _entities;

    public VelocitySystem(IQuery<PositionAddon, VelocityAddon> entities)
    {
      this._entities = entities;
    }

    public void OnUpdate()
    {
      foreach (IEntity entity in (IEnumerable<IEntity>) this._entities)
      {
        PositionAddon addon1 = entity.GetAddon<PositionAddon>();
        VelocityAddon addon2 = entity.GetAddon<VelocityAddon>();
        addon1.Position += addon2.Velocity;
        entity.Update<PositionAddon>(addon1);
      }
    }
  }
}
