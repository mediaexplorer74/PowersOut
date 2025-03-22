
// Type: BlueJay.Events.EventListener`1
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Assembly location: BlueJay.Events.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Events.Interfaces;

#nullable enable
namespace BlueJay.Events
{
  public abstract class EventListener<T> : IEventListener<T>, IEventListener
  {
    public object? ProcessTarget { get; protected set; }

    public void Process(IEvent evt)
    {
      if (!(evt is Event<T> evt1))
        return;
      this.Process((IEvent<T>) evt1);
    }

    public virtual bool ShouldProcess(IEvent evt)
    {
      return evt is Event<T> evt1 && this.ShouldProcess((IEvent<T>) evt1);
    }

    public abstract void Process(IEvent<T> evt);

    public virtual bool ShouldProcess(IEvent<T> evt)
    {
      return this.ProcessTarget == null || this.ProcessTarget == evt.Target;
    }
  }
}
