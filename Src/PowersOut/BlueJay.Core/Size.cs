
// Type: BlueJay.Core.Size
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;

#nullable enable
namespace BlueJay.Core
{
  public struct Size(int width, int height) : IEquatable<Size>
  {
    public int Width { get; set; } = width;

    public int Height { get; set; } = height;

    public Size(int size)
      : this(size, size)
    {
    }

    public Size(Point point)
      : this(point.X, point.Y)
    {
    }

    public Size(Vector2 position)
      : this((int) position.X, (int) position.Y)
    {
    }

    public Point ToPoint() => new Point(this.Width, this.Height);

    public Vector2 ToVector2() => new Vector2((float) this.Width, (float) this.Height);

    public bool Equals(Size other) => this.Width == other.Width && this.Height == other.Height;

    public override int GetHashCode()
    {
      int num = this.Width;
      int hashCode1 = num.GetHashCode();
      num = this.Height;
      int hashCode2 = num.GetHashCode();
      return hashCode1 + hashCode2;
    }

    public override bool Equals(object obj) => obj is Size other && this.Equals(other);

    public static Size operator +(Size ls, Size rs)
    {
      return new Size(ls.Width + rs.Width, ls.Height + rs.Height);
    }

    public static Size operator -(Size ls, Size rs)
    {
      return new Size(ls.Width - rs.Width, ls.Height - rs.Height);
    }

    public static Size operator *(Size ls, Size rs)
    {
      return new Size(ls.Width * rs.Width, ls.Height * rs.Height);
    }

    public static bool operator ==(Size ls, Size rs) => ls.Equals(rs);

    public static bool operator !=(Size ls, Size rs) => !ls.Equals(rs);

    public static Size operator /(Size ls, Size rs)
    {
      if (rs.Width == 0 || rs.Height == 0)
        throw new DivideByZeroException();
      return new Size(ls.Width / rs.Width, ls.Height / rs.Height);
    }

    public static Size operator %(Size ls, Size rs)
    {
      if (rs.Width == 0 || rs.Height == 0)
        throw new DivideByZeroException();
      return new Size(ls.Width % rs.Width, ls.Height % rs.Height);
    }

    public static Size Empty => new Size(0);
  }
}
