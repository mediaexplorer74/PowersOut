
// Type: BlueJay.Common.Systems.RenderingSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Assembly location: BlueJay.Common.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Containers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class RenderingSystem : IDrawSystem, ISystem
  {
    private readonly ISpriteBatchContainer _batch;
    private readonly IQuery<PositionAddon, TextureAddon> _entities;

    public RenderingSystem(
      ISpriteBatchContainer batch,
      IQuery<PositionAddon, TextureAddon> entities)
    {
      this._batch = batch;
      this._entities = entities;
    }

    public void OnDraw()
    {
      this._batch.Begin();
      foreach (IEntity entity in (IEnumerable<IEntity>) this._entities)
      {
        PositionAddon addon1 = entity.GetAddon<PositionAddon>();
        TextureAddon addon2 = entity.GetAddon<TextureAddon>();
        if (addon2.Texture != null)
        {
          ColorAddon addon3;
          Color color = entity.TryGetAddon<ColorAddon>(out addon3) ? addon3.Color : Color.White;
          FrameAddon addon4;
          SpriteSheetAddon addon5;
          if (entity.TryGetAddon<FrameAddon>(out addon4) && entity.TryGetAddon<SpriteSheetAddon>(out addon5))
            this._batch.DrawFrame(addon2.Texture, addon1.Position, addon5.Rows, addon5.Cols, addon4.Frame, new Color?(color));
          else
            this._batch.Draw(addon2.Texture, addon1.Position, color);
        }
      }
      this._batch.End();
    }
  }
}
