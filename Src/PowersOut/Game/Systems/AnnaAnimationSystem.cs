
// Type: GameManager.Systems.AnnaAnimationSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using GameManager.Addon;
using GameManager.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Systems
{
  public class AnnaAnimationSystem : IUpdateSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly IQuery<PlayerAddon, DirectionAddon, FrameArrayAddon, VelocityAddon> _playerQuery;
    private readonly Dictionary<Direction, int[]> _walkFrames;
    private readonly Dictionary<Direction, int[]> _idleFrames;

    public AnnaAnimationSystem(
      ProfileService profileService,
      IQuery<PlayerAddon, DirectionAddon, FrameArrayAddon, VelocityAddon> playerQuery)
    {
      this._profileService = profileService;
      this._playerQuery = playerQuery;
      Dictionary<Direction, int[]> dictionary1 = new Dictionary<Direction, int[]>();
      dictionary1.Add(Direction.North, new int[4]
      {
        0,
        8,
        16,
        24
      });
      dictionary1.Add(Direction.NorthWest, new int[4]
      {
        1,
        9,
        17,
        25
      });
      dictionary1.Add(Direction.West, new int[4]
      {
        2,
        10,
        18,
        26
      });
      dictionary1.Add(Direction.SouthWest, new int[4]
      {
        3,
        11,
        19,
        27
      });
      dictionary1.Add(Direction.South, new int[4]
      {
        4,
        12,
        20,
        28
      });
      dictionary1.Add(Direction.SouthEast, new int[4]
      {
        5,
        13,
        21,
        29
      });
      dictionary1.Add(Direction.East, new int[4]
      {
        6,
        14,
        22,
        30
      });
      dictionary1.Add(Direction.NorthEast, new int[4]
      {
        7,
        15,
        23,
        31
      });
      this._walkFrames = dictionary1;
      Dictionary<Direction, int[]> dictionary2 = new Dictionary<Direction, int[]>();
      dictionary2.Add(Direction.North, new int[1]);
      dictionary2.Add(Direction.NorthWest, new int[1]{ 1 });
      dictionary2.Add(Direction.West, new int[1]{ 2 });
      dictionary2.Add(Direction.SouthWest, new int[1]{ 3 });
      dictionary2.Add(Direction.South, new int[1]{ 4 });
      dictionary2.Add(Direction.SouthEast, new int[1]{ 5 });
      dictionary2.Add(Direction.East, new int[1]{ 6 });
      dictionary2.Add(Direction.NorthEast, new int[1]{ 7 });
      this._idleFrames = dictionary2;
    }

    public void OnUpdate()
    {
      DateTime now = DateTime.Now;
      IEntity entity = Enumerable.FirstOrDefault<IEntity>((IEnumerable<IEntity>) this._playerQuery);
      if (entity == null)
        return;
      DirectionAddon addon1 = entity.GetAddon<DirectionAddon>();
      VelocityAddon addon2 = entity.GetAddon<VelocityAddon>();
      FrameArrayAddon addon3 = entity.GetAddon<FrameArrayAddon>() with
      {
        Frames = !(addon2.Velocity == Vector2.Zero) ? this._walkFrames[addon1.Direction] : this._idleFrames[addon1.Direction]
      };
      entity.Update<FrameArrayAddon>(addon3);
      this._profileService.Profile[nameof (AnnaAnimationSystem)] = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
