
// Type: BlueJay.Common.Events.ViewportChangeEvent
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core;

#nullable disable
namespace BlueJay.Common.Events
{
  public sealed class ViewportChangeEvent
  {
    public Size Current { get; set; }

    public Size Previous { get; set; }
  }
}
