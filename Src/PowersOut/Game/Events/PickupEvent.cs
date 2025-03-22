
// Type: GameManager.Events.PickupEvent
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

#nullable disable
namespace GameManager.Events
{
  public class PickupEvent
  {
    public Pickup Pickup { get; }

    public PickupEvent(Pickup pickup) => this.Pickup = pickup;
  }
}
