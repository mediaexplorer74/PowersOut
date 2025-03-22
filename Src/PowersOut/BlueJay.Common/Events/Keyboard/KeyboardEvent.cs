
// Type: BlueJay.Common.Events.Keyboard.KeyboardEvent
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using Microsoft.Xna.Framework.Input;

#nullable disable
namespace BlueJay.Common.Events.Keyboard
{
  public abstract class KeyboardEvent
  {
    public Keys Key { get; set; }

    public bool CapsLock { get; set; }

    public bool NumLock { get; set; }

    public bool Shift { get; set; }

    public bool Ctrl { get; set; }

    public bool Alt { get; set; }
  }
}
