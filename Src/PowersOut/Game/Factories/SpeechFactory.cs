
// Type: GameManager.Factories.SpeechFactory
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Common.Addons;
using BlueJay.Component.System;
using BlueJay.Component.System.Interfaces;
using BlueJay.Core;
using BlueJay.Core.Container;
using GameManager.Addon;
using System;

#nullable enable
namespace GameManager.Factories
{
  public static class SpeechFactory
  {
    public static IEntity CreateSpeechBubble(
      this IServiceProvider serviceProvider,
      ITexture2DContainer ninePatchtexture,
      ITexture2DContainer portraitTexture,
      string text,
      Expression expression)
    {
      IEntity speechBubble = serviceProvider.AddEntity("SpeechBubble");
      speechBubble.Add<NinePatchAddon>(new NinePatchAddon(new NinePatch(ninePatchtexture)));
      speechBubble.Add<SpriteSheetAddon>(new SpriteSheetAddon(7));
      speechBubble.Add<TextAddon>(new TextAddon(text));
      speechBubble.Add<PortraitAddon>(new PortraitAddon(portraitTexture));
      speechBubble.Add<ExpressionAddon>(new ExpressionAddon(expression));
      speechBubble.Add<SpeechBubbleAddon>(new SpeechBubbleAddon());
      speechBubble.Add<CountdownAddon>(new CountdownAddon(3500));
      return speechBubble;
    }
  }
}
