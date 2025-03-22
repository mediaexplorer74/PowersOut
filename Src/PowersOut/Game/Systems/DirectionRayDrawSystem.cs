
// Type: GameManager.Systems.DirectionRayDrawSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Containers;
using BlueJay.Core.Interfaces;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using GameManager.Services;
using System;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Systems
{
  public class DirectionRayDrawSystem : IDrawSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly ICamera _camera;
    private readonly ISpriteBatchContainer _spriteBatch;
    private readonly SpriteBatchExtension _spriteBatchExtension;
    private readonly IQuery<DirectionRayAddon> _query;

    public DirectionRayDrawSystem(
      ProfileService profileService,
      ICamera camera,
      ISpriteBatchContainer spriteBatch,
      SpriteBatchExtension spriteBatchExtension,
      IQuery<DirectionRayAddon> query)
    {
      this._profileService = profileService;
      this._camera = camera;
      this._spriteBatch = spriteBatch;
      this._spriteBatchExtension = spriteBatchExtension;
      this._query = query;
    }

    public void OnDraw()
    {
      DateTime now = DateTime.Now;
      this._spriteBatch.Begin(transformMatrix: new Matrix?(this._camera.GetTransformMatrix));
      foreach (IEntity entity in (IEnumerable<IEntity>) this._query)
      {
        DirectionRayAddon addon = entity.GetAddon<DirectionRayAddon>();
        foreach (Ray2 ray in addon.Rays)
        {
          Vector2 position = ray.GetPosition(addon.Length);
          this._spriteBatchExtension.DrawRectangle(1, 1, ray.Start, Color.White);
          this._spriteBatchExtension.DrawRectangle(1, 1, position, Color.White);
        }
      }
      this._spriteBatch.End();
      this._profileService.Profile[nameof (DirectionRayDrawSystem)] 
                = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
