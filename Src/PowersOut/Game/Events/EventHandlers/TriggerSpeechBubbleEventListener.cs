
// Type: GameManager.Events.EventHandlers.TriggerSpeechBubbleEventListener
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Container;
using BlueJay.Events;
using BlueJay.Events.Interfaces;
using BlueJay.Utils;
using GameManager.Addon;
using GameManager.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace GameManager.Events.EventHandlers
{
  public class TriggerSpeechBubbleEventListener : EventListener<TriggerSpeechBubbleEvent>
  {
    private readonly IContentManagerContainer _content;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventQueue _eventQueue;
    private readonly IQuery<SpeechBubbleAddon> _query;

    public TriggerSpeechBubbleEventListener(
      IContentManagerContainer content,
      IServiceProvider serviceProvider,
      IEventQueue eventQueue,
      IQuery<SpeechBubbleAddon> query)
    {
      this._content = content;
      this._serviceProvider = serviceProvider;
      this._eventQueue = eventQueue;
      this._query = query;
    }

    public override void Process(IEvent<TriggerSpeechBubbleEvent> evt)
    {
      if (Enumerable.Any<IEntity>((IEnumerable<IEntity>) this._query))
        return;
      this._serviceProvider.CreateSpeechBubble(this._content.Load<ITexture2DContainer>("dialogue/NinePatch"), this._content.Load<ITexture2DContainer>("character/Anna_Sprite_Express"), evt.Data.Message, evt.Data.Expression);
    }
  }
}
