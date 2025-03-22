
// Type: GameManager.Factories.BackgroundFactory
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Container;
using Microsoft.Xna.Framework;
using System;

#nullable enable
namespace GameManager.Factories
{
  public static class BackgroundFactory
  {
    public static IEntity CreateBackground(
      this IServiceProvider serviceProvider,
      Vector2 position,
      ITexture2DContainer texture,
      string layer)
    {
      IEntity background = serviceProvider.AddEntity("Background_" + layer, -15);
      background.Add<PositionAddon>(new PositionAddon(position));
      background.Add<TextureAddon>(new TextureAddon(texture));
      return background;
    }
  }
}
