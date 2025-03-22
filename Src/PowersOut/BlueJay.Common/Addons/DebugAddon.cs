
// Type: BlueJay.Common.Addons.DebugAddon
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;

#nullable disable
namespace BlueJay.Common.Addons
{
  public struct DebugAddon(AddonKey keyIdentifier) : IAddon
  {
    public AddonKey KeyIdentifier = keyIdentifier;
  }
}
