
// Type: BlueJay.Core.Containers.SpriteFontContainer
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

#nullable enable
namespace BlueJay.Core.Containers
{
  internal class SpriteFontContainer : ISpriteFontContainer
  {
    public SpriteFont? Current { get; set; }

    public SpriteFontContainer() => this.Current = (SpriteFont) null;

    public Vector2 MeasureString(string text)
    {
      SpriteFont current = this.Current;
      return current == null ? Vector2.Zero : current.MeasureString(text);
    }

    public Vector2 MeasureString(StringBuilder text)
    {
      SpriteFont current = this.Current;
      return current == null ? Vector2.Zero : current.MeasureString(text);
    }
  }
}
