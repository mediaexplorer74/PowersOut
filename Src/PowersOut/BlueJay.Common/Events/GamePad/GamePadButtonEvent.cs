
// Type: BlueJay.Common.Events.GamePad.GamePadButtonEvent
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace BlueJay.Common.Events.GamePad
{
  public abstract class GamePadButtonEvent
  {
    public GamePadButtonEvent.ButtonType Type { get; set; }

    public PlayerIndex Index { get; set; }

    public enum ButtonType
    {
      DPadDown,
      DPadLeft,
      DPadRight,
      DPadUp,
      RightShoulder,
      LeftStick,
      LeftShoulder,
      Start,
      X,
      Y,
      RightStick,
      Back,
      A,
      B,
      BigButton,
    }
  }
}
