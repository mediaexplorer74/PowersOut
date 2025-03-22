
// Type: BlueJay.Core.Containers.GraphicsDeviceContainer
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core.Container;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable enable
namespace BlueJay.Core.Containers
{
  internal class GraphicsDeviceContainer : IGraphicsDeviceContainer
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly GraphicsDevice _graphicsDevice;

    public GraphicsDeviceContainer(IServiceProvider serviceProvider, GraphicsDevice graphicsDevice)
    {
      this._serviceProvider = serviceProvider;
      this._graphicsDevice = graphicsDevice;
    }

    public ITexture2DContainer CreateTexture2D(int width, int height)
    {
      return new Texture2D(this._graphicsDevice, width, height).AsContainer();
    }
  }
}
