
// Type: BlueJay.Core.SpriteBatchExtension
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using Microsoft.Xna.Framework;
using System;

#nullable enable
namespace BlueJay.Core
{
  public class SpriteBatchExtension
  {
    private readonly ITexture2DContainer _pixel;
    private readonly ISpriteBatchContainer _batch;

    public SpriteBatchExtension(
      IGraphicsDeviceContainer graphicsContainer,
      ISpriteBatchContainer batch)
    {
      this._batch = batch;
      this._pixel = graphicsContainer.CreateRectangle(1, 1, new Color?(Color.White));
    }

    public virtual void DrawRectangle(int width, int height, Vector2 position, Color color)
    {
      if (this._pixel == null)
        return;
      this._batch.Draw(this._pixel, new Rectangle((int) Math.Round((double) position.X), (int) Math.Round((double) position.Y), width, height), color);
    }

    public virtual void DrawRectangle(Size size, Vector2 position, Color color)
    {
      this.DrawRectangle(size.Width, size.Height, position, color);
    }
  }
}
