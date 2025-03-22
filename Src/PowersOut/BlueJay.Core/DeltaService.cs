
// Type: BlueJay.Core.DeltaService
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using BlueJay.Core.Interfaces;

#nullable disable
namespace BlueJay.Core
{
  public class DeltaService : IDeltaService
  {
    public int Delta { get; set; }

    public double DeltaSeconds { get; set; }
  }
}
