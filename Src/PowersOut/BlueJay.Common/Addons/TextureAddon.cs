
// Type: BlueJay.Common.Addons.TextureAddon
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Container;

#nullable enable
namespace BlueJay.Common.Addons
{
  public struct TextureAddon(ITexture2DContainer texture) : IAddon
  {
    public ITexture2DContainer Texture { get; set; } = texture;

    public override string ToString() => "Texture | " + this.Texture.Current?.Name;
  }
}
