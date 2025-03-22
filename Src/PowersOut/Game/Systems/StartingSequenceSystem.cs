
// Type: GameManager.Systems.StartingSequenceSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Interfaces;
using BlueJay.Events.Interfaces;
using GameManager.Addon;
using GameManager.Events;
using GameManager.Services;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Systems
{
  public class StartingSequenceSystem : IUpdateSystem, ISystem
  {
    private readonly IEventQueue _eventQueue;
    private readonly IDeltaService _deltaService;
    private readonly SoundService _soundService;
    private readonly GameService _gameService;
    private readonly IQuery<StartingBedAddon> _startingBedQuery;
    private readonly IQuery<PlayerAddon> _playerQuery;
    private int? _countdown;
    private int _flashes;

    public StartingSequenceSystem(
      IDeltaService deltaService,
      SoundService soundService,
      GameService gameService,
      IEventQueue eventQueue,
      IQuery<StartingBedAddon> startingBedQuery,
      IQuery<PlayerAddon> playerQuery)
    {
      this._deltaService = deltaService;
      this._soundService = soundService;
      this._gameService = gameService;
      this._eventQueue = eventQueue;
      this._startingBedQuery = startingBedQuery;
      this._playerQuery = playerQuery;
      this._countdown = new int?();
      this._flashes = 0;
    }

    public void OnUpdate()
    {
      if (!this._gameService.PlayingStartSequence)
        return;
      IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._startingBedQuery);
      if (entity == null)
        return;
      StartingBedAddon addon1 = entity.GetAddon<StartingBedAddon>();
      addon1.Countdown -= this._deltaService.Delta;
      if (addon1.Countdown <= 0)
      {
        switch (addon1.State)
        {
          case 0:
            FrameArrayAddon addon2 = entity.GetAddon<FrameArrayAddon>() with
            {
              Frames = new int[1]{ 1 }
            };
            entity.Update<FrameArrayAddon>(addon2);
            this._flashes = 2;
            this._countdown = new int?(500);
            this._soundService.PlayThunder(1.5f);
            break;
          case 1:
            FrameArrayAddon addon3 = entity.GetAddon<FrameArrayAddon>() with
            {
              Frames = new int[1]{ 2 }
            };
            entity.Update<FrameArrayAddon>(addon3);
            Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._playerQuery)?.Remove<SkipRenderAddon>();
            this._eventQueue.DispatchEvent<TriggerSpeechBubbleEvent>(new TriggerSpeechBubbleEvent(entity.GetAddon<TextAddon>().Text, entity.GetAddon<ExpressionAddon>().Expression));
            entity.Remove<ExpressionAddon>();
            entity.Remove<TextAddon>();
            this._gameService.PlayingStartSequence = false;
            this._gameService.CanControlPlayer = true;
            break;
        }
        ++addon1.State;
        addon1.Countdown += 3000;
      }
      entity.Update<StartingBedAddon>(addon1);
      if (!this._countdown.HasValue || this._flashes <= 0)
        return;
      int? countdown = this._countdown;
      int delta = this._deltaService.Delta;
      this._countdown = countdown.HasValue ? new int?(countdown.GetValueOrDefault() - delta) : new int?();
      countdown = this._countdown;
      int num = 0;
      if (!(countdown.GetValueOrDefault() <= num & countdown.HasValue))
        return;
      countdown = this._countdown;
      this._countdown = countdown.HasValue ? new int?(countdown.GetValueOrDefault() + 250) : new int?();
      this._gameService.ShowLightningFlash = !this._gameService.ShowLightningFlash;
      if (this._gameService.ShowLightningFlash)
        return;
      --this._flashes;
    }
  }
}
