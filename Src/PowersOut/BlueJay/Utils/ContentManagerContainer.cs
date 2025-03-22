
// Type: BlueJay.Utils.ContentManagerContainer
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Assembly location: BlueJay.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core;
using BlueJay.Core.Container;
using BlueJay.Core.Containers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable enable
namespace BlueJay.Utils
{
  internal class ContentManagerContainer : IContentManagerContainer
  {
    private readonly ContentManager _content;

    public ContentManagerContainer(ContentManager content) => this._content = content;

    public T Load<T>(string assetName)
    {
      if (Type.Equals(typeof (T), typeof (ITexture2DContainer)))
        return (T) this._content.Load<Texture2D>(assetName).AsContainer();
      return Type.Equals(typeof (T), typeof (ISpriteFontContainer)) ? (T) this._content.Load<SpriteFont>(assetName).AsContainer() : this._content.Load<T>(assetName);
    }
  }
}
