
// Type: GameManager.Addon.StartingBedAddon
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;

#nullable disable
namespace GameManager.Addon
{
  public struct StartingBedAddon(int state, int countdown) : IAddon
  {
    public int State { get; set; } = state;

    public int Countdown { get; set; } = countdown;
  }
}
