
// Type: GameManager.Services.ProfileService
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using System.Collections.Generic;

#nullable enable
namespace GameManager.Services
{
  public class ProfileService
  {
    public Dictionary<string, double> Profile { get; set; }

    public ProfileService() => this.Profile = new Dictionary<string, double>();
  }
}
