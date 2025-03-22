
// Type: GameManager.Systems.FlashLightRenderSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using BlueJay.Core.Interfaces;
using BlueJay.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Addon;
using GameManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Systems
{
  public class FlashLightRenderSystem : IDrawSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly GraphicsDevice _graphics;
    private readonly ISpriteBatchContainer _spriteBatch;
    private readonly RenderTargetService _renderTargetService;
    private readonly GameService _gameService;
    private readonly ICamera _camera;
    private readonly IQuery<PlayerAddon, PositionAddon, DirectionAddon> _playerQuery;
    private readonly ITexture2DContainer _flashLightTexture;
    private readonly Vector2 _offset;
    private readonly Direction[] _directions;

    public FlashLightRenderSystem(
      ProfileService profileService,
      GraphicsDevice graphics,
      ISpriteBatchContainer spriteBatch,
      RenderTargetService renderTargetService,
      GameService gameService,
      IContentManagerContainer content,
      ICamera camera,
      IQuery<PlayerAddon, PositionAddon, DirectionAddon> playerQuery)
    {
      this._profileService = profileService;
      this._graphics = graphics;
      this._spriteBatch = spriteBatch;
      this._renderTargetService = renderTargetService;
      this._gameService = gameService;
      this._camera = camera;
      this._playerQuery = playerQuery;
      this._flashLightTexture = content.Load<ITexture2DContainer>("character/FlashLightBeam");
      this._offset = new Vector2(54f, 35f);
      this._directions = new Direction[8]
      {
        Direction.NorthEast,
        Direction.East,
        Direction.SouthEast,
        Direction.South,
        Direction.SouthWest,
        Direction.West,
        Direction.NorthWest,
        Direction.North
      };
    }

    public void OnDraw()
    {
      DateTime now = DateTime.Now;
      this._graphics.SetRenderTarget(this._renderTargetService.FlashLightTarget);
      this._graphics.Clear(Color.Transparent);
      IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._playerQuery);
      if (entity != null && this._gameService.ShowFlashLight)
      {
        this._spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: new Matrix?(this._camera.GetTransformMatrix));
        this._spriteBatch.DrawFrame(this._flashLightTexture, entity.GetAddon<PositionAddon>().Position - this._offset, 1, 8, Array.IndexOf<Direction>(this._directions, entity.GetAddon<DirectionAddon>().Direction));
        this._spriteBatch.End();
      }
      this._graphics.SetRenderTarget((RenderTarget2D) null);
      this._profileService.Profile[nameof (FlashLightRenderSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
