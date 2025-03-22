
// Type: BlueJay.Events.Interfaces.IEvent
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Assembly location: BlueJay.Events.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

#nullable enable
namespace BlueJay.Events.Interfaces
{
  public interface IEvent
  {
    string Name { get; }

    bool IsComplete { get; }

    object? Target { get; }

    void StopPropagation();
  }
}
