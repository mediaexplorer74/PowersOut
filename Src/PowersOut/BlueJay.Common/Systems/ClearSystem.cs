
// Type: BlueJay.Common.Systems.ClearSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class ClearSystem : IDrawSystem, ISystem
  {
    private readonly GraphicsDevice _graphicsDevice;
    private readonly Color _color;

    public AddonKey Key => AddonKey.None;

    public List<string> Layers => new List<string>();

    public ClearSystem(GraphicsDevice graphicsDevice, Color color)
    {
      this._graphicsDevice = graphicsDevice;
      this._color = color;
    }

    public void OnDraw() => this._graphicsDevice.Clear(this._color);
  }
}
