
// Type: BlueJay.Common.Addons.FrameAddon
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace BlueJay.Common.Addons
{
    public struct FrameAddon : IAddon
    {
        public int Frame { get; set; }

        public int StartingFrame { get; set; }

        public int FrameTick { get; set; }

        public int FrameCount { get; set; }

        public int FrameTickAmount { get; set; }

        public FrameAddon(int frameCount, int frameTickAmount, int frame = 0, int? startingFrame = null)
        {
            this.Frame = frame;
            this.StartingFrame = startingFrame.GetValueOrDefault(frame);
            this.FrameTick = frameTickAmount;
            this.FrameCount = this.StartingFrame + frameCount;
            this.FrameTickAmount = frameTickAmount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("FrameAddon | Frame: ");
            sb.Append(this.Frame);
            sb.Append(", FrameCount: ");
            sb.Append(this.FrameCount);
            sb.Append(", FrameTickAmount: ");
            sb.Append(this.FrameTickAmount);
            sb.Append(", FrameTick: ");
            sb.Append(this.FrameTick);
            return sb.ToString();
        }
    }
}
