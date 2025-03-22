
// Type: GameManager.Factories.PlayerFactory
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Container;
using BlueJay.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using System;

#nullable enable
namespace GameManager.Factories
{
  public static class PlayerFactory
  {
    public static IEntity CreatePlayer(this IServiceProvider serviceProvider, Vector2 position)
    {
      IContentManagerContainer requiredService = serviceProvider.GetRequiredService<IContentManagerContainer>();
      IEntity player = serviceProvider.AddEntity("Player", 15);
      Rectangle bounds = new Rectangle(4, 37, 16, 12);
      player.Add<PlayerAddon>(new PlayerAddon());
      player.Add<PositionAddon>(new PositionAddon(position - new Vector2((float) bounds.X, (float) bounds.Y)));
      player.Add<TextureAddon>(new TextureAddon(requiredService.Load<ITexture2DContainer>("character/Anna_Sprite_Turnaround")));
      player.Add<SpriteSheetAddon>(new SpriteSheetAddon(8, 4));
      player.Add<FrameArrayAddon>(new FrameArrayAddon(Array.Empty<int>(), 180));
      player.Add<VelocityAddon>(new VelocityAddon(Vector2.Zero));
      player.Add<BoundsAddon>(new BoundsAddon(bounds));
      player.Add<CollisionAddon>(new CollisionAddon());
      player.Add<DirectionAddon>(new DirectionAddon(Direction.South));
      player.Add<DirectionRayAddon>(new DirectionRayAddon(Math.PI / 16.0, 63f, new Vector2(12f, 22f)));
      player.Add<SkipRenderAddon>(new SkipRenderAddon());
      player.Add<DebugAddon>(new DebugAddon(KeyHelper.Create<DirectionAddon>()));
      return player;
    }
  }
}
