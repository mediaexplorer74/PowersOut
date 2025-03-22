
// Type: GameManager.Systems.ResolutionRenderingSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Containers;
using BlueJay.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Addon;
using GameManager.Services;
using System;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Systems
{
  public class ResolutionRenderingSystem : IDrawSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly RenderTargetService _renderTargetService;
    private readonly ISpriteBatchContainer _spriteBatch;
    private readonly ICamera _camera;
    private readonly GameService _gameService;
    private readonly IQuery<PositionAddon, TextureAddon> _query;

    public ResolutionRenderingSystem(
      ProfileService profileService,
      ISpriteBatchContainer spriteBatch,
      ICamera camera,
      GameService gameService,
      RenderTargetService renderTargetService,
      GraphicsDevice graphicsDevice,
      IQuery<PositionAddon, TextureAddon> query)
    {
      this._profileService = profileService;
      this._graphicsDevice = graphicsDevice;
      this._renderTargetService = renderTargetService;
      this._spriteBatch = spriteBatch;
      this._camera = camera;
      this._gameService = gameService;
      this._query = query;
    }

    public void OnDraw()
    {
      DateTime now = DateTime.Now;
      this._graphicsDevice.SetRenderTarget(this._renderTargetService.ScreenTarget);
      this._graphicsDevice.Clear(Color.Transparent);
      this._spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: new Matrix?(this._camera.GetTransformMatrix));
      foreach (IEntity entity in (IEnumerable<IEntity>) this._query.WhereLayer("Player", "Background_" + this._gameService.CurrentLevel, "Walls_" + this._gameService.CurrentLevel, "Triggers_" + this._gameService.CurrentLevel))
      {
        if (!entity.Contains<SkipRenderAddon>())
        {
          TextureAddon addon1 = entity.GetAddon<TextureAddon>();
          PositionAddon addon2 = entity.GetAddon<PositionAddon>();
          Rectangle rectangle = new Rectangle(0, 0, addon1.Texture.Width, addon1.Texture.Height);
          if (entity.Contains<SourceRectangleAddon>())
            rectangle = entity.GetAddon<SourceRectangleAddon>().SourceRectangle;
          if (entity.Contains<FrameArrayAddon, SpriteSheetAddon>())
          {
            FrameArrayAddon addon3 = entity.GetAddon<FrameArrayAddon>();
            SpriteSheetAddon addon4 = entity.GetAddon<SpriteSheetAddon>();
            this._spriteBatch.DrawFrame(addon1.Texture, addon2.Position, addon4.Rows, addon4.Cols, addon3.Current, new Color?(Color.White));
          }
          else
            this._spriteBatch.Draw(addon1.Texture, addon2.Position, new Rectangle?(rectangle), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        }
      }
      this._spriteBatch.End();
      this._graphicsDevice.SetRenderTarget((RenderTarget2D) null);
      this._profileService.Profile[nameof (ResolutionRenderingSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
