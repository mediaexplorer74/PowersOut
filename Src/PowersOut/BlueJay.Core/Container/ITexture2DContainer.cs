
// Type: BlueJay.Core.Container.ITexture2DContainer
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using Microsoft.Xna.Framework.Graphics;

#nullable enable
namespace BlueJay.Core.Container
{
  public interface ITexture2DContainer
  {
    Texture2D? Current { get; set; }

    int Width { get; }

    int Height { get; }

    void SetData<T>(T[] data) where T : struct;

    void Dispose();
  }
}
