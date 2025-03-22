
// Type: NVorbis.Contracts.Ogg.IStreamPageReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;

#nullable disable
namespace NVorbis.Contracts.Ogg
{
  internal interface IStreamPageReader
  {
    NVorbis.Contracts.IPacketProvider PacketProvider { get; }

    void AddPage();

    Memory<byte>[] GetPagePackets(int pageIndex);

    int FindPage(long granulePos);

    bool GetPage(
      int pageIndex,
      out long granulePos,
      out bool isResync,
      out bool isContinuation,
      out bool isContinued,
      out int packetCount,
      out int pageOverhead);

    void SetEndOfStream();

    int PageCount { get; }

    bool HasAllPages { get; }

    long? MaxGranulePosition { get; }

    int FirstDataPageIndex { get; }
  }
}
