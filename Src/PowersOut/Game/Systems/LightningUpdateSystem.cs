
// Type: GameManager.Systems.LightningUpdateSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Interfaces;
using GameManager.Services;
using System;

#nullable enable
namespace GameManager.Systems
{
  public class LightningUpdateSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly Random _rand;
    private readonly IDeltaService _delta;
    private readonly GameService _gameService;
    private readonly SoundService _soundService;
    private int _flashes;
    private int _countdown;

    public LightningUpdateSystem(
      ProfileService profileService,
      Random rand,
      IDeltaService delta,
      GameService gameService,
      SoundService soundService)
    {
      this._profileService = profileService;
      this._rand = rand;
      this._delta = delta;
      this._gameService = gameService;
      this._soundService = soundService;
      this._countdown = 10000;
      this._flashes = 0;
    }

    public void OnUpdate()
    {
      if (this._gameService.PlayingStartSequence)
        return;
      DateTime now = DateTime.Now;
      this._countdown -= this._delta.Delta;
      if (this._countdown > 0)
        return;
      this._gameService.ShowLightningFlash = !this._gameService.ShowLightningFlash;
      if (!this._gameService.ShowLightningFlash)
      {
        if (this._flashes == 0)
        {
          this._soundService.PlayThunder();
          this._flashes = this._rand.Next(0, 3);
        }
        else
          --this._flashes;
      }
      this._countdown += this._gameService.ShowLightningFlash ? this._rand.Next(100, 150) : (this._flashes == 0 ? this._rand.Next(15000, 30000) : this._rand.Next(200, 500));
      this._profileService.Profile[nameof (LightningUpdateSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
