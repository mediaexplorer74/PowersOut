
// Type: BlueJay.Common.Addons.SizeAddon
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace BlueJay.Common.Addons
{
  public struct SizeAddon : IAddon
  {
    public Size Size { get; set; }

    public SizeAddon(Size size) => this.Size = size;

    public SizeAddon(int size) => this.Size = new Size(size);

    public SizeAddon(int width, int height) => this.Size = new Size(width, height);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Size | Width: ");
            sb.Append(this.Size.Width);
            sb.Append(", Height: ");
            sb.Append(this.Size.Height);
            return sb.ToString();
        }
  }
}
