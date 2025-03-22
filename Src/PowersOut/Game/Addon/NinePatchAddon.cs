﻿
// Type: GameManager.Addon.NinePatchAddon
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core;

#nullable enable
namespace GameManager.Addon
{
  public struct NinePatchAddon(NinePatch ninePatch) : IAddon
  {
    public NinePatch NinePatch = ninePatch;
  }
}
