
// Type: BlueJay.Common.Systems.DebugSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Containers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class DebugSystem : IDrawSystem, ISystem
  {
    private readonly ISpriteBatchContainer _batch;
    private readonly IFontCollection _fonts;
    private readonly string _fontKey;
    private readonly IQuery<DebugAddon> _debugQuery;

    public DebugSystem(
      ISpriteBatchContainer batch,
      IFontCollection fonts,
      IQuery<DebugAddon> debugQuery,
      string fontKey)
    {
      this._batch = batch;
      this._fontKey = fontKey;
      this._fonts = fonts;
      this._debugQuery = debugQuery;
    }

    public void OnDraw()
    {
      this._batch.Begin();
      int y = 10;
      foreach (IEntity entity in (IEnumerable<IEntity>) this._debugQuery)
      {
        foreach (object addon in entity.GetAddons(entity.GetAddon<DebugAddon>().KeyIdentifier))
        {
          this._batch.DrawString(this._fonts.SpriteFonts[this._fontKey], addon?.ToString() ?? string.Empty, new Vector2(10f, (float) y), Color.Black);
          y += 20;
        }
      }
      this._batch.End();
    }
  }
}
