
// Type: BlueJay.Common.Events.GamePad.GamePadTriggerEvent
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using Microsoft.Xna.Framework;

#nullable disable
namespace BlueJay.Common.Events.GamePad
{
  public sealed class GamePadTriggerEvent
  {
    public float Value { get; set; }

    public float PreviousValue { get; set; }

    public GamePadTriggerEvent.TriggerType Type { get; set; }

    public PlayerIndex Index { get; set; }

    public enum TriggerType
    {
      Left,
      Right,
    }
  }
}
