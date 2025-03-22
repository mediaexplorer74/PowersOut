
// Type: GameManager.Events.RemoveEntityEvent
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;

#nullable enable
namespace GameManager.Events
{
  public class RemoveEntityEvent
  {
    public IEntity Entity { get; set; }

    public RemoveEntityEvent(IEntity entity) => this.Entity = entity;
  }
}
