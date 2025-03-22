
// Type: BlueJay.Common.Systems.TouchSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Events.Touch;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class TouchSystem : IUpdateSystem, ISystem
  {
    private readonly IEventQueue _queue;

    public AddonKey Key => AddonKey.None;

    public List<string> Layers => new List<string>();

    public TouchSystem(IEventQueue queue) => this._queue = queue;

    public void OnUpdate()
    {
      TouchCollection state = TouchPanel.GetState();
      if (state.Count != 1)
        return;
      TouchLocation touchLocation = state[0];
      switch (touchLocation.State)
      {
        case TouchLocationState.Moved:
          IEventQueue queue1 = this._queue;
          TouchMoveEvent evt1 = new TouchMoveEvent();
          evt1.Position = touchLocation.Position;
          evt1.Pressure = touchLocation.Pressure;
          queue1.DispatchEvent<TouchMoveEvent>(evt1);
          break;
        case TouchLocationState.Pressed:
          IEventQueue queue2 = this._queue;
          TouchDownEvent evt2 = new TouchDownEvent();
          evt2.Position = touchLocation.Position;
          evt2.Pressure = touchLocation.Pressure;
          queue2.DispatchEvent<TouchDownEvent>(evt2);
          break;
        case TouchLocationState.Released:
          IEventQueue queue3 = this._queue;
          TouchUpEvent evt3 = new TouchUpEvent();
          evt3.Position = touchLocation.Position;
          evt3.Pressure = touchLocation.Pressure;
          queue3.DispatchEvent<TouchUpEvent>(evt3);
          break;
      }
    }
  }
}
