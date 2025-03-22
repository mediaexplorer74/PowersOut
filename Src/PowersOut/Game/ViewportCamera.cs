
// Type: GameManager.ViewportCamera
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Core;
using BlueJay.Core.Interfaces;
using BlueJay.Utils;
using Microsoft.Xna.Framework;
using GameManager.Services;

#nullable enable
namespace GameManager
{
  public class ViewportCamera : ICamera
  {
    private readonly GameService _gameService;
    private readonly IScreenViewport _screen;

    public Vector2 Position { get; set; }

    public float Zoom { get; set; }

    public Matrix GetTransformMatrix => this.GetViewMatrix(Vector2.One);

    public ViewportCamera(IScreenViewport screen, GameService gameService)
    {
      this._screen = screen;
      this._gameService = gameService;
      this.Position = Vector2.Zero;
      this.Zoom = 1f;
    }

    public Matrix GetViewMatrix(Vector2 parallaxFactor)
    {
      Matrix translation = Matrix.CreateTranslation(new Vector3(-this.Position * parallaxFactor, 0.0f));
      int num1 = this._screen.Width - this._gameService.Resolution.Width < this._screen.Height - this._gameService.Resolution.Height ? 1 : 0;
      Size resolution;
      double num2;
      if (num1 == 0)
      {
        double height1 = (double) this._screen.Height;
        resolution = this._gameService.Resolution;
        double height2 = (double) resolution.Height;
        num2 = height1 / height2;
      }
      else
      {
        double width1 = (double) this._screen.Width;
        resolution = this._gameService.Resolution;
        double width2 = (double) resolution.Width;
        num2 = width1 / width2;
      }
      float num3 = (float) num2;
      Matrix matrix = translation * Matrix.CreateScale(num3, num3, 1f);
      Vector3 vector3;
      if (num1 == 0)
      {
        double width = (double) this._screen.Width;
        resolution = this._gameService.Resolution;
        double num4 = (double) resolution.Width * (double) num3;
        vector3 = new Vector3((float) ((width - num4) * 0.5), 0.0f, 0.0f);
      }
      else
      {
        double height = (double) this._screen.Height;
        resolution = this._gameService.Resolution;
        double num5 = (double) resolution.Height * (double) num3;
        vector3 = new Vector3(0.0f, (float) ((height - num5) * 0.5), 0.0f);
      }
      Vector3 position = vector3;
      return matrix * Matrix.CreateTranslation(position);
    }

    public Vector2 ToScreen(Vector2 pos) => Vector2.Transform(pos, this.GetTransformMatrix);

    public Vector2 ToWorld(Vector2 pos)
    {
      return Vector2.Transform(pos, Matrix.Invert(this.GetTransformMatrix));
    }
  }
}
