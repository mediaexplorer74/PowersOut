
// Type: GameManager.Systems.LightningDrawSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Services;
using System;

#nullable enable
namespace GameManager.Systems
{
  public class LightningDrawSystem : IDrawSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly GraphicsDevice _graphics;
    private readonly RenderTargetService _renderTargetService;
    private readonly GameService _gameService;

    public LightningDrawSystem(
      ProfileService profileService,
      GraphicsDevice graphics,
      RenderTargetService renderTargetService,
      GameService gameService)
    {
      this._profileService = profileService;
      this._graphics = graphics;
      this._renderTargetService = renderTargetService;
      this._gameService = gameService;
    }

    public void OnDraw()
    {
      DateTime now = DateTime.Now;
      this._graphics.SetRenderTarget(this._renderTargetService.LightTarget);
      this._graphics.Clear(Color.White * (this._gameService.ShowLightningFlash ? 1f : 0.3f));
      this._graphics.SetRenderTarget((RenderTarget2D) null);
      this._profileService.Profile[nameof (LightningDrawSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
