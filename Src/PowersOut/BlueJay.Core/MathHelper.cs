
// Type: BlueJay.Core.MathHelper
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;

#nullable disable
namespace BlueJay.Core
{
  public static class MathHelper
  {
    public static float Clamp(float val, float min, float max)
    {
      if (float.IsNaN(val))
        throw new ArgumentOutOfRangeException(nameof (val));
      if (float.IsNaN(min))
        throw new ArgumentOutOfRangeException(nameof (min));
      if (float.IsNaN(max))
        throw new ArgumentOutOfRangeException(nameof (max));
      if ((double) min > (double) max)
        throw new ArgumentException("min cannot be greater than max");
      if ((double) val < (double) min)
        return min;
      return (double) val <= (double) max ? val : max;
    }

    public static int Clamp(int val, int min, int max)
    {
      if (val < min)
        return min;
      return val <= max ? val : max;
    }
  }
}
