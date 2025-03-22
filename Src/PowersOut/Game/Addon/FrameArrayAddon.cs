
// Type: GameManager.Addon.FrameArrayAddon
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;

#nullable enable
namespace GameManager.Addon
{
  public struct FrameArrayAddon(int[] frames, int frameTickAmount) : IAddon
  {
    public int Frame { get; set; } = 0;

    public int FrameTick { get; set; } = 0;

    public int[] Frames { get; set; } = frames;

    public int FrameTickAmount { get; set; } = frameTickAmount;

    public int Current
    {
      get => this.Frames.Length != 0 ? this.Frames[this.Frame % this.Frames.Length] : 0;
    }
  }
}
