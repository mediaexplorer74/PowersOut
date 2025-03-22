
// Type: BlueJay.Views.View
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Assembly location: BlueJay.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.EventListeners;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using BlueJay.Events.Lifecycle;
using BlueJay.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

#nullable enable
namespace BlueJay.Views
{
  public abstract class View : IView
  {
    private IServiceScope? _scope;

    protected IServiceProvider? ServiceProvider => this._scope?.ServiceProvider;

    public void Initialize(IServiceProvider serviceProvider)
    {
      this._scope = this._scope == null ? serviceProvider.CreateScope() : throw new ArgumentException("Scope has already been created", "_scope");
      this.ConfigureProvider(this._scope.ServiceProvider);
      this._scope.ServiceProvider.AddEventListener<DrawEventListener, DrawEvent>();
    }

    protected abstract void ConfigureProvider(IServiceProvider serviceProvider);

    public virtual void Draw()
    {
      IServiceProvider serviceProvider = this.ServiceProvider;
      if (serviceProvider == null)
        return;
      serviceProvider.GetRequiredService<IEventProcessor>().Draw();
    }

    public virtual void Update()
    {
      IServiceProvider serviceProvider = this.ServiceProvider;
      if (serviceProvider == null)
        return;
      serviceProvider.GetRequiredService<IEventProcessor>().Update();
    }

    public void Activate()
    {
      IServiceProvider serviceProvider = this.ServiceProvider;
      if (serviceProvider == null)
        return;
      serviceProvider.GetRequiredService<IEventProcessor>().Activate();
    }

    public void Deactivate()
    {
      IServiceProvider serviceProvider = this.ServiceProvider;
      if (serviceProvider == null)
        return;
      serviceProvider.GetRequiredService<IEventProcessor>().Deactivate();
    }

    public void Exit()
    {
      IServiceProvider serviceProvider = this.ServiceProvider;
      if (serviceProvider == null)
        return;
      serviceProvider.GetRequiredService<IEventProcessor>().Exit();
    }

    public virtual void Enter()
    {
    }

    public virtual void Leave()
    {
    }
  }
}
