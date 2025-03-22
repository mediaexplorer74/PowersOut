
// Type: GameManager.Addon.LevelAddon
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;

#nullable enable
namespace GameManager.Addon
{
  public struct LevelAddon(string level) : IAddon
  {
    public string Level = level;
  }
}
