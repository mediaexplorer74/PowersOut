
// Type: GameManager.Line2
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable enable
namespace GameManager
{
  public class Line2
  {
    public Vector2 Start { get; set; }

    public Vector2 End { get; set; }

    public Line2(Vector2 start, Vector2 end)
    {
      this.Start = start;
      this.End = end;
    }

    public bool Intersects(Line2 other)
    {
      float num1 = (float) (((double) other.End.Y - (double) other.Start.Y) * ((double) this.End.X - (double) this.Start.X) - ((double) other.End.X - (double) other.Start.X) * ((double) this.End.Y - (double) this.Start.Y));
      if ((double) num1 == 0.0)
        return false;
      float num2 = this.Start.Y - other.Start.Y;
      float num3 = this.Start.X - other.Start.X;
      float num4 = (float) (((double) other.End.X - (double) other.Start.X) * (double) num2 - ((double) other.End.Y - (double) other.Start.Y) * (double) num3);
      double num5 = ((double) this.End.X - (double) this.Start.X) * (double) num2 - ((double) this.End.Y - (double) this.Start.Y) * (double) num3;
      float num6 = num4 / num1;
      double num7 = (double) num1;
      float num8 = (float) (num5 / num7);
      return (double) num6 > 0.0 && (double) num6 < 1.0 && (double) num8 > 0.0 && (double) num8 < 1.0;
    }
  }
}
