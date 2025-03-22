
// Type: BlueJay.Core.Containers.Texture2DContainer
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core.Container;
using Microsoft.Xna.Framework.Graphics;

#nullable enable
namespace BlueJay.Core.Containers
{
  internal class Texture2DContainer : ITexture2DContainer
  {
    public Texture2D? Current { get; set; }

        public int Width
        {
            get
            {
                Texture2D current = this.Current;
                return current == null ? 0 : current.Width;
            }
        }

        public int Height
        {
            get
            {
                Texture2D current = this.Current;
                return current == null ? 0 : current.Height;
            }
        }

    public Texture2DContainer() => this.Current = (Texture2D) null;

    public void SetData<T>(T[] data) where T : struct => this.Current?.SetData<T>(data);

    public void Dispose() => this.Current?.Dispose();
  }
}
