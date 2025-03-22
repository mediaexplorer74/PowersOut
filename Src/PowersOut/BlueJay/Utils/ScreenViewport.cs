
// Type: BlueJay.Utils.ScreenViewport
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Assembly location: BlueJay.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using Microsoft.Xna.Framework.Graphics;

#nullable enable
namespace BlueJay.Utils
{
  internal class ScreenViewport : IScreenViewport
  {
    private readonly GraphicsDevice _graphics;

    public int Width => this._graphics.Viewport.Width;

    public int Height => this._graphics.Viewport.Height;

    public ScreenViewport(GraphicsDevice graphics) => this._graphics = graphics;
  }
}
