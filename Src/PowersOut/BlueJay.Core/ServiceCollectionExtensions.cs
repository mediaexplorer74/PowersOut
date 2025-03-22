
// Type: BlueJay.Core.ServiceCollectionExtensions
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using BlueJay.Core.Containers;
using Microsoft.Extensions.DependencyInjection;

#nullable enable
namespace BlueJay.Core
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddBlueJayCore(this IServiceCollection collection)
    {
      return collection.AddSingleton<IGraphicsDeviceContainer, GraphicsDeviceContainer>().AddSingleton<ISpriteBatchContainer, SpriteBatchContainer>();
    }
  }
}
