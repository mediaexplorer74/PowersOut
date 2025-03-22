
// Type: BlueJay.Common.Systems.KeyboardSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Common.Events.Keyboard;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class KeyboardSystem : IUpdateSystem, ISystem
  {
    private readonly IEventQueue _queue;
    private readonly Dictionary<Keys, bool> _pressed;

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

    public KeyboardSystem(IEventQueue queue)
    {
      this._queue = queue;
      this._pressed = EnumHelper.GenerateEnumDictionary<Keys, bool>(false);
    }

    public void OnUpdate()
    {
      KeyboardState state = Microsoft.Xna.Framework.Input.Keyboard.GetState();
      KeyValuePair<Keys, bool>[] array = Enumerable.ToArray<KeyValuePair<Keys, bool>>((IEnumerable<KeyValuePair<Keys, bool>>) this._pressed);
      bool flag1 = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
      bool flag2 = state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl);
      bool flag3 = state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt);
      for (int index = 0; index < array.Length; ++index)
      {
        KeyValuePair<Keys, bool> keyValuePair = array[index];
        KeyState keyState = state[keyValuePair.Key];
        if (keyState == KeyState.Down && !keyValuePair.Value)
        {
          IEventQueue queue1 = this._queue;
          KeyboardDownEvent evt1 = new KeyboardDownEvent();
          evt1.Key = keyValuePair.Key;
          evt1.CapsLock = state.CapsLock;
          evt1.NumLock = state.NumLock;
          evt1.Shift = flag1;
          evt1.Ctrl = flag2;
          evt1.Alt = flag3;
          queue1.DispatchEvent<KeyboardDownEvent>(evt1);
          IEventQueue queue2 = this._queue;
          KeyboardPressEvent evt2 = new KeyboardPressEvent();
          evt2.Key = keyValuePair.Key;
          evt2.CapsLock = state.CapsLock;
          evt2.NumLock = state.NumLock;
          evt2.Shift = flag1;
          evt2.Ctrl = flag2;
          evt2.Alt = flag3;
          queue2.DispatchEvent<KeyboardPressEvent>(evt2);
          this._pressed[keyValuePair.Key] = true;
        }
        else if (keyState == KeyState.Up && keyValuePair.Value)
        {
          IEventQueue queue = this._queue;
          KeyboardUpEvent evt = new KeyboardUpEvent();
          evt.Key = keyValuePair.Key;
          evt.CapsLock = state.CapsLock;
          evt.NumLock = state.NumLock;
          evt.Shift = flag1;
          evt.Ctrl = flag2;
          evt.Alt = flag3;
          queue.DispatchEvent<KeyboardUpEvent>(evt);
          this._pressed[keyValuePair.Key] = false;
        }
        else if (keyState == KeyState.Down && keyValuePair.Value)
        {
          IEventQueue queue = this._queue;
          KeyboardPressEvent evt = new KeyboardPressEvent();
          evt.Key = keyValuePair.Key;
          evt.CapsLock = state.CapsLock;
          evt.NumLock = state.NumLock;
          evt.Shift = flag1;
          evt.Ctrl = flag2;
          evt.Alt = flag3;
          queue.DispatchEvent<KeyboardPressEvent>(evt);
        }
      }
    }
  }
}
