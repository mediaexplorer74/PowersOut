
// Type: BlueJay.Core.TextureFont
// Assembly: BlueJay.Core, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D47B7F7B-0D97-47BA-939B-B77D67177A29
// Assembly location: BlueJay.Core.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Core.Container;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable enable
namespace BlueJay.Core
{
  public class TextureFont
  {
    private readonly ITexture2DContainer _container;
    private readonly int _rows;
    private readonly int _cols;
    private readonly string _alphabet;

    public int Width => this._container.Width / this._cols;

    public int Height => this._container.Height / this._rows;

    public ITexture2DContainer Texture => this._container;

    public TextureFont(ITexture2DContainer container, int rows, int cols, string alphabet = "abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()/.,';\\][=-?><\":|}{+_`")
    {
      this._container = container;
      this._rows = rows;
      this._cols = cols;
      this._alphabet = alphabet;
    }

    public Rectangle GetBounds(char letter)
    {
      int num = this._alphabet.IndexOf(letter.ToString(), (StringComparison) 5);
      return num == -1 ? Rectangle.Empty : new Rectangle(num % this._cols * this.Width, num / this._cols * this.Height, this.Width, this.Height);
    }

    public Vector2 MeasureString(string str, int size = 1)
    {
      string[] strArray = str.Split('\n', (StringSplitOptions) 0);
      return new Vector2((float) (Enumerable.Max(Enumerable.Select<string, int>((IEnumerable<string>) strArray, (Func<string, int>) (x => x.Length))) * (this.Width * size)), (float) (Enumerable.Count<string>((IEnumerable<string>) strArray) * (this.Height * size)));
    }

    public string FitString(string str, int width, int size)
    {
      List<string> stringList = new List<string>();
      string str1 = string.Empty;
      foreach (Match match in new Regex("([^\\s]+)(\\s*)").Matches(str))
      {
        if ((double) this.MeasureString(str1 + ((Capture) match.Groups[1]).Value, size).X > (double) width)
        {
          if (!string.IsNullOrWhiteSpace(str1))
            stringList.Add(str1.Trim());
          str1 = ((Capture) match.Groups[0]).Value;
        }
        else
          str1 += ((Capture) match.Groups[0]).Value;
        if (!string.IsNullOrWhiteSpace(((Capture) match.Groups[2]).Value))
        {
          for (int index = 0; index < ((Capture) match.Groups[2]).Value.Length; ++index)
          {
            ReadOnlySpan<char> readOnlySpan1 = str1.AsSpan();
            char ch = ((Capture) match.Groups[2]).Value[index];
            string str2 = ch.ToString();
            ReadOnlySpan<char> readOnlySpan2 = str2.AsSpan();//new ReadOnlySpan<char>(ref ch);
            if ((double) this.MeasureString(readOnlySpan1.ToString() + readOnlySpan2.ToString(), size).X 
                            > (double) width)
            {
              if (!string.IsNullOrWhiteSpace(str1))
                stringList.Add(str1);
              ch = ((Capture) match.Groups[2]).Value[index];
              str1 = ch.ToString();
            }
            else
            {
              ReadOnlySpan<char> readOnlySpan3 = str1.AsSpan();
              ch = ((Capture) match.Groups[2]).Value[index];
              str2 = ch.ToString();
              //ReadOnlySpan<char> readOnlySpan4 = new ReadOnlySpan<char>(ref ch);
              str1 = readOnlySpan3.ToString() + str2;//readOnlySpan4;
            }
          }
        }
      }
      if (!string.IsNullOrWhiteSpace(str1))
        stringList.Add(str1);
      return string.Join("\n", (IEnumerable<string>) stringList);
    }
  }
}
