
// Type: BlueJay.Common.Events.Mouse.MouseMoveEvent
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using Microsoft.Xna.Framework;

#nullable disable
namespace BlueJay.Common.Events.Mouse
{
  public sealed class MouseMoveEvent : MouseEvent
  {
    public Point? PreviousPosition { get; set; }
  }
}
