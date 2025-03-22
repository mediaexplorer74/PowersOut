
// Type: GameManager.Events.LevelTransitionEvent
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable enable
namespace GameManager.Events
{
  public class LevelTransitionEvent
  {
    public string GotoLevel;
    public Vector2 SpawnPosition;

    public LevelTransitionEvent(string gotoLevel, Vector2 spawnPosition)
    {
      this.GotoLevel = gotoLevel;
      this.SpawnPosition = spawnPosition;
    }
  }
}
