
// Type: GameManager.Systems.EndRenderingSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using BlueJay.Utils;
using Microsoft.Xna.Framework;
using GameManager.Services;

#nullable enable
namespace GameManager.Systems
{
  public class EndRenderingSystem : IDrawSystem, ISystem
  {
    private readonly GameService _gameService;
    private readonly ISpriteBatchContainer _spriteBatchContainer;
    private readonly ITexture2DContainer _endingTextureContainer;
    private readonly IScreenViewport _screenViewport;
    private readonly IFontCollection _font;

    public EndRenderingSystem(
      IContentManagerContainer contentManagerContainer,
      GameService gameService,
      ISpriteBatchContainer spriteBatchContainer,
      IScreenViewport screenViewport,
      IFontCollection font)
    {
      this._gameService = gameService;
      this._spriteBatchContainer = spriteBatchContainer;
      this._endingTextureContainer = contentManagerContainer.Load<ITexture2DContainer>("character/PowersOutEndScene");
      this._screenViewport = screenViewport;
      this._font = font;
    }

    public void OnDraw()
    {
      this._spriteBatchContainer.Begin();
      Point size = new Point(this._endingTextureContainer.Width / 2, this._endingTextureContainer.Height / 2);
      this._spriteBatchContainer.Draw(this._endingTextureContainer, new Rectangle(new Vector2((float) (this._screenViewport.Width - size.X) / 2f, (float) (this._screenViewport.Height - size.Y) / 2f).ToPoint(), size), Color.White);
      string str1 = "Thank you for playing!";
      int num = 3;
      string str2 = this._font.TextureFonts["Default"].FitString(str1, this._screenViewport.Width, num);
      Vector2 vector2 = this._font.TextureFonts["Default"].MeasureString(str2, num);
      Vector2 position = new Vector2((float) (((double) this._screenViewport.Width - (double) vector2.X) / 2.0), (float) ((double) this._screenViewport.Height - (double) vector2.Y - 100.0));
      this._spriteBatchContainer.DrawString(this._font.TextureFonts["Default"], str2, position, Color.White, (float) num);
      this._spriteBatchContainer.End();
    }
  }
}
