
// Type: NVorbis.Contracts.ITagData
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System.Collections.Generic;

#nullable disable
namespace NVorbis.Contracts
{
  public interface ITagData
  {
    IReadOnlyDictionary<string, IReadOnlyList<string>> All { get; }

    string EncoderVendor { get; }

    string Title { get; }

    string Version { get; }

    string Album { get; }

    string TrackNumber { get; }

    string Artist { get; }

    IReadOnlyList<string> Performers { get; }

    string Copyright { get; }

    string License { get; }

    string Organization { get; }

    string Description { get; }

    IReadOnlyList<string> Genres { get; }

    IReadOnlyList<string> Dates { get; }

    IReadOnlyList<string> Locations { get; }

    string Contact { get; }

    string Isrc { get; }

    string GetTagSingle(string key, bool concatenate = false);

    IReadOnlyList<string> GetTagMulti(string key);
  }
}
