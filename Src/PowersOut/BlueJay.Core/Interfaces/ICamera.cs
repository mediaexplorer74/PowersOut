
// Type: BlueJay.Core.Interfaces.ICamera
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace BlueJay.Core.Interfaces
{
  public interface ICamera
  {
    Vector2 Position { get; set; }

    float Zoom { get; set; }

    Matrix GetTransformMatrix { get; }

    Matrix GetViewMatrix(Vector2 parallaxFactor);

    Vector2 ToWorld(Vector2 pos);

    Vector2 ToScreen(Vector2 pos);
  }
}
