
// Type: GameManager.Events.EventHandlers.ViewportChangeEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Events;
using BlueJay.Core;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Services;

#nullable enable
namespace GameManager.Events.EventHandlers
{
  public class ViewportChangeEventListener : EventListener<ViewportChangeEvent>
  {
    private readonly GraphicsDevice _graphicsDevice;
    private readonly RenderTargetService _renderTargetService;

    public ViewportChangeEventListener(
      GraphicsDevice graphicsDevice,
      RenderTargetService renderTargetService)
    {
      this._graphicsDevice = graphicsDevice;
      this._renderTargetService = renderTargetService;
    }

    public override void Process(IEvent<ViewportChangeEvent> evt)
    {
      this._renderTargetService.LightTarget.Dispose();
      RenderTargetService renderTargetService1 = this._renderTargetService;
      GraphicsDevice graphicsDevice1 = this._graphicsDevice;
      Size current1 = evt.Data.Current;
      int width1 = current1.Width;
      current1 = evt.Data.Current;
      int height1 = current1.Height;
      RenderTarget2D renderTarget2D1 = new RenderTarget2D(graphicsDevice1, width1, height1);
      renderTargetService1.LightTarget = renderTarget2D1;
      this._renderTargetService.ScreenTarget.Dispose();
      RenderTargetService renderTargetService2 = this._renderTargetService;
      GraphicsDevice graphicsDevice2 = this._graphicsDevice;
      Size current2 = evt.Data.Current;
      int width2 = current2.Width;
      current2 = evt.Data.Current;
      int height2 = current2.Height;
      RenderTarget2D renderTarget2D2 = new RenderTarget2D(graphicsDevice2, width2, height2);
      renderTargetService2.ScreenTarget = renderTarget2D2;
      this._renderTargetService.FlashLightTarget.Dispose();
      RenderTargetService renderTargetService3 = this._renderTargetService;
      GraphicsDevice graphicsDevice3 = this._graphicsDevice;
      Size current3 = evt.Data.Current;
      int width3 = current3.Width;
      current3 = evt.Data.Current;
      int height3 = current3.Height;
      RenderTarget2D renderTarget2D3 = new RenderTarget2D(graphicsDevice3, width3, height3);
      renderTargetService3.FlashLightTarget = renderTarget2D3;
    }
  }
}
