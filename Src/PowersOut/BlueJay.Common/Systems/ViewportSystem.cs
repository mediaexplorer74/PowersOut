
// Type: BlueJay.Common.Systems.ViewportSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Events;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class ViewportSystem : IUpdateSystem, ISystem
  {
    private readonly GraphicsDevice _graphics;
    private readonly IEventQueue _queue;
    private Size _previous;

    public AddonKey Key => AddonKey.None;

    public List<string> Layers => new List<string>();

    public ViewportSystem(GraphicsDevice graphics, IEventQueue queue)
    {
      this._graphics = graphics;
      this._queue = queue;
      this._previous = Size.Empty;
    }

    public void OnUpdate()
    {
      Viewport viewport = this._graphics.Viewport;
      int width = viewport.Width;
      viewport = this._graphics.Viewport;
      int height = viewport.Height;
      Size size = new Size(width, height);
      if (!(this._previous != size))
        return;
      this._queue.DispatchEvent<ViewportChangeEvent>(new ViewportChangeEvent()
      {
        Current = size,
        Previous = this._previous
      });
      this._previous = size;
    }
  }
}
