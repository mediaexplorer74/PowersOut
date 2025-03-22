
// Type: BlueJay.Common.Events.Mouse.MouseEvent
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace BlueJay.Common.Events.Mouse
{
  public abstract class MouseEvent
  {
    public Point Position { get; set; }

    public MouseEvent.ButtonType? Button { get; set; }

    public enum ButtonType
    {
      Right,
      Middle,
      Left,
      XButton1,
      XButton2,
    }
  }
}
