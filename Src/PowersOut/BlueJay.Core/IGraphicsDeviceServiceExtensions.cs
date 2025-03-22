
// Type: BlueJay.Core.IGraphicsDeviceServiceExtensions
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using Microsoft.Xna.Framework;

#nullable enable
namespace BlueJay.Core
{
  public static class IGraphicsDeviceServiceExtensions
  {
    public static ITexture2DContainer CreateRectangle(
      this IGraphicsDeviceContainer graphicsContainer,
      int width,
      int height,
      Color? color = null)
    {
      color = new Color?(color ?? Color.Black);
      ITexture2DContainer texture2D = graphicsContainer.CreateTexture2D(width, height);
      Color[] data = new Color[width * height];
      for (int index = 0; index < data.Length; ++index)
        data[index] = color.Value;
      texture2D.SetData<Color>(data);
      return texture2D;
    }
  }
}
