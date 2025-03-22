
// Type: GameManager.Factories.EntityFactory
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using BlueJay.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using System;

#nullable enable
namespace GameManager.Factories
{
  public static class EntityFactory
  {
    public static IEntity CreateExitTrigger(
      this IServiceProvider serviceProvider,
      Rectangle bounds,
      string level,
      string toLevel,
      Vector2 start)
    {
      IEntity exitTrigger = serviceProvider.AddEntity("Triggers_" + level);
      exitTrigger.Add<BoundsAddon>(new BoundsAddon(bounds));
      exitTrigger.Add<ExitAddon>(new ExitAddon(toLevel, start));
      return exitTrigger;
    }

    public static IEntity CreateWall(
      this IServiceProvider serviceProvider,
      Vector2 position,
      Rectangle bounds,
      string level)
    {
      IEntity wall = serviceProvider.AddEntity("Walls_" + level);
      wall.Add<PositionAddon>(new PositionAddon(position));
      wall.Add<BoundsAddon>(new BoundsAddon(bounds));
      wall.Add<WallAddon>(new WallAddon());
      return wall;
    }

    public static IEntity CreateFakeFurniture(
      this IServiceProvider serviceProvider,
      Vector2 position,
      Rectangle sourceRectangle,
      string level,
      string assetName,
      string message,
      Expression expression)
    {
      IContentManagerContainer requiredService = serviceProvider.GetRequiredService<IContentManagerContainer>();
      IEntity fakeFurniture = serviceProvider.AddEntity("Walls_" + level);
      fakeFurniture.Add<TextureAddon>(new TextureAddon(requiredService.Load<ITexture2DContainer>(assetName)));
      fakeFurniture.Add<SourceRectangleAddon>(new SourceRectangleAddon(sourceRectangle));
      fakeFurniture.Add<PositionAddon>(new PositionAddon(position));
      fakeFurniture.Add<BoundsAddon>(new BoundsAddon(new Rectangle(0, 0, sourceRectangle.Width, sourceRectangle.Height)));
      fakeFurniture.Add<FakeFurnitureAddon>(new FakeFurnitureAddon());
      fakeFurniture.Add<WallAddon>(new WallAddon());
      if (!string.IsNullOrWhiteSpace(message))
        fakeFurniture.Add<TextAddon>(new TextAddon(message));
      fakeFurniture.Add<ExpressionAddon>(new ExpressionAddon(expression));
      return fakeFurniture;
    }

    public static IEntity CreateDoorSpawn(
      this IServiceProvider serviceProvider,
      Vector2 position,
      Rectangle sourceRectangle,
      Door door,
      string level,
      string assetName,
      string message,
      Expression expression)
    {
      IContentManagerContainer requiredService = serviceProvider.GetRequiredService<IContentManagerContainer>();
      IEntity doorSpawn = serviceProvider.AddEntity("Walls_" + level);
      doorSpawn.Add<PositionAddon>(new PositionAddon(position));
      doorSpawn.Add<TextureAddon>(new TextureAddon(requiredService.Load<ITexture2DContainer>(assetName)));
      doorSpawn.Add<BoundsAddon>(new BoundsAddon(new Rectangle(0, 0, sourceRectangle.Width, sourceRectangle.Height)));
      doorSpawn.Add<SourceRectangleAddon>(new SourceRectangleAddon(sourceRectangle));
      doorSpawn.Add<DoorAddon>(new DoorAddon(door));
      doorSpawn.Add<WallAddon>(new WallAddon());
      if (!string.IsNullOrWhiteSpace(message))
        doorSpawn.Add<TextAddon>(new TextAddon(message));
      doorSpawn.Add<ExpressionAddon>(new ExpressionAddon(expression));
      return doorSpawn;
    }

    public static IEntity CreatePickup(
      this IServiceProvider serviceProvider,
      Vector2 position,
      Rectangle sourceRectangle,
      Pickup pickup,
      string level,
      string? assetName,
      string message,
      Expression expression)
    {
      IGraphicsDeviceContainer requiredService1 = serviceProvider.GetRequiredService<IGraphicsDeviceContainer>();
      IContentManagerContainer requiredService2 = serviceProvider.GetRequiredService<IContentManagerContainer>();
      IEntity pickup1 = serviceProvider.AddEntity("Triggers_" + level);
      pickup1.Add<TextureAddon>(new TextureAddon(assetName == null ? requiredService1.CreateRectangle(sourceRectangle.Width, sourceRectangle.Height, new Color?(Color.Black)) : requiredService2.Load<ITexture2DContainer>(assetName)));
      pickup1.Add<PositionAddon>(new PositionAddon(position));
      pickup1.Add<BoundsAddon>(new BoundsAddon(new Rectangle(0, 0, sourceRectangle.Width, sourceRectangle.Height)));
      if (assetName != null)
        pickup1.Add<SourceRectangleAddon>(new SourceRectangleAddon(sourceRectangle));
      pickup1.Add<PickupAddon>(new PickupAddon(pickup));
      if (!string.IsNullOrWhiteSpace(message))
        pickup1.Add<TextAddon>(new TextAddon(message));
      pickup1.Add<ExpressionAddon>(new ExpressionAddon(expression));
      return pickup1;
    }

    public static IEntity CreateStartingBed(
      this IServiceProvider serviceProvider,
      Vector2 position,
      string level,
      string message,
      Expression expression)
    {
      IContentManagerContainer requiredService = serviceProvider.GetRequiredService<IContentManagerContainer>();
      IEntity startingBed = serviceProvider.AddEntity("Walls_" + level);
      startingBed.Add<PositionAddon>(new PositionAddon(position));
      startingBed.Add<TextureAddon>(new TextureAddon(requiredService.Load<ITexture2DContainer>("character/Sleeping_Anna")));
      startingBed.Add<SpriteSheetAddon>(new SpriteSheetAddon(3));
      startingBed.Add<FrameArrayAddon>(new FrameArrayAddon(new int[1], -1));
      startingBed.Add<BoundsAddon>(new BoundsAddon(new Rectangle(0, 0, 32, 64)));
      startingBed.Add<WallAddon>(new WallAddon());
      startingBed.Add<StartingBedAddon>(new StartingBedAddon(0, 2000));
      startingBed.Add<TextAddon>(new TextAddon(message));
      startingBed.Add<ExpressionAddon>(new ExpressionAddon(expression));
      return startingBed;
    }

    public static IEntity CreateEndTrigger(
      this IServiceProvider serviceProvider,
      Rectangle bounds,
      string level)
    {
      IEntity endTrigger = serviceProvider.AddEntity("Triggers_" + level);
      endTrigger.Add<EndTriggerAddon>(new EndTriggerAddon());
      endTrigger.Add<BoundsAddon>(new BoundsAddon(bounds));
      return endTrigger;
    }
  }
}
