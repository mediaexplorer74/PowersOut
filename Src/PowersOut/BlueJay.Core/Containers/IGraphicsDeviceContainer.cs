
// Type: BlueJay.Core.Containers.IGraphicsDeviceContainer
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using BlueJay.Core.Container;

#nullable enable
namespace BlueJay.Core.Containers
{
  public interface IGraphicsDeviceContainer
  {
    ITexture2DContainer CreateTexture2D(int width, int height);
  }
}
