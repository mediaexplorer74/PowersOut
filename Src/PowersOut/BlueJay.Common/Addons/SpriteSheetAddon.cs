
// Type: BlueJay.Common.Addons.SpriteSheetAddon
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using System;
using System.Runtime.CompilerServices;

#nullable enable
namespace BlueJay.Common.Addons
{
  public struct SpriteSheetAddon : IAddon
  {
    public int Rows { get; set; }

    public int Cols { get; set; }

    public SpriteSheetAddon(int cols, int rows = 1)
    {
      if (rows <= 0)
        throw new ArgumentOutOfRangeException(nameof (rows), "Should be greater than 0");
      if (cols <= 0)
        throw new ArgumentOutOfRangeException(nameof (cols), "Should be greater than 0");
      this.Rows = rows;
      this.Cols = cols;
    }

        public override string ToString()
        {
            return $"SpriteSheet | Rows: {this.Rows}, Cols: {this.Cols}";
        }
  }
}
