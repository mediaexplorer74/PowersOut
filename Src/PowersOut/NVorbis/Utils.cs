
// Type: NVorbis.Utils
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;

#nullable disable
namespace NVorbis
{
  internal static class Utils
  {
    internal static int ilog(int x)
    {
      int num = 0;
      for (; x > 0; x >>= 1)
        ++num;
      return num;
    }

    internal static uint BitReverse(uint n) => Utils.BitReverse(n, 32);

    internal static uint BitReverse(uint n, int bits)
    {
      n = (n & 2863311530U) >> 1 | (uint) (((int) n & 1431655765) << 1);
      n = (n & 3435973836U) >> 2 | (uint) (((int) n & 858993459) << 2);
      n = (n & 4042322160U) >> 4 | (uint) (((int) n & 252645135) << 4);
      n = (n & 4278255360U) >> 8 | (uint) (((int) n & 16711935) << 8);
      return (n >> 16 | n << 16) >> 32 - bits;
    }

    internal static float ClipValue(float value, ref bool clipped)
    {
      if ((double) value > 0.99999994039535522)
      {
        clipped = true;
        return 0.99999994f;
      }
      if ((double) value >= -0.99999994039535522)
        return value;
      clipped = true;
      return -0.99999994f;
    }

    internal static float ConvertFromVorbisFloat32(uint bits)
    {
      int num1 = (int) bits >> 31;
      double num2 = (double) ((int) ((bits & 2145386496U) >> 21) - 788);
      return (float) (((long) (bits & 2097151U) ^ (long) num1) + (long) (num1 & 1)) * (float) Math.Pow(2.0, num2);
    }
  }
}
