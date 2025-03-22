
// Type: GameManager.Systems.FullRenderSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Containers;
using BlueJay.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Services;
using System;

#nullable enable
namespace GameManager.Systems
{
  public class FullRenderSystem : IDrawSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly RenderTargetService _renderTargetService;
    private readonly ISpriteBatchContainer _spriteBatch;
    private readonly Effect _effect;

    public FullRenderSystem(
      ProfileService profileService,
      IContentManagerContainer content,
      RenderTargetService renderTargetService,
      ISpriteBatchContainer spriteBatch)
    {
      this._profileService = profileService;
      this._renderTargetService = renderTargetService;
      this._spriteBatch = spriteBatch;
      this._effect = content.Load<Effect>("LightingShader");
    }

    public void OnDraw()
    {
      DateTime now = DateTime.Now;
      this._effect.Parameters["LightMapTexture"].SetValue((Texture) this._renderTargetService.LightTarget);
      this._effect.Parameters["FlashLightTexture"].SetValue((Texture) this._renderTargetService.FlashLightTarget);
      this._spriteBatch.Begin(blendState: BlendState.AlphaBlend, effect: this._effect);
      this._spriteBatch.Draw(this._renderTargetService.ScreenTarget.AsContainer(), Vector2.Zero, Color.White);
      this._spriteBatch.End();
      this._profileService.Profile[nameof (FullRenderSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
