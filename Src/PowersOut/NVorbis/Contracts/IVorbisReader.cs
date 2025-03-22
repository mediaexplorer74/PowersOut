
// Type: NVorbis.Contracts.IVorbisReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NVorbis.Contracts
{
  public interface IVorbisReader : IDisposable
  {
    event EventHandler<NewStreamEventArgs> NewStream;

    long ContainerOverheadBits { get; }

    long ContainerWasteBits { get; }

    IReadOnlyList<IStreamDecoder> Streams { get; }

    int StreamIndex { get; }

    int Channels { get; }

    int SampleRate { get; }

    int UpperBitrate { get; }

    int NominalBitrate { get; }

    int LowerBitrate { get; }

    TimeSpan TotalTime { get; }

    long TotalSamples { get; }

    bool ClipSamples { get; set; }

    TimeSpan TimePosition { get; set; }

    long SamplePosition { get; set; }

    bool HasClipped { get; }

    bool IsEndOfStream { get; }

    IStreamStats StreamStats { get; }

    ITagData Tags { get; }

    bool FindNextStream();

    bool SwitchStreams(int index);

    int ReadSamples(float[] buffer, int offset, int count);

    void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = 0);

    void SeekTo(long samplePosition, SeekOrigin seekOrigin = 0);
  }
}
