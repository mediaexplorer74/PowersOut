
// Type: NVorbis.TagData
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace NVorbis
{
  internal class TagData : ITagData
  {
    private static IReadOnlyList<string> s_emptyList = (IReadOnlyList<string>) new List<string>();
    private Dictionary<string, IList<string>> _tags;

    public TagData(string vendor, string[] comments)
    {
      this.EncoderVendor = vendor;
      Dictionary<string, IList<string>> dictionary1 = new Dictionary<string, IList<string>>();
      for (int index = 0; index < comments.Length; ++index)
      {
        string[] strArray = comments[index].Split(new char[1]
        {
          '='
        });
        if (strArray.Length == 1)
          strArray = new string[2]
          {
            strArray[0],
            string.Empty
          };
        int num = strArray[0].IndexOf('[');
        if (num > -1)
        {
          strArray[1] = strArray[0].Substring(num + 1, strArray[0].Length - num - 2)
                        .ToUpper(CultureInfo.CurrentCulture) + ": " + strArray[1];
          strArray[0] = strArray[0].Substring(0, num);
        }
        IList<string> stringList1;
        if (dictionary1.TryGetValue(strArray[0].ToUpperInvariant(), out stringList1))
        {
          ((ICollection<string>) stringList1).Add(strArray[1]);
        }
        else
        {
          Dictionary<string, IList<string>> dictionary2 = dictionary1;
          string upperInvariant = strArray[0].ToUpperInvariant();
          List<string> stringList2 = new List<string>();
          stringList2.Add(strArray[1]);
          dictionary2.Add(upperInvariant, (IList<string>) stringList2);
        }
      }
      this._tags = dictionary1;
    }

    public string GetTagSingle(string key, bool concatenate = false)
    {
      IReadOnlyList<string> tagMulti = this.GetTagMulti(key);
      if (((IReadOnlyCollection<string>) tagMulti).Count <= 0)
        return string.Empty;
      return concatenate ? string.Join(Environment.NewLine, 
          Enumerable.ToArray<string>((IEnumerable<string>) tagMulti)) 
                : tagMulti[((IReadOnlyCollection<string>) tagMulti).Count - 1];
    }

    public IReadOnlyList<string> GetTagMulti(string key)
    {
      IList<string> stringList;
      return this._tags.TryGetValue(key.ToUpperInvariant(), out stringList)
                ? (IReadOnlyList<string>) stringList : TagData.s_emptyList;
    }

    public IReadOnlyDictionary<string, IReadOnlyList<string>> All
    {
      get => (IReadOnlyDictionary<string, IReadOnlyList<string>>) this._tags;
    }

    public string EncoderVendor { get; }

    public string Title => this.GetTagSingle("TITLE", false);

    public string Version => this.GetTagSingle("VERSION", false);

    public string Album => this.GetTagSingle("ALBUM", false);

    public string TrackNumber => this.GetTagSingle("TRACKNUMBER", false);

    public string Artist => this.GetTagSingle("ARTIST", false);

    public IReadOnlyList<string> Performers => this.GetTagMulti("PERFORMER");

    public string Copyright => this.GetTagSingle("COPYRIGHT", false);

    public string License => this.GetTagSingle("LICENSE", false);

    public string Organization => this.GetTagSingle("ORGANIZATION", false);

    public string Description => this.GetTagSingle("DESCRIPTION", false);

    public IReadOnlyList<string> Genres => this.GetTagMulti("GENRE");

    public IReadOnlyList<string> Dates => this.GetTagMulti("DATE");

    public IReadOnlyList<string> Locations => this.GetTagMulti("LOCATION");

    public string Contact => this.GetTagSingle("CONTACT", false);

    public string Isrc => this.GetTagSingle("ISRC", false);
  }
}
