
// Type: GameManager.Data.LdtkEnum
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace GameManager.Data
{
  public class LdtkEnum
  {
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; }

    [JsonPropertyName("uid")]
    public int Uid { get; set; }

    [JsonPropertyName("values")]
    public IEnumerable<LdtkEnumItem> Values { get; set; }
  }
}
