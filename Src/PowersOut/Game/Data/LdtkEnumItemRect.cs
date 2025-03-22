
// Type: GameManager.Data.LdtkEnumItemRect
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using System.Text.Json.Serialization;

#nullable disable
namespace GameManager.Data
{
  public class LdtkEnumItemRect
  {
    [JsonPropertyName("tilesetUid")]
    public int TilesetUid { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("w")]
    public int Width { get; set; }

    [JsonPropertyName("h")]
    public int Height { get; set; }
  }
}
