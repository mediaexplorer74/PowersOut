
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
using System.Diagnostics;

#nullable enable
namespace BlueJay.Utils
{
  internal class ContentManagerContainer : IContentManagerContainer
  {
    private readonly ContentManager _content;

    public ContentManagerContainer(ContentManager content) => this._content = content;

    public T Load<T>(string assetName)
    {
      T result = default;

        if (Type.Equals(typeof(T), typeof(ITexture2DContainer)))
        {
            try
            {
                // RnD: hotfix
                if (assetName == "")
                    assetName = "Anna_Boxes";

                result = (T)this._content.Load<Texture2D>(assetName).AsContainer();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] content.Load<Texture2D>(assetName) error: " + ex.Message);
            }
        }

        if (Type.Equals(typeof(T), typeof(ISpriteFontContainer)))
        {
            try
            {
                result = (T)this._content.Load<SpriteFont>(assetName).AsContainer();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] content.Load<SpriteFont>(assetName) error: " + ex.Message);
            }
        }
        else
        {
            try
            {
                result = this._content.Load<T>(assetName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] content.Load<T>(assetName) error: " + ex.Message);
            }
        }
        return result;
    }
  }
}
