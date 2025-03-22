
// Type: BlueJay.Events.EventListenerWeight
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Modded by [M]edia[E]xplorer

using BlueJay.Events.Interfaces;

#nullable enable
namespace BlueJay.Events
{
  internal class EventListenerWeight
  {
    public IEventListener EventListener { get; private set; }

    public int Weight { get; private set; }

    public EventListenerWeight(IEventListener eventListener, int weight)
    {
      this.EventListener = eventListener;
      this.Weight = weight;
    }
  }
}
