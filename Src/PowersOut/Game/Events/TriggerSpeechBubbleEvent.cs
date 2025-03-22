
// Type: GameManager.Events.TriggerSpeechBubbleEvent
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

#nullable enable
namespace GameManager.Events
{
  public class TriggerSpeechBubbleEvent
  {
    public string Message { get; set; }

    public Expression Expression { get; set; }

    public TriggerSpeechBubbleEvent(string message, Expression expression)
    {
      this.Message = message;
      this.Expression = expression;
    }
  }
}
