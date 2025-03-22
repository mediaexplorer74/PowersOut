
// Type: BlueJay.Core.ContentExtensions
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using Microsoft.Xna.Framework.Graphics;

#nullable enable
namespace BlueJay.Core
{
  public static class ContentExtensions
  {
    public static ITexture2DContainer AsContainer(this Texture2D texture)
    {
      return (ITexture2DContainer) new Texture2DContainer()
      {
        Current = texture
      };
    }

    public static ISpriteFontContainer AsContainer(this SpriteFont spriteFont)
    {
      return (ISpriteFontContainer) new SpriteFontContainer()
      {
        Current = spriteFont
      };
    }
  }
}
