
// Type: BlueJay.Events.CallbackListener`1
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Assembly location: BlueJay.Events.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Events.Interfaces;
using System;

#nullable enable
namespace BlueJay.Events
{
  public class CallbackListener<T> : EventListener<T>
  {
    private readonly Func<T, object?, bool> _callback;
    private readonly bool _shouldProcessTarget;

    public CallbackListener(
      Func<T, object?, bool> callback,
      object? processTarget,
      bool shouldProcessTarget)
    {
      this._callback = callback;
      this._shouldProcessTarget = shouldProcessTarget;
      this.ProcessTarget = processTarget;
    }

    public override bool ShouldProcess(IEvent evt)
    {
      return !this._shouldProcessTarget || this.ProcessTarget == evt.Target;
    }

    public override void Process(IEvent<T> evt)
    {
      if (this._callback(evt.Data, evt.Target))
        return;
      evt.StopPropagation();
    }
  }
}
