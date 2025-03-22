
// Type: BlueJay.Common.Addons.SpriteEffectsAddon
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using Microsoft.Xna.Framework.Graphics;

#nullable enable
namespace BlueJay.Common.Addons
{
  public struct SpriteEffectsAddon(SpriteEffects effects) : IAddon
  {
    public SpriteEffects Effects { get; set; } = effects;

    public override string ToString() => "Sprite Effect | " + this.Effects.ToString("G");
  }
}
