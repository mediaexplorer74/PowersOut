
// Type: BlueJay.Core.Containers.SpriteBatchContainer
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core.Container;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable enable
namespace BlueJay.Core.Containers
{
  internal class SpriteBatchContainer : ISpriteBatchContainer
  {
    private readonly Texture2D _pixel;
    private readonly SpriteBatch _spriteBatch;

    public SpriteBatch Current => this._spriteBatch;

    public SpriteBatchContainer(SpriteBatch spriteBatch)
    {
      this._spriteBatch = spriteBatch;
      this._pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
    }

    public void Draw(
      ITexture2DContainer container,
      Rectangle destinationRectangle,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vector2 origin,
      SpriteEffects effects,
      float layerDepth)
    {
      if (container.Current == null)
        throw new ArgumentNullException("container.Current");
      this._spriteBatch.Draw(container.Current, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
    }

    public void Draw(
      ITexture2DContainer container,
      Vector2 position,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vector2 origin,
      Vector2 scale,
      SpriteEffects effects,
      float layerDepth)
    {
      if (container.Current == null)
        throw new ArgumentNullException("container.Current");
      this._spriteBatch.Draw(container.Current, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
    }

    public void Draw(ITexture2DContainer container, Rectangle destinationRectangle, Color color)
    {
      if (container.Current == null)
        throw new ArgumentNullException("container.Current");
      this._spriteBatch.Draw(container.Current, destinationRectangle, color);
    }

    public void Draw(
      ITexture2DContainer container,
      Vector2 position,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vector2 origin,
      float scale,
      SpriteEffects effects,
      float layerDepth)
    {
      if (container.Current == null)
        throw new ArgumentNullException("container.Current");
      this._spriteBatch.Draw(container.Current, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
    }

    public void Draw(ITexture2DContainer container, Vector2 position, Color color)
    {
      if (container.Current == null)
        throw new ArgumentNullException("container.Current");
      this._spriteBatch.Draw(container.Current, position, color);
    }

    public void DrawString(
      ISpriteFontContainer container,
      string text,
      Vector2 position,
      Color color)
    {
      if (container.Current == null)
        throw new ArgumentNullException("container.Current");
      this._spriteBatch.DrawString(container.Current, text, position, color);
    }

    public void Begin(
      SpriteSortMode sortMode = SpriteSortMode.Deferred,
      BlendState? blendState = null,
      SamplerState? samplerState = null,
      DepthStencilState? depthStencilState = null,
      RasterizerState? rasterizerState = null,
      Effect? effect = null,
      Matrix? transformMatrix = null)
    {
      this._spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
    }

    public void End() => this._spriteBatch.End();

    public void DrawString(
      TextureFont font,
      string text,
      Vector2 position,
      Color color,
      float scale = 1f)
    {
      Vector2 position1 = position;
      for (int index = 0; index < text.Length; ++index)
      {
        if (text[index] == '\n')
          position1 = new Vector2(position.X, position1.Y + (float) font.Height * scale);
        else if (text[index] == ' ')
        {
          position1 += new Vector2((float) font.Width * scale, 0.0f);
        }
        else
        {
          Rectangle bounds = font.GetBounds(text[index]);
          this.Draw(font.Texture, position1, new Rectangle?(bounds), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
          position1 += new Vector2((float) font.Width * scale, 0.0f);
        }
      }
    }

    public void DrawFrame(
      ITexture2DContainer container,
      Vector2 position,
      int rows,
      int columns,
      int frame,
      Color? color = null,
      float rotation = 0.0f,
      Vector2? origin = null,
      SpriteEffects effects = SpriteEffects.None,
      float layerDepth = 1f)
    {
      if (container == null)
        return;
      int num1 = container.Width / columns;
      int num2 = container.Height / rows;
      Rectangle rectangle = new Rectangle(frame % columns * num1, frame / columns * num2, num1, num2);
      this.Draw(container, new Rectangle(position.ToPoint(), new Point(num1, num2)), new Rectangle?(rectangle), color ?? Color.White, rotation, origin ?? Vector2.Zero, effects, layerDepth);
    }

    public void DrawFrame(
      ITexture2DContainer container,
      Rectangle destinationRectangle,
      int rows,
      int columns,
      int frame,
      Color? color = null,
      float rotation = 0.0f,
      Vector2? origin = null,
      SpriteEffects effects = SpriteEffects.None,
      float layerDepth = 1f)
    {
      int width = container.Width / columns;
      int height = container.Height / rows;
      Rectangle rectangle = new Rectangle(frame % columns * width, frame / columns * height, width, height);
      this.Draw(container, destinationRectangle, new Rectangle?(rectangle), color ?? Color.White, rotation, origin ?? Vector2.Zero, effects, layerDepth);
    }

    public void DrawNinePatch(
      NinePatch ninePatch,
      int width,
      int height,
      Vector2 position,
      Color color)
    {
      int num1 = height - ninePatch.Break.Y * 2;
      int num2 = width - ninePatch.Break.X * 2;
      int num3 = (int) Math.Ceiling((double) num1 / (double) ninePatch.Break.Y);
      int num4 = (int) Math.Ceiling((double) num2 / (double) ninePatch.Break.X);
      int num5 = num1 % ninePatch.Break.Y;
      int num6 = num2 % ninePatch.Break.X;
      for (int index1 = 0; index1 < num3; ++index1)
      {
        int y = ninePatch.Break.Y + index1 * ninePatch.Break.Y;
        for (int index2 = 0; index2 < num4; ++index2)
        {
          Rectangle destinationRectangle = new Rectangle(position.ToPoint() + new Point(ninePatch.Break.X + index2 * ninePatch.Break.X, y), ninePatch.Break);
          Rectangle middle = ninePatch.Middle;
          if (num6 > 0 && index2 == num4 - 1)
          {
            destinationRectangle.Width = num6;
            middle.Width = num6;
          }
          if (num5 > 0 && index1 == num3 - 1)
          {
            destinationRectangle.Height = num5;
            middle.Height = num5;
          }
          this.Draw(ninePatch.Texture, destinationRectangle, new Rectangle?(middle), color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }
        Rectangle destinationRectangle1 = new Rectangle(position.ToPoint() + new Point(0, y), ninePatch.Break);
        Rectangle middleLeft = ninePatch.MiddleLeft;
        if (num5 > 0 && index1 == num3 - 1)
        {
          destinationRectangle1.Height = num5;
          middleLeft.Height = num5;
        }
        this.Draw(ninePatch.Texture, destinationRectangle1, new Rectangle?(middleLeft), color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        Rectangle destinationRectangle2 = new Rectangle(position.ToPoint() + new Point(width - ninePatch.Break.X, y), ninePatch.Break);
        Rectangle middleRight = ninePatch.MiddleRight;
        if (num5 > 0 && index1 == num3 - 1)
        {
          destinationRectangle2.Height = num5;
          middleRight.Height = num5;
        }
        this.Draw(ninePatch.Texture, destinationRectangle2, new Rectangle?(middleRight), color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      }
      for (int index = 0; index < num4; ++index)
      {
        int x = ninePatch.Break.X + index * ninePatch.Break.X;
        Rectangle destinationRectangle3 = new Rectangle(position.ToPoint() + new Point(x, 0), ninePatch.Break);
        Rectangle top = ninePatch.Top;
        if (num6 > 0 && index == num4 - 1)
        {
          destinationRectangle3.Width = num6;
          top.Width = num6;
        }
        this.Draw(ninePatch.Texture, destinationRectangle3, new Rectangle?(top), color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        Rectangle destinationRectangle4 = new Rectangle(position.ToPoint() + new Point(x, height - ninePatch.Break.Y), ninePatch.Break);
        Rectangle bottom = ninePatch.Bottom;
        if (num6 > 0 && index == num4 - 1)
        {
          destinationRectangle4.Width = num6;
          bottom.Width = num6;
        }
        this.Draw(ninePatch.Texture, destinationRectangle4, new Rectangle?(bottom), color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      }
      this.Draw(ninePatch.Texture, new Rectangle(position.ToPoint(), ninePatch.Break), new Rectangle?(ninePatch.TopLeft), color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      this.Draw(ninePatch.Texture, new Rectangle(position.ToPoint() + new Point(width - ninePatch.Break.X, 0), ninePatch.Break), new Rectangle?(ninePatch.TopRight), color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      this.Draw(ninePatch.Texture, new Rectangle(position.ToPoint() + new Point(0, height - ninePatch.Break.Y), ninePatch.Break), new Rectangle?(ninePatch.BottomLeft), color, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
      this.Draw(ninePatch.Texture, new Rectangle(position.ToPoint() + new Point(width - ninePatch.Break.X, height - ninePatch.Break.Y), ninePatch.Break), new Rectangle?(ninePatch.BottomRight), color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
    }

    public void DrawLine(Vector2 pointA, Vector2 pointB, int weight, Color color)
    {
      Vector2 vector2 = pointA - pointB;
      float rotation = (float) Math.Atan2((double) vector2.Y, (double) vector2.X);
      float y = Vector2.Distance(pointA, pointB);
      Vector2 origin = new Vector2();
      Rectangle rectangle = new Rectangle(0, 0, 1, 1);
      Vector2 scale = new Vector2((float) weight, y);
      this._spriteBatch.Draw(this._pixel, pointA, new Rectangle?(rectangle), color, rotation, origin, scale, SpriteEffects.None, 0.0f);
    }
  }
}
