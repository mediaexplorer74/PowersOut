
// Type: BlueJay.Core.Containers.ISpriteBatchContainer
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core.Container;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable enable
namespace BlueJay.Core.Containers
{
  public interface ISpriteBatchContainer
  {
    SpriteBatch Current { get; }

    void Draw(
      ITexture2DContainer container,
      Rectangle destinationRectangle,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vector2 origin,
      SpriteEffects effects,
      float layerDepth);

    void Draw(
      ITexture2DContainer container,
      Vector2 position,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vector2 origin,
      Vector2 scale,
      SpriteEffects effects,
      float layerDepth);

    void Draw(ITexture2DContainer container, Rectangle destinationRectangle, Color color);

    void Draw(
      ITexture2DContainer container,
      Vector2 position,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vector2 origin,
      float scale,
      SpriteEffects effects,
      float layerDepth);

    void Draw(ITexture2DContainer container, Vector2 position, Color color);

    void DrawString(ISpriteFontContainer container, string text, Vector2 position, Color color);

    void Begin(
      SpriteSortMode sortMode = SpriteSortMode.Deferred,
      BlendState? blendState = null,
      SamplerState? samplerState = null,
      DepthStencilState? depthStencilState = null,
      RasterizerState? rasterizerState = null,
      Effect? effect = null,
      Matrix? transformMatrix = null);

    void End();

    void DrawString(TextureFont font, string text, Vector2 position, Color color, float scale = 1f);

    void DrawFrame(
      ITexture2DContainer texture,
      Vector2 position,
      int rows,
      int columns,
      int frame,
      Color? color = null,
      float rotation = 0.0f,
      Vector2? origin = null,
      SpriteEffects effects = SpriteEffects.None,
      float layerDepth = 1f);

    void DrawFrame(
      ITexture2DContainer texture,
      Rectangle destinationRectangle,
      int rows,
      int columns,
      int frame,
      Color? color = null,
      float rotation = 0.0f,
      Vector2? origin = null,
      SpriteEffects effects = SpriteEffects.None,
      float layerDepth = 1f);

    void DrawNinePatch(NinePatch ninePatch, int width, int height, Vector2 position, Color color);

    void DrawLine(Vector2 pointA, Vector2 pointB, int weight, Color color);
  }
}
