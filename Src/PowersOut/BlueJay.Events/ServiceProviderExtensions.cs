
// Type: BlueJay.Events.ServiceProviderExtensions
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Modded by [M]edia[E]xplorer

using BlueJay.Events.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

#nullable enable
namespace BlueJay.Events
{
  public static class ServiceProviderExtensions
  {
    public static IDisposable AddEventListener<T, K>(
      this IServiceProvider provider,
      params object[] parameters)
      where T : IEventListener<K>
    {
      return provider.GetRequiredService<IEventQueue>().AddEventListener<K>((IEventListener<K>) ActivatorUtilities.CreateInstance<T>(provider, parameters));
    }

    public static IDisposable AddEventListener<T>(
      this IServiceProvider provider,
      Func<T, bool> callback,
      int? weight = null)
    {
      return provider.GetRequiredService<IEventQueue>().AddEventListener<T>(callback, weight);
    }

    public static IDisposable AddEventListener<T>(
      this IServiceProvider provider,
      Func<T, object?, bool> callback,
      int? weight = null)
    {
      return provider.GetRequiredService<IEventQueue>().AddEventListener<T>(callback, weight);
    }

    public static IDisposable AddEventListener<T>(
      this IServiceProvider provider,
      Func<T, bool> callback,
      object? target,
      int? weight = null)
    {
      return provider.GetRequiredService<IEventQueue>().AddEventListener<T>(callback, target, weight);
    }

    public static IDisposable AddEventListener<T>(
      this IServiceProvider provider,
      Func<T, object?, bool> callback,
      object? target,
      int? weight = null)
    {
      return provider.GetRequiredService<IEventQueue>().AddEventListener<T>(callback, target, weight);
    }
  }
}
