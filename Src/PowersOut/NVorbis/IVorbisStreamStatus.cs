
// Type: NVorbis.IVorbisStreamStatus
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;

#nullable disable
namespace NVorbis
{
  [Obsolete("Moved to NVorbis.Contracts.IStreamStats", true)]
  public interface IVorbisStreamStatus : IStreamStats
  {
    [Obsolete("No longer supported.", true)]
    TimeSpan PageLatency { get; }

    [Obsolete("No longer supported.", true)]
    TimeSpan PacketLatency { get; }

    [Obsolete("No longer supported.", true)]
    TimeSpan SecondLatency { get; }

    [Obsolete("No longer supported.", true)]
    int PagesRead { get; }

    [Obsolete("No longer supported.", true)]
    int TotalPages { get; }

    [Obsolete("Use IStreamDecoder.HasClipped instead.  VorbisReader.HasClipped will return the same value for the stream it is handling.", true)]
    bool Clipped { get; }
  }
}
