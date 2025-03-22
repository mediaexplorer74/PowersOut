
// Type: BlueJay.Component.System.ServiceProviderExtensions
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Containers;
using Microsoft.Extensions.DependencyInjection;
using System;

#nullable enable
namespace BlueJay.Component.System
{
  public static class ServiceProviderExtensions
  {
    public static IEntity AddEntity(this IServiceProvider provider, string layer = "", int weight = 0)
    {
      return provider.AddEntity((IEntity) ActivatorUtilities.CreateInstance<Entity>(provider), layer, weight);
    }

    public static IEntity AddEntity(
      this IServiceProvider provider,
      IEntity entity,
      string layer = "",
      int weight = 0)
    {
      entity.Layer = layer;
      provider.GetRequiredService<ILayers>().Add(entity, layer, weight);
      return entity;
    }

    public static void RemoveEntity(this IServiceProvider provider, IEntity entity)
    {
      provider.GetRequiredService<ILayers>().Remove(entity);
      entity.Dispose();
    }

    public static void ClearEntities(this IServiceProvider provider, string? layer = null)
    {
      ILayers requiredService = provider.GetRequiredService<ILayers>();
      if (string.IsNullOrWhiteSpace(layer))
      {
        requiredService.Clear();
      }
      else
      {
        if (!requiredService.Contains(layer))
          return;
        requiredService[layer].Clear();
      }
    }

    public static void AddSpriteFont(
      this IServiceProvider provider,
      string key,
      ISpriteFontContainer font)
    {
      provider.GetRequiredService<IFontCollection>().SpriteFonts[key] = font;
    }

    public static void AddTextureFont(this IServiceProvider provider, string key, TextureFont font)
    {
      provider.GetRequiredService<IFontCollection>().TextureFonts[key] = font;
    }
  }
}
