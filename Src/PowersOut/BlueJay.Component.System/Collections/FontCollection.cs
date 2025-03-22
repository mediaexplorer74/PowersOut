
// Type: BlueJay.Component.System.Collections.FontCollection
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Containers;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System.Collections
{
  internal class FontCollection : IFontCollection
  {
    public Dictionary<string, ISpriteFontContainer> SpriteFonts { get; set; }

    public Dictionary<string, TextureFont> TextureFonts { get; set; }

    public FontCollection()
    {
      this.SpriteFonts = new Dictionary<string, ISpriteFontContainer>();
      this.TextureFonts = new Dictionary<string, TextureFont>();
    }
  }
}
