
// Type: GameManager.Events.EventHandlers.GamePadButtonUpEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Events.GamePad;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using GameManager.Services;

#nullable enable
namespace GameManager.Events.EventHandlers
{
  public class GamePadButtonUpEventListener : EventListener<GamePadButtonUpEvent>
  {
    private readonly GameService _gameService;

    public GamePadButtonUpEventListener(GameService gameService) => this._gameService = gameService;

    public override void Process(IEvent<GamePadButtonUpEvent> evt)
    {
      if (!this._gameService.CanControlPlayer || evt.Data.Type != GamePadButtonEvent.ButtonType.X)
        return;
      this._gameService.ShowFlashLight = false;
    }
  }
}
