
// Type: BlueJay.Core.NinePatch
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using BlueJay.Core.Container;
using Microsoft.Xna.Framework;
using System;

#nullable enable
namespace BlueJay.Core
{
  public class NinePatch
  {
    public ITexture2DContainer Texture { get; private set; }

    public Point Break { get; private set; }

    public Rectangle TopLeft => new Rectangle(Point.Zero, this.Break);

    public Rectangle Top => new Rectangle(new Point(this.Break.X, 0), this.Break);

    public Rectangle TopRight => new Rectangle(new Point(this.Break.X * 2, 0), this.Break);

    public Rectangle MiddleLeft => new Rectangle(new Point(0, this.Break.Y), this.Break);

    public Rectangle Middle => new Rectangle(this.Break, this.Break);

    public Rectangle MiddleRight
    {
      get => new Rectangle(new Point(this.Break.X * 2, this.Break.Y), this.Break);
    }

    public Rectangle BottomLeft => new Rectangle(new Point(0, this.Break.Y * 2), this.Break);

    public Rectangle Bottom => new Rectangle(new Point(this.Break.X, this.Break.Y * 2), this.Break);

    public Rectangle BottomRight
    {
      get => new Rectangle(new Point(this.Break.X * 2, this.Break.Y * 2), this.Break);
    }

    public NinePatch(ITexture2DContainer texture)
    {
      if (texture.Width % 3 != 0)
        throw new ArgumentException("Width value needs to be a multiple of 3", nameof (texture));
      this.Texture = texture.Height % 3 == 0 ? texture : throw new ArgumentException("Height value needs to be a multiple of 3", nameof (texture));
      this.Break = new Point(this.Texture.Width / 3, this.Texture.Height / 3);
    }
  }
}
