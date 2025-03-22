
// Type: GameManager.LDtkLoader
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Utils;
using LDtk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Data;
using GameManager.Factories;
using GameManager.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

#nullable enable
namespace GameManager
{
  public static class LDtkLoader
  {
    private static Dictionary<string, int> _entityCount = new Dictionary<string, int>();

    public static void LoadLDtk(this IServiceProvider serviceProvider, string path)
    {
      LDtkFile ldtkFile = LDtkFile.FromFile(path);
      JsonNode jsonNode1 = JsonNode.Parse(File.ReadAllText(path));
      List<LdtkEnum> ldtkEnumList;
      if (jsonNode1 == null)
      {
        ldtkEnumList = (List<LdtkEnum>) null;
      }
      else
      {
        JsonNode jsonNode2 = jsonNode1["defs"];
        if (jsonNode2 == null)
        {
          ldtkEnumList = (List<LdtkEnum>) null;
        }
        else
        {
          JsonNode node = jsonNode2["enums"];
          ldtkEnumList = node != null ? node.Deserialize<List<LdtkEnum>>() : (List<LdtkEnum>) null;
        }
      }
      if (ldtkEnumList == null)
        ldtkEnumList = new List<LdtkEnum>();
      List<LdtkEnum> enums = ldtkEnumList;
      List<LdtkTileset> ldtkTilesetList;
      if (jsonNode1 == null)
      {
        ldtkTilesetList = (List<LdtkTileset>) null;
      }
      else
      {
        JsonNode jsonNode3 = jsonNode1["defs"];
        if (jsonNode3 == null)
        {
          ldtkTilesetList = (List<LdtkTileset>) null;
        }
        else
        {
          JsonNode node = jsonNode3["tilesets"];
          ldtkTilesetList = node != null ? node.Deserialize<List<LdtkTileset>>() : (List<LdtkTileset>) null;
        }
      }
      if (ldtkTilesetList == null)
        ldtkTilesetList = new List<LdtkTileset>();
      List<LdtkTileset> tilesets = ldtkTilesetList;
      Guid iid = Guid.Parse("f35f8ff1-c210-11ef-8fba-83f3c8441249");
      LDtkWorld world = ldtkFile.LoadWorld(iid);
      GameService requiredService = serviceProvider.GetRequiredService<GameService>();
      if (world == null)
        return;
      foreach (LDtkLevel level in world.Levels)
      {
        LDtkLoader.WallGrid wallGrid = new LDtkLoader.WallGrid();
        requiredService.LevelSizes[level.Identifier] = new Size(level.PxWid, level.PxHei);
        if (level != null && level.LayerInstances != null)
        {
          foreach (LayerInstance layer in Enumerable.Reverse<LayerInstance>(
              (IEnumerable<LayerInstance>) level.LayerInstances))
          {
            switch (layer._Type)
            {
              case LayerType.Entities:
                serviceProvider.AddEntities(layer, level, world, enums, tilesets);
                continue;
              case LayerType.IntGrid:
                if (wallGrid.IntGrid == null)
                {
                  wallGrid.IntGrid = layer.IntGridCsv;
                  wallGrid.Width = layer._CWid;
                  wallGrid.Height = layer._CHei;
                  wallGrid.GridSize = layer._GridSize;
                  continue;
                }
                if (wallGrid.Width == layer._CWid && wallGrid.Height == layer._CHei)
                {
                  for (int index = 0; index < layer.IntGridCsv.Length; ++index)
                  {
                    if (layer.IntGridCsv[index] == 1 && wallGrid.IntGrid[index] == 0)
                      wallGrid.IntGrid[index] = 1;
                  }
                  continue;
                }
                continue;
              case LayerType.Tiles:
                serviceProvider.AddTiles(layer, level);
                continue;
              case LayerType.AutoLayer:
                serviceProvider.AddBackground(layer, level);
                continue;
              default:
                continue;
            }
          }
          if (wallGrid.IntGrid != null)
          {
            int num;
            while ((num = Array.IndexOf<int>(wallGrid.IntGrid, 1)) != -1)
            {
              Point point = new Point(num % wallGrid.Width, num / wallGrid.Width);
              Point bottomCorner = LDtkLoader.CalculateBottomCorner(point, 
                  wallGrid.Width, wallGrid.Height, wallGrid.IntGrid);
              if (!(point == bottomCorner))
              {
                Vector2 position = new Vector2((float) (point.X * wallGrid.GridSize), 
                    (float) (point.Y * wallGrid.GridSize));

                Rectangle bounds = new Rectangle(0, 0, (bottomCorner.X - point.X) 
                    * wallGrid.GridSize, (bottomCorner.Y - point.Y) * wallGrid.GridSize);

                serviceProvider.CreateWall(position, bounds, level.Identifier);

                LDtkLoader._entityCount["Wall"] = CollectionExtensions.GetValueOrDefault<string, int>(
                    (IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "Wall") + 1;
                LDtkLoader.ZeroGrid(point, bottomCorner, wallGrid.Width, wallGrid.IntGrid);
              }
              else
                break;
            }
          }
        }
      }
    }

    private static void ZeroGrid(Point from, Point to, int width, int[] grid)
    {
      for (int x = from.X; x < to.X; ++x)
      {
        for (int y = from.Y; y < to.Y; ++y)
          grid[x + y * width] = 0;
      }
    }

    private static Point CalculateBottomCorner(Point start, int width, int height, int[] grid)
    {
      int num1 = 0;
      int num2 = 0;
      while (true)
      {
        while (num1 != num2 || num1 + start.X + 1 > width
                    || num2 + start.Y + 1 > height 
                    || !LDtkLoader.IsAllWalls(start.X, start.Y, num1 + 1, num2 + 1, width, grid))
        {
          if ((num1 == num2 || num1 > num2) && num1 + start.X + 1 <= width 
                        && LDtkLoader.IsAllWalls(start.X, start.Y, num1 + 1, num2, width, grid))
          {
            ++num1;
          }
          else
          {
            if (num2 != num1 && num2 <= num1 || num2 + start.Y + 1 > height 
                            || !LDtkLoader.IsAllWalls(start.X, start.Y, num1, num2 + 1, width, grid))
              return new Point(num1, num2) + start;
            ++num2;
          }
        }
        ++num1;
        ++num2;
      }
    }

    private static bool IsAllWalls(int x, int y, int spanX, int spanY, int width, int[] grid)
    {
      for (int index1 = x; index1 < x + spanX; ++index1)
      {
        for (int index2 = y; index2 < y + spanY; ++index2)
        {
          if (grid[index1 + index2 * width] == 0)
            return false;
        }
      }
      return true;
    }

    private static void AddEntities(
      this IServiceProvider serviceProvider,
      LayerInstance layer,
      LDtkLevel level,
      LDtkWorld world,
      List<LdtkEnum> enums,
      List<LdtkTileset> tilesets)
    {
      if (layer.EntityInstances.Length == 0)
        return;
      GameService requiredService = serviceProvider.GetRequiredService<GameService>();
      foreach (EntityInstance entityInstance1 in layer.EntityInstances)
      {
        string identifier1 = entityInstance1._Identifier;
        if (identifier1 != null)
        {
          switch (identifier1.Length)
          {
            case 3:
              if (string.Equals(identifier1, "Box"))
                break;
              continue;
            case 4:
              if (string.Equals(identifier1, "Exit"))
              {
                FieldInstance fieldInstance = Enumerable.FirstOrDefault<FieldInstance>((IEnumerable<FieldInstance>) entityInstance1.FieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "Entity_ref")));
                if (fieldInstance != null)
                {
                  EntityReference reference = fieldInstance._Value.Deserialize<EntityReference>();
                  if (reference != null)
                  {
                    LDtkLevel ldtkLevel = world.LoadLevel(reference.LevelIid);
                    LayerInstance[] layerInstances = ldtkLevel.LayerInstances;
                    LayerInstance layerInstance = layerInstances != null ? Enumerable.FirstOrDefault<LayerInstance>((IEnumerable<LayerInstance>) layerInstances, (Func<LayerInstance, bool>) (x => Guid.Equals(x.Iid, reference.LayerIid))) : (LayerInstance) null;
                    EntityInstance entityInstance2;
                    if (layerInstance == null)
                    {
                      entityInstance2 = (EntityInstance) null;
                    }
                    else
                    {
                      EntityInstance[] entityInstances = layerInstance.EntityInstances;
                      entityInstance2 = entityInstances != null ? Enumerable.FirstOrDefault<EntityInstance>((IEnumerable<EntityInstance>) entityInstances, (Func<EntityInstance, bool>) (x => Guid.Equals(x.Iid, reference.EntityIid))) : (EntityInstance) null;
                    }
                    EntityInstance entityInstance3 = entityInstance2;
                    if (entityInstance3 != null)
                    {
                      Vector2 vector2 = new Vector2((float) entityInstance1.Px.X, (float) entityInstance1.Px.Y);
                      Rectangle bounds = new Rectangle((int) vector2.X, (int) vector2.Y, entityInstance1.Width, entityInstance1.Height);
                      serviceProvider.CreateExitTrigger(bounds, level.Identifier, ldtkLevel.Identifier, new Vector2((float) entityInstance3.Px.X, (float) entityInstance3.Px.Y));
                      LDtkLoader._entityCount["ExitTrigger"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "ExitTrigger") + 1;
                      continue;
                    }
                    continue;
                  }
                  continue;
                }
                continue;
              }
              continue;
            case 8:
              if (string.Equals(identifier1, "KeySpawn"))
                break;
              continue;
            case 9:
              if (string.Equals(identifier1, "DoorSpawn"))
                break;
              continue;
            case 10:
              if (string.Equals(identifier1, "EndTrigger"))
              {
                Rectangle bounds = new Rectangle(entityInstance1.Px.X, entityInstance1.Px.Y, entityInstance1.Width, entityInstance1.Height);
                serviceProvider.CreateEndTrigger(bounds, level.Identifier);
                LDtkLoader._entityCount["EndTrigger"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "EndTrigger") + 1;
                continue;
              }
              continue;
            case 11:
              switch (identifier1[0])
              {
                case 'P':
                  if (string.Equals(identifier1, "PlayerSpawn"))
                  {
                    requiredService.CurrentLevel = level.Identifier;
                    serviceProvider.CreatePlayer(new Vector2((float) entityInstance1.Px.X, (float) entityInstance1.Px.Y));
                    LDtkLoader._entityCount["Player"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "Player") + 1;
                    continue;
                  }
                  continue;
                case 'S':
                  if (string.Equals(identifier1, "StartingBed"))
                  {
                    FieldInstance fieldInstance1 = Enumerable.FirstOrDefault<FieldInstance>((IEnumerable<FieldInstance>) entityInstance1.FieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "AnaExpressions")));
                    Expression expression = Enum.Parse<Expression>((fieldInstance1 != null ? fieldInstance1._Value.Deserialize<string>() : (string) null) ?? "Regular");
                    FieldInstance fieldInstance2 = Enumerable.FirstOrDefault<FieldInstance>((IEnumerable<FieldInstance>) entityInstance1.FieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "message")));
                    string message = (fieldInstance2 != null ? fieldInstance2._Value.Deserialize<string>() : (string) null) ?? string.Empty;
                    Vector2 position = new Vector2((float) entityInstance1.Px.X, (float) entityInstance1.Px.Y);
                    serviceProvider.CreateStartingBed(position, level.Identifier, message, expression);
                    LDtkLoader._entityCount["StartingBed"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "StartingBed") + 1;
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            case 13:
              if (string.Equals(identifier1, "FakeFurniture"))
                break;
              continue;
            case 15:
              if (string.Equals(identifier1, "FlashLightSpawn"))
              {
                FieldInstance fieldInstance3 = Enumerable.FirstOrDefault<FieldInstance>((IEnumerable<FieldInstance>) entityInstance1.FieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "message")));
                string message = (fieldInstance3 != null ? fieldInstance3._Value.Deserialize<string>() : (string) null) ?? string.Empty;
                FieldInstance fieldInstance4 = Enumerable.FirstOrDefault<FieldInstance>((IEnumerable<FieldInstance>) entityInstance1.FieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "Tile")));
                TilesetRectangle tile = fieldInstance4 != null ? fieldInstance4._Value.Deserialize<TilesetRectangle>() : (TilesetRectangle) null;
                LdtkTileset ldtkTileset = Enumerable.FirstOrDefault<LdtkTileset>((IEnumerable<LdtkTileset>) tilesets, (Func<LdtkTileset, bool>) (x =>
                {
                  int uid = x.Uid;
                  int? tilesetUid = tile?.TilesetUid;
                  int valueOrDefault = tilesetUid.GetValueOrDefault();
                  return uid == valueOrDefault & tilesetUid.HasValue;
                }));
                if (tile != null && ldtkTileset != null)
                {
                  string assetName = Enumerable.First<string>((IEnumerable<string>) ldtkTileset.RelPath.Split('.', (StringSplitOptions) 0)) ?? string.Empty;
                  FieldInstance fieldInstance5 = Enumerable.FirstOrDefault<FieldInstance>((IEnumerable<FieldInstance>) entityInstance1.FieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "AnaExpressions")));
                  Expression expression = Enum.Parse<Expression>((fieldInstance5 != null ? fieldInstance5._Value.Deserialize<string>() : (string) null) ?? "Regular");
                  Vector2 position = new Vector2((float) entityInstance1.Px.X, (float) entityInstance1.Px.Y);
                  Rectangle sourceRectangle = new Rectangle(tile.X, tile.Y, tile.W, tile.H);
                  serviceProvider.CreatePickup(position, sourceRectangle, Pickup.FlashLight, level.Identifier, assetName, message, expression);
                  LDtkLoader._entityCount["FlashLightSpawn"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "FlashLightSpawn") + 1;
                  continue;
                }
                continue;
              }
              continue;
            default:
              continue;
          }
          IEnumerable<FieldInstance> fieldInstances = Enumerable.Where<FieldInstance>((IEnumerable<FieldInstance>) entityInstance1.FieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(Enumerable.First<string>((IEnumerable<string>) x._Type.Split('.', (StringSplitOptions) 0)), "LocalEnum")));
          FieldInstance fieldInstance6 = Enumerable.FirstOrDefault<FieldInstance>((IEnumerable<FieldInstance>) entityInstance1.FieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "message")));
          string message1 = (fieldInstance6 != null ? fieldInstance6._Value.Deserialize<string>() : (string) null) ?? string.Empty;
          if (Enumerable.Any<FieldInstance>(fieldInstances))
          {
            Dictionary<string, string> dictionary1 = Enumerable.ToDictionary<FieldInstance, string, string>(fieldInstances, (Func<FieldInstance, string>) (x => Enumerable.Last<string>((IEnumerable<string>) x._Type.Split('.', (StringSplitOptions) 0))), (Func<FieldInstance, string>) (x => x._Value.Deserialize<string>()));
            if (Enumerable.Any<KeyValuePair<string, string>>((IEnumerable<KeyValuePair<string, string>>) dictionary1))
            {
              Dictionary<string, LdtkEnumItem> dictionary2 = Enumerable.ToDictionary<KeyValuePair<string, string>, string, LdtkEnumItem>((IEnumerable<KeyValuePair<string, string>>) dictionary1, (Func<KeyValuePair<string, string>, string>) (x => x.Key), (Func<KeyValuePair<string, string>, LdtkEnumItem>) (x =>
              {
                LdtkEnum ldtkEnum = Enumerable.FirstOrDefault<LdtkEnum>((IEnumerable<LdtkEnum>) enums, (Func<LdtkEnum, bool>) (y => string.Equals(y.Identifier, x.Key)));
                return ldtkEnum == null ? (LdtkEnumItem) null : Enumerable.FirstOrDefault<LdtkEnumItem>(ldtkEnum.Values, (Func<LdtkEnumItem, bool>) (y => string.Equals(y.Id, x.Value)));
              }));
              Dictionary<string, LdtkTileset> dictionary3 = Enumerable.ToDictionary<KeyValuePair<string, LdtkEnumItem>, string, LdtkTileset>((IEnumerable<KeyValuePair<string, LdtkEnumItem>>) dictionary2, (Func<KeyValuePair<string, LdtkEnumItem>, string>) (x => x.Key), (Func<KeyValuePair<string, LdtkEnumItem>, LdtkTileset>) (x => Enumerable.FirstOrDefault<LdtkTileset>((IEnumerable<LdtkTileset>) tilesets, (Func<LdtkTileset, bool>) (y =>
              {
                int uid = y.Uid;
                int? tilesetUid = x.Value?.TileRect?.TilesetUid;
                int valueOrDefault = tilesetUid.GetValueOrDefault();
                return uid == valueOrDefault & tilesetUid.HasValue;
              }))));
              if (Enumerable.Any<KeyValuePair<string, LdtkEnumItem>>((IEnumerable<KeyValuePair<string, LdtkEnumItem>>) dictionary2) && Enumerable.Any<KeyValuePair<string, LdtkTileset>>((IEnumerable<KeyValuePair<string, LdtkTileset>>) dictionary3))
              {
                Vector2 position = new Vector2((float) entityInstance1.Px.X, (float) entityInstance1.Px.Y);
                KeyValuePair<string, LdtkEnumItem> keyValuePair = Enumerable.FirstOrDefault<KeyValuePair<string, LdtkEnumItem>>((IEnumerable<KeyValuePair<string, LdtkEnumItem>>) dictionary2,
                (Func<KeyValuePair<string, LdtkEnumItem>, bool>)(x => x.Key != "AnaExpressions"));
                if (keyValuePair.Value != null)
                {
                  LdtkEnumItemRect tileRect = keyValuePair.Value.TileRect;
                  if (tileRect != null)
                  {
                    Rectangle sourceRectangle = new Rectangle(tileRect.X, tileRect.Y, tileRect.Width, tileRect.Height);
                    LdtkTileset ldtkTileset = dictionary3[keyValuePair.Key];
                    string assetName = (ldtkTileset != null ? Enumerable.First<string>((IEnumerable<string>) ldtkTileset.RelPath.Split('.', (StringSplitOptions) 0)) : (string) null) ?? string.Empty;
                    FieldInstance fieldInstance7 = Enumerable.FirstOrDefault<FieldInstance>(fieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "AnaExpressions")));
                    Expression expression = Enum.Parse<Expression>((fieldInstance7 != null ? fieldInstance7._Value.Deserialize<string>() : (string) null) ?? "Regular");
                    string identifier2 = entityInstance1._Identifier;
                    if (!string.Equals(identifier2, "DoorSpawn"))
                    {
                      if (string.Equals(identifier2, "KeySpawn"))
                      {
                        FieldInstance fieldInstance8 = Enumerable.FirstOrDefault<FieldInstance>(fieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "Keys")));
                        Pickup pickup = Enum.Parse<Pickup>(((fieldInstance8 != null ? fieldInstance8._Value.Deserialize<string>() : (string) null) ?? "BrotherRoom") + "Key");
                        serviceProvider.CreatePickup(position, sourceRectangle, pickup, level.Identifier, assetName, message1, expression);
                        LDtkLoader._entityCount["KeySpawn"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "KeySpawn") + 1;
                      }
                      else
                      {
                        serviceProvider.CreateFakeFurniture(position, sourceRectangle, level.Identifier, assetName, message1, expression);
                        LDtkLoader._entityCount["FakeFurniture"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "FakeFurniture") + 1;
                      }
                    }
                    else
                    {
                      FieldInstance fieldInstance9 = Enumerable.FirstOrDefault<FieldInstance>(fieldInstances, (Func<FieldInstance, bool>) (x => string.Equals(x._Identifier, "Doors")));
                      Door door = Enum.Parse<Door>((fieldInstance9 != null ? fieldInstance9._Value.Deserialize<string>() : (string) null) ?? "BrotherRoomDoor");
                      serviceProvider.CreateDoorSpawn(position, sourceRectangle, door, level.Identifier, assetName, message1, expression);
                      LDtkLoader._entityCount["DoorSpawn"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "DoorSpawn") + 1;
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    private static IEntity? AddTiles(
      this IServiceProvider serviceProvider,
      LayerInstance layer,
      LDtkLevel level)
    {
      if (layer.GridTiles.Length == 0)
        return (IEntity) null;
      IContentManagerContainer requiredService1 = serviceProvider.GetRequiredService<IContentManagerContainer>();
      GraphicsDevice requiredService2 = serviceProvider.GetRequiredService<GraphicsDevice>();
      SpriteBatch requiredService3 = serviceProvider.GetRequiredService<SpriteBatch>();
      int width = layer._CWid * layer._GridSize;
      int height = layer._CHei * layer._GridSize;
      string tilesetRelPath = layer._TilesetRelPath;
      string assetName = (tilesetRelPath != null ? Enumerable.First<string>((IEnumerable<string>) tilesetRelPath.Split('.', (StringSplitOptions) 0)) : (string) null) ?? "InteriorTilesLITE";
      Texture2D texture = requiredService1.Load<Texture2D>(assetName);
      RenderTarget2D renderTarget2D = new RenderTarget2D(requiredService2, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
      requiredService2.SetRenderTarget(renderTarget2D);
      requiredService3.Begin(samplerState: SamplerState.PointClamp);
      foreach (TileInstance gridTile in layer.GridTiles)
      {
        Vector2 position = new Vector2((float) gridTile.Px.X, (float) gridTile.Px.Y);
        Rectangle rectangle = new Rectangle(gridTile.Src.X, gridTile.Src.Y, layer._GridSize, layer._GridSize);
        SpriteEffects f = (SpriteEffects) gridTile.F;
        requiredService3.Draw(texture, position, new Rectangle?(rectangle), Color.White * layer._Opacity, 0.0f, Vector2.Zero, 1f, f, 0.0f);
      }
      requiredService3.End();
      requiredService2.SetRenderTarget((RenderTarget2D) null);
      LDtkLoader._entityCount["Background"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "Background") + 1;
      return serviceProvider.CreateBackground(new Vector2((float) layer._PxTotalOffsetX, (float) layer._PxTotalOffsetY), renderTarget2D.AsContainer(), level.Identifier);
    }

    private static IEntity? AddBackground(
      this IServiceProvider serviceProvider,
      LayerInstance layer,
      LDtkLevel level)
    {
      if (layer.AutoLayerTiles.Length == 0)
        return (IEntity) null;
      IContentManagerContainer requiredService1 = serviceProvider.GetRequiredService<IContentManagerContainer>();
      GraphicsDevice requiredService2 = serviceProvider.GetRequiredService<GraphicsDevice>();
      SpriteBatch requiredService3 = serviceProvider.GetRequiredService<SpriteBatch>();
      int width = layer._CWid * layer._GridSize;
      int height = layer._CHei * layer._GridSize;
      string tilesetRelPath = layer._TilesetRelPath;
      string assetName = (tilesetRelPath != null ? Enumerable.First<string>((IEnumerable<string>) tilesetRelPath.Split('.', (StringSplitOptions) 0)) : (string) null) ?? "tiles/InteriorTilesLITE";
      Texture2D texture = requiredService1.Load<Texture2D>(assetName);
      RenderTarget2D renderTarget2D = new RenderTarget2D(requiredService2, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
      requiredService2.SetRenderTarget(renderTarget2D);
      requiredService3.Begin(samplerState: SamplerState.PointClamp);
      foreach (TileInstance autoLayerTile in layer.AutoLayerTiles)
      {
        Vector2 position = new Vector2((float) autoLayerTile.Px.X, (float) autoLayerTile.Px.Y);
        Rectangle rectangle = new Rectangle(autoLayerTile.Src.X, autoLayerTile.Src.Y, layer._GridSize, layer._GridSize);
        SpriteEffects f = (SpriteEffects) autoLayerTile.F;
        requiredService3.Draw(texture, position, new Rectangle?(rectangle), Color.White * layer._Opacity, 0.0f, Vector2.Zero, 1f, f, 0.0f);
      }
      requiredService3.End();
      requiredService2.SetRenderTarget((RenderTarget2D) null);
      LDtkLoader._entityCount["Background"] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) LDtkLoader._entityCount, "Background") + 1;
      return serviceProvider.CreateBackground(new Vector2((float) layer._PxTotalOffsetX, (float) layer._PxTotalOffsetY), renderTarget2D.AsContainer(), level.Identifier);
    }

    private static IEnumerable<Rectangle> GetAllWalls(LayerInstance layer)
    {
      if (layer.IntGridCsv.Length != 0)
      {
        for (int y = 0; y < layer._CHei; ++y)
        {
          for (int x = 0; x < layer._CWid; ++x)
          {
            if (layer.IntGridCsv[x + y * layer._CWid] == 1)
              yield return new Rectangle(x * layer._GridSize, y * layer._GridSize, layer._GridSize, layer._GridSize);
          }
        }
      }
    }

    private class WallGrid
    {
      public int[]? IntGrid { get; set; }

      public int Width { get; set; }

      public int Height { get; set; }

      public int GridSize { get; set; }
    }
  }
}
