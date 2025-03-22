
// Type: GameManager.Systems.BoundsRenderingSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Containers;
using BlueJay.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Services;
using System;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Systems
{
  public class BoundsRenderingSystem : IDrawSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly GameService _gameService;
    private readonly ISpriteBatchContainer _spriteBatch;
    private readonly SpriteBatchExtension _spriteBatchExtension;
    private readonly ICamera _camera;
    private readonly IQuery<PositionAddon, BoundsAddon> _query;

    public BoundsRenderingSystem(
      ProfileService profileService,
      GameService gameService,
      ISpriteBatchContainer spriteBatch,
      SpriteBatchExtension spriteBatchExtension,
      ICamera camera,
      IQuery<PositionAddon, BoundsAddon> query)
    {
      this._profileService = profileService;
      this._gameService = gameService;
      this._spriteBatch = spriteBatch;
      this._spriteBatchExtension = spriteBatchExtension;
      this._camera = camera;
      this._query = query;
    }

    public void OnDraw()
    {
      DateTime now = DateTime.Now;
      this._spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: new Matrix?(this._camera.GetTransformMatrix));
      foreach (IEntity entity in (IEnumerable<IEntity>) this._query.WhereLayer("Player", "Walls_" + this._gameService.CurrentLevel))
      {
        PositionAddon addon1 = entity.GetAddon<PositionAddon>();
        BoundsAddon addon2 = entity.GetAddon<BoundsAddon>();
        Color color = Color.Red * 0.5f;
        Point point = addon1.Position.ToPoint() + new Point(addon2.Bounds.X, addon2.Bounds.Y);
        this._spriteBatchExtension.DrawRectangle(addon2.Bounds.Width, addon2.Bounds.Height, point.ToVector2(), color);
      }
      this._spriteBatch.End();
      this._profileService.Profile[nameof (BoundsRenderingSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
