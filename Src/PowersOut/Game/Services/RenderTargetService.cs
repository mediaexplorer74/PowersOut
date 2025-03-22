
// Type: GameManager.Services.RenderTargetService
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Utils;
using Microsoft.Xna.Framework.Graphics;

#nullable enable
namespace GameManager.Services
{
  public class RenderTargetService
  {
    public RenderTarget2D LightTarget { get; set; }

    public RenderTarget2D FlashLightTarget { get; set; }

    public RenderTarget2D ScreenTarget { get; set; }

    public RenderTargetService(GraphicsDevice graphicsDevice, IScreenViewport screen)
    {
      this.LightTarget = new RenderTarget2D(graphicsDevice, screen.Width, screen.Height);
      this.ScreenTarget = new RenderTarget2D(graphicsDevice, screen.Width, screen.Height);
      this.FlashLightTarget = new RenderTarget2D(graphicsDevice, screen.Width, screen.Height);
    }
  }
}
