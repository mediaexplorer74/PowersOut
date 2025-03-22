
// Type: GameManager.Addon.DirectionRayAddon
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Addon
{
  public struct DirectionRayAddon(double offset, float length, Vector2 positionOffset) : IAddon
  {
    public double Offset = offset;
    public float Length = length;
    public Vector2 PositionOffset = positionOffset;
    public List<Ray2> Rays = new List<Ray2>();
  }
}
