
// Type: BlueJay.ComponentSystemGame
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System;
using BlueJay.Core;
using BlueJay.Core.Interfaces;
using BlueJay.Events;
using BlueJay.Interfaces;
using BlueJay.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable enable
namespace BlueJay
{
  public abstract class ComponentSystemGame : Game
  {
    private IServiceCollection _serviceCollection;
    private IServiceProvider? _serviceProvider;
    private DeltaService _deltaService;

    protected GraphicsDeviceManager GraphicsManager { get; private set; }

    public ComponentSystemGame()
    {
      this.GraphicsManager = new GraphicsDeviceManager((Game) this);
      this._deltaService = new DeltaService();
      this._serviceCollection = new ServiceCollection().AddSingleton<IGraphicsDeviceService>(
          (IGraphicsDeviceService) this.GraphicsManager).AddSingleton<IDeltaService>(
          (IDeltaService) this._deltaService);
    }

    protected abstract void ConfigureServices(IServiceCollection serviceCollection);

    protected abstract void ConfigureProvider(IServiceProvider serviceProvider);

    protected override void LoadContent()
    {
      this._serviceCollection.AddSingleton<ComponentSystemGame>(this);
      this._serviceCollection.AddSingleton<ContentManager>(this.Content);
      this._serviceCollection.AddSingleton<GameWindow>(this.Window);
      this._serviceCollection.AddSingleton<GraphicsDevice>(this.GraphicsDevice);
      this._serviceCollection.AddSingleton<SpriteBatch>();
      this._serviceCollection.AddSingleton<SpriteBatchExtension>();
      this._serviceCollection.AddSingleton<IScreenViewport, ScreenViewport>();
      this._serviceCollection.AddSingleton<IContentManagerContainer, ContentManagerContainer>();
      this._serviceCollection.AddBlueJayEvents();
      this._serviceCollection.AddBlueJaySystem();
      this._serviceCollection.AddBlueJayCore();
      this._serviceCollection.AddBlueJay();
      this._serviceCollection.AddScoped<SystemTypeCollection>();
      this.ConfigureServices(this._serviceCollection);
      this._serviceProvider = (IServiceProvider) this._serviceCollection.BuildServiceProvider();
      this.ConfigureProvider(this._serviceProvider);
    }

    protected override void Update(GameTime gameTime)
    {
      this._deltaService.Delta = gameTime.ElapsedGameTime.Milliseconds;
      this._deltaService.DeltaSeconds = gameTime.ElapsedGameTime.TotalSeconds;
      IServiceProvider serviceProvider = this._serviceProvider;
      if (serviceProvider != null)
        serviceProvider.GetRequiredService<IViewCollection>().Current?.Update();
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      IServiceProvider serviceProvider = this._serviceProvider;
      if (serviceProvider != null)
        serviceProvider.GetRequiredService<IViewCollection>().Current?.Draw();
      base.Draw(gameTime);
    }

    protected override void OnActivated(object sender, EventArgs args)
    {
      IServiceProvider serviceProvider = this._serviceProvider;
      if (serviceProvider != null)
        serviceProvider.GetRequiredService<IViewCollection>().Current?.Activate();
      base.OnActivated(sender, args);
    }

    protected override void OnDeactivated(object sender, EventArgs args)
    {
      IServiceProvider serviceProvider = this._serviceProvider;
      if (serviceProvider != null)
        serviceProvider.GetRequiredService<IViewCollection>().Current?.Deactivate();
      base.OnDeactivated(sender, args);
    }

    protected override void OnExiting(object sender, /*ExitingEventArgs*/EventArgs args)
    {
      IServiceProvider serviceProvider = this._serviceProvider;
      if (serviceProvider != null)
        serviceProvider.GetRequiredService<IViewCollection>().Current?.Exit();
      base.OnExiting(sender, args);
    }
  }
}
