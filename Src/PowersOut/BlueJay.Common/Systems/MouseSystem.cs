
// Type: BlueJay.Common.Systems.MouseSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Common.Events.Mouse;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class MouseSystem : IUpdateSystem, ISystem
  {
    private readonly Dictionary<MouseEvent.ButtonType, bool> _pressed;
    private readonly IEventQueue _queue;

    private Point PreviousPosition { get; set; }

    private int PreviousScrollWheelValue { get; set; }

    public AddonKey Key => AddonKey.None;

    public List<string> Layers => new List<string>();

    public MouseSystem(IEventQueue queue)
    {
      this._queue = queue;
      this._pressed = EnumHelper.GenerateEnumDictionary<MouseEvent.ButtonType, bool>(false);
      this.PreviousPosition = Point.Zero;
      this.PreviousScrollWheelValue = 0;
    }

    public void OnUpdate()
    {
      MouseState state1 = Microsoft.Xna.Framework.Input.Mouse.GetState();
      foreach (KeyValuePair<MouseEvent.ButtonType, bool> keyValuePair in Enumerable.ToArray<KeyValuePair<MouseEvent.ButtonType, bool>>((IEnumerable<KeyValuePair<MouseEvent.ButtonType, bool>>) this._pressed))
      {
        ButtonState state2 = this.GetState(state1, keyValuePair.Key);
        if (state2 == ButtonState.Pressed && !keyValuePair.Value)
        {
          IEventQueue queue = this._queue;
          MouseDownEvent evt = new MouseDownEvent();
          evt.Position = state1.Position;
          evt.Button = new MouseEvent.ButtonType?(keyValuePair.Key);
          queue.DispatchEvent<MouseDownEvent>(evt);
          this._pressed[keyValuePair.Key] = true;
        }
        else if (state2 == ButtonState.Released && keyValuePair.Value)
        {
          IEventQueue queue = this._queue;
          MouseUpEvent evt = new MouseUpEvent();
          evt.Position = state1.Position;
          evt.Button = new MouseEvent.ButtonType?(keyValuePair.Key);
          queue.DispatchEvent<MouseUpEvent>(evt);
          this._pressed[keyValuePair.Key] = false;
        }
      }
      if (this.PreviousPosition != state1.Position)
      {
        MouseEvent.ButtonType? nullable = new MouseEvent.ButtonType?();
        if (state1.LeftButton == ButtonState.Pressed)
          nullable = new MouseEvent.ButtonType?(MouseEvent.ButtonType.Left);
        else if (state1.RightButton == ButtonState.Pressed)
          nullable = new MouseEvent.ButtonType?(MouseEvent.ButtonType.Right);
        else if (state1.MiddleButton == ButtonState.Pressed)
          nullable = new MouseEvent.ButtonType?(MouseEvent.ButtonType.Middle);
        else if (state1.XButton1 == ButtonState.Pressed)
          nullable = new MouseEvent.ButtonType?(MouseEvent.ButtonType.XButton1);
        else if (state1.XButton2 == ButtonState.Pressed)
          nullable = new MouseEvent.ButtonType?(MouseEvent.ButtonType.XButton2);
        IEventQueue queue = this._queue;
        MouseMoveEvent evt = new MouseMoveEvent();
        evt.Position = state1.Position;
        evt.PreviousPosition = new Point?(this.PreviousPosition);
        evt.Button = nullable;
        queue.DispatchEvent<MouseMoveEvent>(evt);
      }
      if (this.PreviousScrollWheelValue != state1.ScrollWheelValue)
        this._queue.DispatchEvent<ScrollEvent>(new ScrollEvent()
        {
          PreviousScrollWheelValue = this.PreviousScrollWheelValue,
          ScrollWheelValue = state1.ScrollWheelValue,
          Position = state1.Position
        });
      this.PreviousPosition = state1.Position;
      this.PreviousScrollWheelValue = state1.ScrollWheelValue;
    }

    private ButtonState GetState(MouseState state, MouseEvent.ButtonType type)
    {
      switch (type)
      {
        case MouseEvent.ButtonType.Right:
          return state.RightButton;
        case MouseEvent.ButtonType.Middle:
          return state.MiddleButton;
        case MouseEvent.ButtonType.Left:
          return state.LeftButton;
        case MouseEvent.ButtonType.XButton1:
          return state.XButton1;
        case MouseEvent.ButtonType.XButton2:
          return state.XButton2;
        default:
          throw new ArgumentException("The enum type was not defined in the switch", nameof (type));
      }
    }
  }
}
