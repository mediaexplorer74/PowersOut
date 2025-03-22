
// Type: BlueJay.Events.ServiceCollectionExtensions
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Modded by [M]edia[E]xplorer

using BlueJay.Events.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#nullable enable
namespace BlueJay.Events
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddBlueJayEvents(this IServiceCollection collection)
    {
      return collection.AddScoped<IEventQueue, EventQueue>().AddScoped<IEventProcessor, EventProcessor>();
    }
  }
}
