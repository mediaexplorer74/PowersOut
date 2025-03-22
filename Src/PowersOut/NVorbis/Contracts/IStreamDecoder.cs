
// Type: NVorbis.Contracts.IStreamDecoder
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;
using System.IO;

#nullable disable
namespace NVorbis.Contracts
{
  public interface IStreamDecoder : IDisposable
  {
    int Channels { get; }

    int SampleRate { get; }

    int UpperBitrate { get; }

    int NominalBitrate { get; }

    int LowerBitrate { get; }

    ITagData Tags { get; }

    TimeSpan TotalTime { get; }

    long TotalSamples { get; }

    TimeSpan TimePosition { get; set; }

    long SamplePosition { get; set; }

    bool ClipSamples { get; set; }

    bool HasClipped { get; }

    bool IsEndOfStream { get; }

    IStreamStats Stats { get; }

    void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = 0);

    void SeekTo(long samplePosition, SeekOrigin seekOrigin = 0);

    int Read(Span<float> buffer, int offset, int count);
  }
}
