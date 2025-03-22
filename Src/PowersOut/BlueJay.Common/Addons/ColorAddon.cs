
// Type: BlueJay.Common.Addons.ColorAddon
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
    public struct ColorAddon(Color color) : IAddon
    {
        public Color Color = color;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Color | R: ");
            sb.Append(this.Color.R);
            sb.Append(", G: ");
            sb.Append(this.Color.G);
            sb.Append(", B: ");
            sb.Append(this.Color.B);
            sb.Append(", A: ");
            sb.Append(this.Color.A);
            return sb.ToString();
        }
    }
}
