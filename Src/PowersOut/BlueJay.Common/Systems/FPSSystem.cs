
// Type: BlueJay.Common.Systems.FPSSystem
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Containers;
using BlueJay.Core.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable
namespace BlueJay.Common.Systems
{
  public class FPSSystem : IDrawSystem, ISystem, IUpdateSystem
  {
    private readonly ISpriteBatchContainer _batch;
    private readonly IDeltaService _deltaService;
    private readonly IFontCollection _fonts;
    private readonly string _fontKey;
    private int _fps;
    private int _updates;
    private int _countdown = 1000;

    public AddonKey Key => AddonKey.None;

    public List<string> Layers => new List<string>();

    public FPSSystem(
      ISpriteBatchContainer batch,
      IDeltaService deltaService,
      IFontCollection fonts,
      string fontKey)
    {
      this._batch = batch;
      this._deltaService = deltaService;
      this._fonts = fonts;
      this._fontKey = fontKey;
    }

    public void OnUpdate()
    {
      ++this._updates;
      this._countdown -= this._deltaService.Delta;
      if (this._countdown > 0)
        return;
      this._fps = this._updates;
      this._updates = 0;
      this._countdown += 1000;
    }

        public void OnDraw()
        {
            this._batch.Begin();
            ISpriteBatchContainer batch = this._batch;
            ISpriteFontContainer spriteFont = this._fonts.SpriteFonts[this._fontKey];
            string fpsText = $"fps: {this._fps}";
            Vector2 position = new Vector2(200f, 10f);
            Color black = Color.Black;
            batch.DrawString(spriteFont, fpsText, position, black);
            this._batch.End();
        }
  }
}
