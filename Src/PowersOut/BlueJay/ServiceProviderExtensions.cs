
// Type: BlueJay.ServiceProviderExtensions
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.EventListeners;
using BlueJay.Events.Interfaces;
using BlueJay.Events.Lifecycle;
using BlueJay.Interfaces;
using BlueJay.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

#nullable enable
namespace BlueJay
{
  public static class ServiceProviderExtensions
  {
    public static void AddSystem<T>(this IServiceProvider provider, params object[] parameters) where T : ISystem
    {
      SystemTypeCollection requiredService1 = provider.GetRequiredService<SystemTypeCollection>();
      if (requiredService1.Contains(typeof (T)))
        return;
      IEventQueue requiredService2 = provider.GetRequiredService<IEventQueue>();
      T instance = ActivatorUtilities.CreateInstance<T>(provider, parameters);
      if ((object) instance is IUpdateSystem)
        requiredService2.AddEventListener<UpdateEvent>((IEventListener<UpdateEvent>) ActivatorUtilities.CreateInstance<UpdateEventListener>(provider, (object) instance));
      if ((object) instance is IDrawSystem)
        provider.GetRequiredService<DrawableSystemCollection>().Add((ISystem) instance);
      requiredService1.Add(typeof (T));
    }

    public static IServiceCollection AddBlueJay(this IServiceCollection collection)
    {
      return collection.AddSingleton<IViewCollection, ViewCollection>().AddScoped<DrawableSystemCollection>();
    }

    public static T SetStartView<T>(this IServiceProvider provider) where T : IView
    {
      return provider.GetRequiredService<IViewCollection>().SetCurrent<T>();
    }
  }
}
