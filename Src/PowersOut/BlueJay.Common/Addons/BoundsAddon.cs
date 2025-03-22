
// Type: BlueJay.Common.Addons.BoundsAddon
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Interfaces;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace BlueJay.Common.Addons
{
    public struct BoundsAddon(Rectangle bounds) : IAddon
    {
        public Rectangle Bounds = bounds;

        public BoundsAddon(int x, int y, int width, int height)
          : this(new Rectangle(x, y, width, height))
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Bounds | X: ");
            sb.Append(this.Bounds.X);
            sb.Append(", Y: ");
            sb.Append(this.Bounds.Y);
            sb.Append(", Width: ");
            sb.Append(this.Bounds.Width);
            sb.Append(", Height: ");
            sb.Append(this.Bounds.Height);
            return sb.ToString();
        }
    }
}
