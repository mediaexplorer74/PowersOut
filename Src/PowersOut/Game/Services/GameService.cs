
// Type: GameManager.Services.GameService
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Core;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Services
{
  public class GameService
  {
    public string CurrentLevel { get; set; }

    public Dictionary<string, Size> LevelSizes { get; set; }

    public Size Resolution { get; set; }

    public Size LevelBounds => this.LevelSizes[this.CurrentLevel];

    public bool ShowLightningFlash { get; set; }

    public bool ShowFlashLight { get; set; }

    public bool HasFlashLight { get; set; }

    public List<Pickup> FoundKeys { get; set; }

    public bool CanControlPlayer { get; set; }

    public bool PlayingStartSequence { get; set; }

    public bool PlayEndGame { get; set; }

    public GameService()
    {
      this.Resolution = new Size(256, 256);
      this.CurrentLevel = string.Empty;
      this.LevelSizes = new Dictionary<string, Size>();
      this.ShowLightningFlash = false;
      this.ShowFlashLight = false;
      this.HasFlashLight = false;
      this.CanControlPlayer = false;
      this.PlayingStartSequence = true;
      this.PlayEndGame = false;
      this.FoundKeys = new List<Pickup>();
    }
  }
}
