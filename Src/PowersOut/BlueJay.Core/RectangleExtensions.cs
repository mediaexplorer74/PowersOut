
// Type: BlueJay.Core.RectangleExtensions
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BlueJay.Core
{
  public static class RectangleExtensions
  {
    public static Rectangle Add(this Rectangle rect, Vector2 pos)
    {
      int num1 = (double) pos.X < 0.0 ? (int) Math.Floor((double) pos.X) : (int) Math.Ceiling((double) pos.X);
      int num2 = (double) pos.Y < 0.0 ? (int) Math.Floor((double) pos.Y) : (int) Math.Ceiling((double) pos.Y);
      return new Rectangle(rect.X + num1, rect.Y + num2, rect.Width, rect.Height);
    }

    public static Rectangle Add(this Rectangle rect, Point pos)
    {
      return new Rectangle(rect.X + pos.X, rect.Y + pos.Y, rect.Width, rect.Height);
    }

    public static Rectangle Add(this Rectangle rect, int x, int y)
    {
      return new Rectangle(rect.X + x, rect.Y + y, rect.Width, rect.Height);
    }

    public static Rectangle AddX(this Rectangle rect, int x)
    {
      return new Rectangle(rect.X + x, rect.Y, rect.Width, rect.Height);
    }

    public static Rectangle AddY(this Rectangle rect, int y)
    {
      return new Rectangle(rect.X, rect.Y + y, rect.Width, rect.Height);
    }

    public static Rectangle AddX(this Rectangle rect, float x)
    {
      int num = (double) x < 0.0 ? (int) Math.Floor((double) x) : (int) Math.Ceiling((double) x);
      return new Rectangle(rect.X + num, rect.Y, rect.Width, rect.Height);
    }

    public static Rectangle AddY(this Rectangle rect, float y)
    {
      int num = (double) y < 0.0 ? (int) Math.Floor((double) y) : (int) Math.Ceiling((double) y);
      return new Rectangle(rect.X, rect.Y + num, rect.Width, rect.Height);
    }
  }
}
