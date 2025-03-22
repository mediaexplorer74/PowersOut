
// Type: GameManager.PowersOutGame
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay;
using BlueJay.Core;
using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using BlueJay.Core.Interfaces;
using BlueJay.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Services;
using GameManager.Views;
using System;
using BlueJay.Component.System;

#nullable enable
namespace GameManager
{
  public class Game1 : ComponentSystemGame
  {
    public Game1()
    {
      this.GraphicsManager.GraphicsProfile = GraphicsProfile.HiDef;
      this.GraphicsManager.PreferredBackBufferWidth = 800;
      this.GraphicsManager.PreferredBackBufferHeight = 480;
      this.Content.RootDirectory = "Content";
      this.Window.AllowUserResizing = true;
      this.IsMouseVisible = true;
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
      serviceCollection.AddSingleton<SoundService>();
      serviceCollection.AddScoped<ProfileService>();
      serviceCollection.AddScoped<GameService>();
      serviceCollection.AddScoped<RenderTargetService>();
      serviceCollection.AddScoped<ICamera, ViewportCamera>();
      serviceCollection.AddSingleton<Random>(new Random());
    }

    protected override void ConfigureProvider(IServiceProvider serviceProvider)
    {
      IContentManagerContainer requiredService = serviceProvider.GetRequiredService<IContentManagerContainer>();
      serviceProvider.AddSpriteFont("Default", requiredService.Load<ISpriteFontContainer>("TestFont"));
      ITexture2DContainer container = requiredService.Load<ITexture2DContainer>("Bitmap-Font");
      serviceProvider.AddTextureFont("Default", new TextureFont(container, 3, 24));
      serviceProvider.SetStartView<TitleView>();
    }
  }
}
