
// Type: GameManager.Data.LdtkEnumItem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using System.Text.Json.Serialization;

#nullable enable
namespace GameManager.Data
{
  public class LdtkEnumItem
  {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("tileRect")]
    public LdtkEnumItemRect? TileRect { get; set; }
  }
}
