
// Type: BlueJay.Core.RectangleHelper
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using Microsoft.Xna.Framework;

#nullable disable
namespace BlueJay.Core
{
  public static class RectangleHelper
  {
    public static RectangleSide SideIntersection(Rectangle self, Rectangle target)
    {
      return RectangleHelper.SideIntersection(self, target, out Rectangle _);
    }

    public static RectangleSide SideIntersection(
      Rectangle self,
      Rectangle target,
      out Rectangle intersection)
    {
      intersection = Rectangle.Intersect(self, target);
      if (intersection == Rectangle.Empty)
        return RectangleSide.None;
      if (intersection.Y == target.Y)
        return RectangleSide.Top;
      if (intersection.Y + intersection.Height == target.Y + target.Height)
        return RectangleSide.Bottom;
      if (intersection.X == target.X)
        return RectangleSide.Left;
      return intersection.X + intersection.Width == target.X + target.Width ? RectangleSide.Right : RectangleSide.None;
    }
  }
}
