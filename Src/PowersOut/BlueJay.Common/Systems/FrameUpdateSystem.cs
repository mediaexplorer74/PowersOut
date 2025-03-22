
// Type: BlueJay.Common.Systems.FrameUpdateSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Interfaces;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class FrameUpdateSystem : IUpdateSystem, ISystem
  {
    private readonly IDeltaService _delta;
    private readonly IQuery<FrameAddon> _frameQuery;

    public FrameUpdateSystem(IDeltaService delta, IQuery<FrameAddon> frameQuery)
    {
      this._delta = delta;
      this._frameQuery = frameQuery;
    }

    public void OnUpdate()
    {
      foreach (IEntity entity in (IEnumerable<IEntity>) this._frameQuery)
      {
        FrameAddon addon = entity.GetAddon<FrameAddon>();
        if (addon.FrameTickAmount > 0)
        {
          addon.FrameTick -= this._delta.Delta;
          if (addon.FrameTick <= 0)
          {
            addon.FrameTick += addon.FrameTickAmount;
            ++addon.Frame;
            if (addon.Frame >= addon.FrameCount)
              addon.Frame = addon.StartingFrame;
          }
          entity.Update<FrameAddon>(addon);
        }
      }
    }
  }
}
