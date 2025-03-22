
// Type: GameManager.Systems.TitleSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using BlueJay.Core.Interfaces;
using BlueJay.Interfaces;
using BlueJay.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameManager.Services;
using GameManager.Views;

#nullable enable
namespace GameManager.Systems
{
  public class TitleSystem : IUpdateSystem, ISystem, IDrawSystem
  {
    private readonly IScreenViewport _screen;
    private readonly IViewCollection _viewCollection;
    private readonly ISpriteBatchContainer _spriteBatch;
    private readonly SpriteBatchExtension _spriteBatchExtension;
    private readonly ICamera _camera;
    private readonly GameService _gameService;
    private readonly ITexture2DContainer _creditsTexture;
    private readonly ITexture2DContainer _lightSelector;
    private readonly ITexture2DContainer _powersOutTitle_Off;
    private readonly ITexture2DContainer _powersOutTitle_On;
    private readonly ITexture2DContainer _repeatingDither;
    private readonly ITexture2DContainer _titleOptions;
    private readonly Color _leftBackgroundColor;
    private readonly Color _rightBackgroundColor;
    private readonly Rectangle[] _titleOptionsRectangles = new Rectangle[3];
    private int? _selectedOption = new int?(0);
    private bool _showCredits;
    private bool _waitForLeftButtonUp;

    public TitleSystem(
      IContentManagerContainer content,
      IScreenViewport screen,
      ISpriteBatchContainer spriteBatch,
      SpriteBatchExtension spriteBatchExtension,
      ICamera camera,
      GameService gameService,
      IViewCollection viewCollection)
    {
      this._screen = screen;
      this._spriteBatch = spriteBatch;
      this._spriteBatchExtension = spriteBatchExtension;
      this._camera = camera;
      this._gameService = gameService;
      this._viewCollection = viewCollection;
      this._creditsTexture = content.Load<ITexture2DContainer>("items/Credits");
      this._lightSelector = content.Load<ITexture2DContainer>("items/LightSelector");
      this._powersOutTitle_Off = content.Load<ITexture2DContainer>("items/PowersOutTitle_Off");
      this._powersOutTitle_On = content.Load<ITexture2DContainer>("items/PowersOutTitle_On");
      this._repeatingDither = content.Load<ITexture2DContainer>("items/RepeatingDither");
      this._titleOptions = content.Load<ITexture2DContainer>("items/TitleOptions");
      this._leftBackgroundColor = new Color(24, 23, 30);
      this._rightBackgroundColor = new Color(36, 34, 46);
      Vector2 vector2 = new Vector2((float) ((double) (this._gameService.Resolution.Width - this._powersOutTitle_Off.Width) / 2.0 + 20.0), (float) (this._powersOutTitle_Off.Height / 2 + 10));
      for (int index = 0; index < 2; ++index)
      {
        this._titleOptionsRectangles[index] = new Rectangle((int) vector2.X, (int) vector2.Y, this._titleOptions.Width / 2, this._titleOptions.Height / 3);
        vector2.Y += (float) (this._titleOptions.Height / 3 + 10);
      }
      this._showCredits = false;
    }

    public void OnDraw()
    {
      this._spriteBatch.Begin();
      int num1 = this._screen.Width / 2;
      this._spriteBatchExtension.DrawRectangle(num1, this._screen.Height, Vector2.Zero, this._leftBackgroundColor);
      this._spriteBatchExtension.DrawRectangle(num1, this._screen.Height, new Vector2((float) num1, 0.0f), this._rightBackgroundColor);
      this._spriteBatch.End();
      this._spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: new Matrix?(this._camera.GetTransformMatrix));
      Vector2 position1 = new Vector2((float) (this._gameService.Resolution.Width - this._repeatingDither.Width) / 2f, 0.0f);
      Size resolution;
      while (true)
      {
        double y = (double) position1.Y;
        resolution = this._gameService.Resolution;
        double height = (double) resolution.Height;
        if (y < height)
        {
          this._spriteBatch.Draw(this._repeatingDither, position1, Color.White);
          position1.Y += (float) this._repeatingDither.Height;
        }
        else
          break;
      }
      if (!this._showCredits)
      {
        resolution = this._gameService.Resolution;
        double x = (double) (resolution.Width - this._powersOutTitle_Off.Width) / 2.0;
        Vector2 vector2 = new Vector2((float) x, 0.0f);
        ISpriteBatchContainer spriteBatch1 = this._spriteBatch;
        int? selectedOption = this._selectedOption;
        int num2 = 0;
        ITexture2DContainer container = selectedOption.GetValueOrDefault() == num2 
                    & selectedOption.HasValue ? this._powersOutTitle_On : this._powersOutTitle_Off;
        Vector2 position2 = vector2;
        Color white = Color.White;
        spriteBatch1.Draw(container, position2, white);
        if (this._selectedOption.HasValue)
          this._spriteBatch.Draw(this._lightSelector, new Vector2((float) (this._titleOptionsRectangles[this._selectedOption.Value].X - 25), (float) (this._titleOptionsRectangles[this._selectedOption.Value].Y - 5)), Color.White);
        for (int index = 0; index < 3; ++index)
        {
          ISpriteBatchContainer spriteBatch2 = this._spriteBatch;
          ITexture2DContainer titleOptions = this._titleOptions;
          Rectangle optionsRectangle = this._titleOptionsRectangles[index];
          int num3 = index * 2;
          selectedOption = this._selectedOption;
          int num4 = index;
          int num5 = selectedOption.GetValueOrDefault() == num4 & selectedOption.HasValue ? 1 : 0;
          int frame = num3 + num5;
          Color? color = new Color?(Color.White);
          Vector2? origin = new Vector2?();
          spriteBatch2.DrawFrame(titleOptions, optionsRectangle, 2, 2, frame, color, origin: origin);
        }
      }
      else
      {
        resolution = this._gameService.Resolution;
        double x = (double) (resolution.Width - this._creditsTexture.Width) / 2.0;
        Vector2 position3= new Vector2((float) x, 20f);
        this._spriteBatch.Draw(this._creditsTexture, position3, Color.White);
      }
      this._spriteBatch.End();
    }

    public void OnUpdate()
    {
      MouseState state = Mouse.GetState();
      Vector2 world = this._camera.ToWorld(state.Position.ToVector2());
      this._selectedOption = new int?();
      if (!this._waitForLeftButtonUp)
      {
        if (!this._showCredits)
        {
          for (int index = 0; index < 2; ++index)
          {
            if (this._titleOptionsRectangles[index].Contains(world))
            {
              this._selectedOption = new int?(index);
              int? selectedOption = this._selectedOption;
              int num = 0;
              if (selectedOption.GetValueOrDefault() == num & selectedOption.HasValue && state.LeftButton == ButtonState.Pressed)
              {
                this._viewCollection.SetCurrent<GameView>();
                break;
              }
              if (this._selectedOption.GetValueOrDefault() == 1 && state.LeftButton == ButtonState.Pressed)
              {
                this._showCredits = true;
                break;
              }
              break;
            }
          }
        }
        else if (this._showCredits && state.LeftButton == ButtonState.Pressed)
          this._showCredits = false;
      }
      if (!this._waitForLeftButtonUp && state.LeftButton == ButtonState.Pressed)
        this._waitForLeftButtonUp = true;
      if (!this._waitForLeftButtonUp || state.LeftButton != ButtonState.Released)
        return;
      this._waitForLeftButtonUp = false;
    }
  }
}
