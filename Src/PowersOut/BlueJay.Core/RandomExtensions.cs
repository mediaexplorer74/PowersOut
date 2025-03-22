
// Type: BlueJay.Core.RandomExtensions
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;

#nullable enable
namespace BlueJay.Core
{
  public static class RandomExtensions
  {
    public static float NextFloat(this Random rand, float min, float max)
    {
      return (float) rand.NextDouble() * (max - min) + min;
    }
  }
}
