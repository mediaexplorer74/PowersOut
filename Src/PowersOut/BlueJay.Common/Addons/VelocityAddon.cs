
// Type: BlueJay.Common.Addons.VelocityAddon
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
    public struct VelocityAddon(Vector2 velocity) : IAddon
    {
        public Vector2 Velocity = velocity;

        public VelocityAddon(int x, int y)
          : this(new Vector2((float)x, (float)y))
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Velocity | X: ");
            sb.Append(this.Velocity.X);
            sb.Append(", Y: ");
            sb.Append(this.Velocity.Y);
            return sb.ToString();
        }
    }
}
