
// Type: BlueJay.Common.Events.GamePad.GamePadStickEvent
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace BlueJay.Common.Events.GamePad
{
  public sealed class GamePadStickEvent
  {
    public Vector2 Value { get; set; }

    public Vector2 PreviousValue { get; set; }

    public GamePadStickEvent.ThumbStickType Type { get; set; }

    public PlayerIndex Index { get; set; }

    public enum ThumbStickType
    {
      Left,
      Right,
    }
  }
}
