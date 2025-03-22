
// Type: BlueJay.Common.Addons.PositionAddon
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace BlueJay.Common.Addons
{
    public struct PositionAddon : IAddon
    {
        public Vector2 Position { get; set; }

        public PositionAddon(int x, int y) => this.Position = new Vector2((float)x, (float)y);

        public PositionAddon(Vector2 position) => this.Position = position;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Position | X: ");
            sb.Append(this.Position.X);
            sb.Append(", Y: ");
            sb.Append(this.Position.Y);
            return sb.ToString();
        }
    }
}
