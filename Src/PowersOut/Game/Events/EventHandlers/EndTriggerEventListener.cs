
// Type: GameManager.Events.EventHandlers.EndTriggerEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Events;
using BlueJay.Events.Interfaces;
using BlueJay.Interfaces;
using GameManager.Views;

#nullable enable
namespace GameManager.Events.EventHandlers
{
  public class EndTriggerEventListener : EventListener<EndEvent>
  {
    private readonly IViewCollection _viewCollection;

    public EndTriggerEventListener(IViewCollection viewCollection)
    {
      this._viewCollection = viewCollection;
    }

    public override void Process(IEvent<EndEvent> evt)
    {
      this._viewCollection.SetCurrent<EndView>();
    }
  }
}
