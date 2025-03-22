
// Type: BlueJay.Core.Camera
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core.Interfaces;
using Microsoft.Xna.Framework;

#nullable disable
namespace BlueJay.Core
{
  public class Camera : ICamera
  {
    private float _zoom = 1f;

    public Vector2 Position { get; set; }

    public float Zoom
    {
      get => this._zoom;
      set => this._zoom = MathHelper.Clamp(value, 0.35f, 3f);
    }

    public virtual Matrix GetTransformMatrix => this.GetViewMatrix(Vector2.One);

    public Matrix GetViewMatrix(Vector2 parallaxFactor)
    {
      return Matrix.CreateTranslation(new Vector3(-this.Position * parallaxFactor, 0.0f))
                * Matrix.CreateScale(this.Zoom);
    }

    public Vector2 ToWorld(Vector2 pos)
    {
      return Vector2.Transform(pos, Matrix.Invert(this.GetTransformMatrix));
    }

    public Vector2 ToScreen(Vector2 pos) => Vector2.Transform(pos, this.GetTransformMatrix);
  }
}
