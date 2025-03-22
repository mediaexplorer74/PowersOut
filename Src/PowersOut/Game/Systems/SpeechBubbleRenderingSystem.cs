
// Type: GameManager.Systems.SpeechBubbleRenderingSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Containers;
using BlueJay.Utils;
using Microsoft.Xna.Framework;
using GameManager.Addon;
using GameManager.Services;
using System;
using System.Collections.Generic;

#nullable enable
namespace GameManager.Systems
{
  public class SpeechBubbleRenderingSystem : IDrawSystem, ISystem
  {
    private readonly ProfileService _profileService;
    private readonly ISpriteBatchContainer _spriteBatch;
    private readonly IQuery<NinePatchAddon, TextAddon, PortraitAddon, SpriteSheetAddon, ExpressionAddon> _query;
    private readonly IFontCollection _font;
    private readonly IScreenViewport _screen;

    public SpeechBubbleRenderingSystem(
      ProfileService profileService,
      ISpriteBatchContainer spriteBatch,
      IQuery<NinePatchAddon, TextAddon, PortraitAddon, SpriteSheetAddon, ExpressionAddon> query,
      IFontCollection font,
      IScreenViewport screen)
    {
      this._profileService = profileService;
      this._spriteBatch = spriteBatch;
      this._query = query;
      this._font = font;
      this._screen = screen;
    }

    public void OnDraw()
    {
      DateTime now = DateTime.Now;
      this._spriteBatch.Begin();
      string str = "SpeechBubble";
      foreach (IEntity entity in (IEnumerable<IEntity>) this._query.WhereLayer(str))
      {
        NinePatchAddon addon1 = entity.GetAddon<NinePatchAddon>();
        TextAddon addon2 = entity.GetAddon<TextAddon>();
        PortraitAddon addon3 = entity.GetAddon<PortraitAddon>();
        SpriteSheetAddon addon4 = entity.GetAddon<SpriteSheetAddon>();
        ExpressionAddon addon5 = entity.GetAddon<ExpressionAddon>();
        float width1 = 600f;
        float height = 125f;
        float num1 = 10f;
        float num2 = 25f;
        float num3 = height - num1 * 2f;
        float num4 = 10f;
        Vector2 position = new Vector2((float) (((double) this._screen.Width - (double) width1) / 2.0), (float) this._screen.Height - height - num2);
        this._spriteBatch.DrawNinePatch(addon1.NinePatch, (int) width1, (int) height, position, Color.White);
        float width2 = width1 - (num1 * 2f + num3 + num4);
        this._spriteBatch.DrawString(this._font.TextureFonts["Default"], this._font.TextureFonts["Default"].FitString(addon2.Text, (int) width2, 1), new Vector2(position.X + num1 + num3 + num4, position.Y + num1), Color.White);
        Vector2 vector2_1 = new Vector2(position.X + num1, position.Y + num1);
        Vector2 vector2_2 = new Vector2(num3, num3);
        this._spriteBatch.DrawFrame(addon3.PortraitTexture, new Rectangle(vector2_1.ToPoint(), vector2_2.ToPoint()), addon4.Rows, addon4.Cols, (int) addon5.Expression, new Color?(Color.White));
      }
      this._spriteBatch.End();
      this._profileService.Profile[nameof (SpeechBubbleRenderingSystem)] 
                = now.Subtract(DateTime.Now).TotalMilliseconds;
    }
  }
}
