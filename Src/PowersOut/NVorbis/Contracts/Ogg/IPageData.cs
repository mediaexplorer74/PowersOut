
// Type: NVorbis.Contracts.Ogg.IPageData
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;

#nullable disable
namespace NVorbis.Contracts.Ogg
{
  internal interface IPageData : IPageReader, IDisposable
  {
    long PageOffset { get; }

    int StreamSerial { get; }

    int SequenceNumber { get; }

    PageFlags PageFlags { get; }

    long GranulePosition { get; }

    short PacketCount { get; }

    bool? IsResync { get; }

    bool IsContinued { get; }

    int PageOverhead { get; }

    Memory<byte>[] GetPackets();
  }
}
