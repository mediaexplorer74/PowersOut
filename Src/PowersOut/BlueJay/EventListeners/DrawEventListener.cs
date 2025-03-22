
// Type: BlueJay.EventListeners.DrawEventListener
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Assembly location: BlueJay.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Interfaces;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using BlueJay.Events.Lifecycle;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace BlueJay.EventListeners
{
  internal class DrawEventListener : EventListener<DrawEvent>
  {
    private readonly DrawableSystemCollection _systems;

    public DrawEventListener(DrawableSystemCollection systems) => this._systems = systems;

    public override void Process(IEvent<DrawEvent> evt)
    {
      //RnD
      Span<ISystem> span = CollectionsMarshal.AsSpan<ISystem>((List<ISystem>) this._systems);
      for (int index = 0; index < span.Length; ++index)
      {
        ISystem system = span[index];
        if (system is IDrawSystem)
          ((IDrawSystem) system).OnDraw();
      }
    }
  }
}
