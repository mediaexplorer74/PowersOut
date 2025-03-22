
// Type: GameManager.Views.GameView
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Events;
using BlueJay.Common.Events.GamePad;
using BlueJay.Common.Events.Keyboard;
using BlueJay.Common.Systems;
using BlueJay.Core.Container;
using BlueJay.Utils;
using BlueJay.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using GameManager.Events;
using GameManager.Events.EventHandlers;
using GameManager.Services;
using GameManager.Systems;
using System;
using BlueJay.Events;
using BlueJay;

#nullable enable
namespace GameManager.Views
{
  public class GameView : View
  {
    protected override void ConfigureProvider(IServiceProvider serviceProvider)
    {
      serviceProvider.AddSystem<ViewportCameraSystem>();
      serviceProvider.AddSystem<StartingSequenceSystem>();
      serviceProvider.AddSystem<GamepadSystem>();
      serviceProvider.AddSystem<ViewportSystem>();
      serviceProvider.AddSystem<KeyboardSystem>();
      serviceProvider.AddSystem<DirectionSystem>();
      serviceProvider.AddSystem<AnnaAnimationSystem>();
      serviceProvider.AddSystem<FrameArraySystem>();
      serviceProvider.AddSystem<WallCollisionSystem>();
      serviceProvider.AddSystem<VelocitySystem>();
      serviceProvider.AddSystem<ExitTriggerSystem>();
      serviceProvider.AddSystem<LightningUpdateSystem>();
      serviceProvider.AddSystem<DirectionRayUpdateSystem>();
      serviceProvider.AddSystem<DirectionRayCollisionFakeFurnitureSystem>();
      serviceProvider.AddSystem<PickupSystem>();
      serviceProvider.AddSystem<CountdownRemoveSystem>();
      serviceProvider.AddSystem<EndTriggerSystem>();
      serviceProvider.AddSystem<ClearSystem>((object) Color.Black);
      serviceProvider.AddSystem<ResolutionRenderingSystem>();
      serviceProvider.AddSystem<LightningDrawSystem>();
      serviceProvider.AddSystem<FlashLightRenderSystem>();
      serviceProvider.AddSystem<FullRenderSystem>();
      serviceProvider.AddSystem<SpeechBubbleRenderingSystem>();
      serviceProvider.AddEventListener<GamePadStickEventListener, GamePadStickEvent>();
      serviceProvider.AddEventListener<LevelTransitionEventListener, LevelTransitionEvent>();
      serviceProvider.AddEventListener<KeyboardPressEventListener, KeyboardPressEvent>();
      serviceProvider.AddEventListener<KeyboardUpEventListener, KeyboardUpEvent>();
      serviceProvider.AddEventListener<ViewportChangeEventListener, ViewportChangeEvent>();
      serviceProvider.AddEventListener<GamePadButtonDownEventListener, GamePadButtonDownEvent>();
      serviceProvider.AddEventListener<GamePadButtonUpEventListener, GamePadButtonUpEvent>();
      serviceProvider.AddEventListener<RemoveEntityEventListener, RemoveEntityEvent>();
      serviceProvider.AddEventListener<PickupEventListener, PickupEvent>();
      serviceProvider.AddEventListener<TriggerSpeechBubbleEventListener, TriggerSpeechBubbleEvent>();
      serviceProvider.AddEventListener<EndTriggerEventListener, EndEvent>();
      serviceProvider.LoadLDtk("Content/Levels.ldtk");
      serviceProvider.GetRequiredService<SoundService>().Start();
      IContentManagerContainer requiredService = serviceProvider.GetRequiredService<IContentManagerContainer>();
      requiredService.Load<ITexture2DContainer>("dialogue/NinePatch");
      requiredService.Load<ITexture2DContainer>("character/AnaTestPortrait");
    }
  }
}
