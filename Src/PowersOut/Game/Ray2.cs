
// Type: GameManager.Ray2
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;

#nullable enable
namespace GameManager
{
  public class Ray2
  {
    public Vector2 Start { get; set; }

    public Vector2 Direction { get; set; }

    public double Radians { get; set; }

    public Ray2(Vector2 start, double radians)
    {
      this.Start = start;
      this.Direction = new Vector2((float) Math.Cos(radians), (float) Math.Sin(radians));
      this.Radians = radians;
    }

    public Ray2(Vector2 start, Vector2 direction)
    {
      this.Start = start;
      this.Direction = direction;
      this.Radians = Math.Atan2((double) this.Direction.Y, (double) this.Direction.X);
    }

    public Vector2 GetPosition(float distance)
    {
      return this.Start + new Vector2(distance * (float) Math.Cos(this.Radians), distance * (float) Math.Sin(this.Radians));
    }

    public Vector2? IntersectionPoint(Ray2 other)
    {
      if (this.Start == other.Start)
        return new Vector2?(this.Start);
      float num1 = other.Start.X - this.Start.X;
      float num2 = other.Start.Y - this.Start.Y;
      float num3 = (float) ((double) other.Direction.X * (double) this.Direction.Y - (double) other.Direction.Y * (double) this.Direction.X);
      if ((double) num3 != 0.0)
      {
        float num4 = (float) ((double) num2 * (double) other.Direction.X - (double) num1 * (double) other.Direction.Y) / num3;
        float num5 = (float) ((double) num2 * (double) this.Direction.X - (double) num1 * (double) this.Direction.Y) / num3;
        if ((double) num4 >= 0.0 && (double) num5 >= 0.0)
          return new Vector2?(this.Start + this.Direction * num4);
      }
      return new Vector2?();
    }
  }
}
