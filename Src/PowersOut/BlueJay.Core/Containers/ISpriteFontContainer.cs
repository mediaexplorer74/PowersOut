
// Type: BlueJay.Core.Containers.ISpriteFontContainer
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

#nullable enable
namespace BlueJay.Core.Containers
{
  public interface ISpriteFontContainer
  {
    SpriteFont? Current { get; set; }

    Vector2 MeasureString(string text);

    Vector2 MeasureString(StringBuilder text);
  }
}
