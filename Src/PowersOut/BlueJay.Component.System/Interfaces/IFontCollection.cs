
// Type: BlueJay.Component.System.Interfaces.IFontCollection
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using BlueJay.Core;
using BlueJay.Core.Containers;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System.Interfaces
{
  public interface IFontCollection
  {
    Dictionary<string, ISpriteFontContainer> SpriteFonts { get; }

    Dictionary<string, TextureFont> TextureFonts { get; }
  }
}
