
// Type: GameManager.Systems.ViewportCameraSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Interfaces;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using GameManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Systems
{
  public class ViewportCameraSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly ICamera _camera;
    private readonly GameService _gameService;
    private readonly IQuery<PlayerAddon> _playerQuery;

    public ViewportCameraSystem(
      ProfileService profileService,
      ICamera camera,
      GameService gameService,
      IQuery<PlayerAddon> playerQuery)
    {
      this._profileService = profileService;
      this._camera = camera;
      this._gameService = gameService;
      this._playerQuery = playerQuery;
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._playerQuery);
      if (entity == null)
        return;
      PositionAddon addon = entity.GetAddon<PositionAddon>();
      Size size = this._gameService.LevelBounds;
      int width1 = size.Width;
      size = this._gameService.Resolution;
      int width2 = size.Width;
      int max1 = width1 - width2;
      size = this._gameService.LevelBounds;
      int height1 = size.Height;
      size = this._gameService.Resolution;
      int height2 = size.Height;
      int max2 = height1 - height2;
      ICamera camera = this._camera;
      double x1 = (double) addon.Position.X;
      size = this._gameService.Resolution;
      double num1 = (double) (size.Width / 2);
      double x2 = (double) Microsoft.Xna.Framework.MathHelper.Clamp((float) (x1 - num1), 0.0f, (float) max1);
      double y1 = (double) addon.Position.Y;
      size = this._gameService.Resolution;
      double num2 = (double) (size.Height / 2);
      double y2 = (double) Microsoft.Xna.Framework.MathHelper.Clamp((float) (y1 - num2), 0.0f, (float) max2);
      Vector2 vector2 = new Vector2((float) x2, (float) y2);
      camera.Position = vector2;
      this._profileService.Profile[nameof (ViewportCameraSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
