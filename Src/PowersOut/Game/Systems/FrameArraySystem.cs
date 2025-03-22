
// Type: GameManager.Systems.FrameArraySystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Interfaces;
using GameManager.Addon;
using GameManager.Services;
using System;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Systems
{
  public class FrameArraySystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly IDeltaService _deltaService;
    private readonly IQuery<FrameArrayAddon> _frameArrayQuery;

    public FrameArraySystem(
      ProfileService profileService,
      IQuery<FrameArrayAddon> frameArrayQuery,
      IDeltaService deltaService)
    {
      this._profileService = profileService;
      this._deltaService = deltaService;
      this._frameArrayQuery = frameArrayQuery;
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      foreach (IEntity entity in (IEnumerable<IEntity>) this._frameArrayQuery)
      {
        FrameArrayAddon addon = entity.GetAddon<FrameArrayAddon>();
        if (addon.FrameTickAmount > 0)
        {
          addon.FrameTick += this._deltaService.Delta;
          if (addon.FrameTick >= addon.FrameTickAmount)
          {
            addon.FrameTick -= addon.FrameTickAmount;
            ++addon.Frame;
            if (addon.Frame >= addon.Frames.Length)
              addon.Frame = 0;
          }
          entity.Update<FrameArrayAddon>(addon);
        }
      }
      this._profileService.Profile[nameof (FrameArraySystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
