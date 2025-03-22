
// Type: BlueJay.Common.Systems.GamepadSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Common.Events.GamePad;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class GamepadSystem : IUpdateSystem, ISystem
  {
    private readonly Dictionary<PlayerIndex, GamepadSystem.GamePadHandler> _handlers;

    public AddonKey Key => AddonKey.None;

    public List<string> Layers
    {
      get
      {
        List<string> layers = new List<string>();
        layers.Add(string.Empty);
        return layers;
      }
    }

    public GamepadSystem(IEventQueue queue, IServiceProvider provider)
    {
      Dictionary<PlayerIndex, GamepadSystem.GamePadHandler> dictionary = new Dictionary<PlayerIndex, GamepadSystem.GamePadHandler>();
      dictionary.Add(PlayerIndex.One, ActivatorUtilities.CreateInstance<GamepadSystem.GamePadHandler>(provider, (object) queue));
      dictionary.Add(PlayerIndex.Two, ActivatorUtilities.CreateInstance<GamepadSystem.GamePadHandler>(provider, (object) queue));
      dictionary.Add(PlayerIndex.Three, ActivatorUtilities.CreateInstance<GamepadSystem.GamePadHandler>(provider, (object) queue));
      dictionary.Add(PlayerIndex.Four, ActivatorUtilities.CreateInstance<GamepadSystem.GamePadHandler>(provider, (object) queue));
      this._handlers = dictionary;
    }

    public void OnUpdate()
    {
      foreach (KeyValuePair<PlayerIndex, GamepadSystem.GamePadHandler> keyValuePair in Enumerable.ToArray<KeyValuePair<PlayerIndex, GamepadSystem.GamePadHandler>>((IEnumerable<KeyValuePair<PlayerIndex, GamepadSystem.GamePadHandler>>) this._handlers))
      {
        if (Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(keyValuePair.Key).IsConnected)
        {
          GamePadState state = Microsoft.Xna.Framework.Input.GamePad.GetState(keyValuePair.Key);
          keyValuePair.Value.HandleButtons(state, keyValuePair.Key);
          keyValuePair.Value.HandleTriggerEvents(state, keyValuePair.Key);
          keyValuePair.Value.HandleStickEvents(state, keyValuePair.Key);
        }
      }
    }

    private class GamePadHandler
    {
      private readonly Dictionary<GamePadButtonEvent.ButtonType, bool> _pressed;
      private readonly Dictionary<GamePadTriggerEvent.TriggerType, float> _triggers;
      private readonly Dictionary<GamePadStickEvent.ThumbStickType, Vector2> _sticks;
      private readonly IEventQueue _queue;

      public GamePadHandler(IEventQueue queue)
      {
        this._queue = queue;
        this._pressed = EnumHelper.GenerateEnumDictionary<GamePadButtonEvent.ButtonType, bool>(false);
        this._triggers = EnumHelper.GenerateEnumDictionary<GamePadTriggerEvent.TriggerType, float>(0.0f);
        this._sticks = EnumHelper.GenerateEnumDictionary<GamePadStickEvent.ThumbStickType, Vector2>(Vector2.Zero);
      }

      public void HandleButtons(GamePadState state, PlayerIndex index)
      {
        foreach (KeyValuePair<GamePadButtonEvent.ButtonType, bool> keyValuePair in Enumerable.ToArray<KeyValuePair<GamePadButtonEvent.ButtonType, bool>>((IEnumerable<KeyValuePair<GamePadButtonEvent.ButtonType, bool>>) this._pressed))
        {
          ButtonState buttonState = this.GetButtonState(state, keyValuePair.Key);
          if (buttonState == ButtonState.Pressed && !keyValuePair.Value)
          {
            IEventQueue queue = this._queue;
            GamePadButtonDownEvent evt = new GamePadButtonDownEvent();
            evt.Type = keyValuePair.Key;
            evt.Index = index;
            queue.DispatchEvent<GamePadButtonDownEvent>(evt);
            this._pressed[keyValuePair.Key] = true;
          }
          else if (buttonState == ButtonState.Released && keyValuePair.Value)
          {
            IEventQueue queue = this._queue;
            GamePadButtonUpEvent evt = new GamePadButtonUpEvent();
            evt.Type = keyValuePair.Key;
            evt.Index = index;
            queue.DispatchEvent<GamePadButtonUpEvent>(evt);
            this._pressed[keyValuePair.Key] = false;
          }
        }
      }

      public void HandleStickEvents(GamePadState state, PlayerIndex index)
      {
        foreach (KeyValuePair<GamePadStickEvent.ThumbStickType, Vector2> keyValuePair in Enumerable.ToArray<KeyValuePair<GamePadStickEvent.ThumbStickType, Vector2>>((IEnumerable<KeyValuePair<GamePadStickEvent.ThumbStickType, Vector2>>) this._sticks))
        {
          Vector2 stickState = this.GetStickState(state, keyValuePair.Key);
          this._queue.DispatchEvent<GamePadStickEvent>(new GamePadStickEvent()
          {
            Value = stickState,
            PreviousValue = keyValuePair.Value,
            Type = keyValuePair.Key,
            Index = index
          });
          this._sticks[keyValuePair.Key] = stickState;
        }
      }

      public void HandleTriggerEvents(GamePadState state, PlayerIndex index)
      {
        foreach (KeyValuePair<GamePadTriggerEvent.TriggerType, float> keyValuePair in Enumerable.ToArray<KeyValuePair<GamePadTriggerEvent.TriggerType, float>>((IEnumerable<KeyValuePair<GamePadTriggerEvent.TriggerType, float>>) this._triggers))
        {
          float triggerState = this.GetTriggerState(state, keyValuePair.Key);
          if ((double) triggerState != (double) keyValuePair.Value)
          {
            this._queue.DispatchEvent<GamePadTriggerEvent>(new GamePadTriggerEvent()
            {
              Value = triggerState,
              PreviousValue = keyValuePair.Value,
              Type = keyValuePair.Key,
              Index = index
            });
            this._triggers[keyValuePair.Key] = triggerState;
          }
        }
      }

      private ButtonState GetButtonState(GamePadState state, GamePadButtonEvent.ButtonType type)
      {
        switch (type)
        {
          case GamePadButtonEvent.ButtonType.DPadDown:
            return state.DPad.Down;
          case GamePadButtonEvent.ButtonType.DPadLeft:
            return state.DPad.Left;
          case GamePadButtonEvent.ButtonType.DPadRight:
            return state.DPad.Right;
          case GamePadButtonEvent.ButtonType.DPadUp:
            return state.DPad.Up;
          case GamePadButtonEvent.ButtonType.RightShoulder:
            return state.Buttons.RightShoulder;
          case GamePadButtonEvent.ButtonType.LeftStick:
            return state.Buttons.LeftStick;
          case GamePadButtonEvent.ButtonType.LeftShoulder:
            return state.Buttons.LeftShoulder;
          case GamePadButtonEvent.ButtonType.Start:
            return state.Buttons.Start;
          case GamePadButtonEvent.ButtonType.X:
            return state.Buttons.X;
          case GamePadButtonEvent.ButtonType.Y:
            return state.Buttons.Y;
          case GamePadButtonEvent.ButtonType.RightStick:
            return state.Buttons.RightStick;
          case GamePadButtonEvent.ButtonType.Back:
            return state.Buttons.Back;
          case GamePadButtonEvent.ButtonType.A:
            return state.Buttons.A;
          case GamePadButtonEvent.ButtonType.B:
            return state.Buttons.B;
          case GamePadButtonEvent.ButtonType.BigButton:
            return state.Buttons.BigButton;
          default:
            throw new ArgumentException("The enum type was not defined in the switch", nameof (type));
        }
      }

      private float GetTriggerState(GamePadState state, GamePadTriggerEvent.TriggerType type)
      {
        if (type == GamePadTriggerEvent.TriggerType.Left)
          return state.Triggers.Left;
        if (type == GamePadTriggerEvent.TriggerType.Right)
          return state.Triggers.Right;
        throw new ArgumentException("The enum type was not defined in the switch", nameof (type));
      }

      private Vector2 GetStickState(GamePadState state, GamePadStickEvent.ThumbStickType type)
      {
        if (type == GamePadStickEvent.ThumbStickType.Left)
          return state.ThumbSticks.Left;
        if (type == GamePadStickEvent.ThumbStickType.Right)
          return state.ThumbSticks.Right;
        throw new ArgumentException("The enum type was not defined in the switch", nameof (type));
      }
    }
  }
}
