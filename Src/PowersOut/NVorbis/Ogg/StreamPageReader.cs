
// Type: NVorbis.Ogg.StreamPageReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts.Ogg;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NVorbis.Ogg
{
  internal class StreamPageReader : IStreamPageReader
  {
    private readonly IPageData _reader;
    private readonly List<long> _pageOffsets = new List<long>();
    private int _lastSeqNbr;
    private int? _firstDataPageIndex;
    private long _maxGranulePos;
    private int _lastPageIndex = -1;
    private long _lastPageGranulePos;
    private bool _lastPageIsResync;
    private bool _lastPageIsContinuation;
    private bool _lastPageIsContinued;
    private int _lastPagePacketCount;
    private int _lastPageOverhead;
    private Memory<byte>[] _cachedPagePackets;

    internal static Func<IStreamPageReader, int, NVorbis.Contracts.IPacketProvider> CreatePacketProvider { get; set; } = (Func<IStreamPageReader, int, NVorbis.Contracts.IPacketProvider>) ((pr, ss) => (NVorbis.Contracts.IPacketProvider) new NVorbis.Ogg.PacketProvider(pr, ss));

    public NVorbis.Contracts.IPacketProvider PacketProvider { get; private set; }

    public StreamPageReader(IPageData pageReader, int streamSerial)
    {
      this._reader = pageReader;
      this.PacketProvider = StreamPageReader.CreatePacketProvider((IStreamPageReader) this, streamSerial);
    }

    public void AddPage()
    {
      if (this.HasAllPages)
        return;
      if (this._reader.GranulePosition != -1L)
      {
        if (!this._firstDataPageIndex.HasValue && this._reader.GranulePosition > 0L)
          this._firstDataPageIndex = new int?(this._pageOffsets.Count);
        else if (this._maxGranulePos > this._reader.GranulePosition)
          throw new InvalidDataException("Granule Position regressed?!");
        this._maxGranulePos = this._reader.GranulePosition;
      }
      else if (this._firstDataPageIndex.HasValue && (!this._reader.IsContinued || this._reader.PacketCount != (short) 1))
        throw new InvalidDataException("Granule Position was -1 but page does not have exactly 1 continued packet.");
      if ((this._reader.PageFlags & PageFlags.EndOfStream) != PageFlags.None)
        this.HasAllPages = true;
      if (this._reader.IsResync.Value || this._lastSeqNbr != 0 && this._lastSeqNbr + 1 != this._reader.SequenceNumber)
        this._pageOffsets.Add(-this._reader.PageOffset);
      else
        this._pageOffsets.Add(this._reader.PageOffset);
      this._lastSeqNbr = this._reader.SequenceNumber;
    }

    public Memory<byte>[] GetPagePackets(int pageIndex)
    {
      if (this._cachedPagePackets != null && this._lastPageIndex == pageIndex)
        return this._cachedPagePackets;
      long offset = this._pageOffsets[pageIndex];
      if (offset < 0L)
        offset = -offset;
      this._reader.Lock();
      try
      {
        this._reader.ReadPageAt(offset);
        Memory<byte>[] packets = this._reader.GetPackets();
        if (pageIndex == this._lastPageIndex)
          this._cachedPagePackets = packets;
        return packets;
      }
      finally
      {
        this._reader.Release();
      }
    }

    public int FindPage(long granulePos)
    {
      int num1 = -1;
      if (granulePos == 0L)
      {
        num1 = this.FindFirstDataPage();
      }
      else
      {
        int num2 = this._pageOffsets.Count - 1;
        long pageGranulePos;
        if (this.GetPageRaw(num2, out pageGranulePos))
          num1 = granulePos >= pageGranulePos ? (granulePos <= pageGranulePos ? num2 + 1 : this.FindPageForward(num2, pageGranulePos, granulePos)) : this.FindPageBisection(granulePos, this.FindFirstDataPage(), num2, pageGranulePos);
      }
      return num1 != -1 ? num1 : throw new ArgumentOutOfRangeException(nameof (granulePos));
    }

    private int FindFirstDataPage()
    {
      while (!this._firstDataPageIndex.HasValue)
      {
        if (!this.GetPageRaw(this._pageOffsets.Count, out long _))
          return -1;
      }
      return this._firstDataPageIndex.Value;
    }

    private int FindPageForward(int pageIndex, long pageGranulePos, long granulePos)
    {
      while (pageGranulePos <= granulePos)
      {
        if (++pageIndex == this._pageOffsets.Count)
        {
          if (!this.GetNextPageGranulePos(out pageGranulePos))
          {
            long? maxGranulePosition = this.MaxGranulePosition;
            long num = granulePos;
            if (maxGranulePosition.GetValueOrDefault() < num & maxGranulePosition.HasValue)
            {
              pageIndex = -1;
              break;
            }
            break;
          }
        }
        else if (!this.GetPageRaw(pageIndex, out pageGranulePos))
        {
          pageIndex = -1;
          break;
        }
      }
      return pageIndex;
    }

    private bool GetNextPageGranulePos(out long granulePos)
    {
      int count = this._pageOffsets.Count;
      while (count == this._pageOffsets.Count && !this.HasAllPages)
      {
        this._reader.Lock();
        try
        {
          if (!this._reader.ReadNextPage())
            this.HasAllPages = true;
          else if (count < this._pageOffsets.Count)
          {
            granulePos = this._reader.GranulePosition;
            return true;
          }
        }
        finally
        {
          this._reader.Release();
        }
      }
      granulePos = 0L;
      return false;
    }

    private int FindPageBisection(long granulePos, int low, int high, long highGranulePos)
    {
      long num1 = 0;
      int num2;
      while ((num2 = high - low) > 0)
      {
        int pageIndex = low + (int) ((double) num2 * ((double) (granulePos - num1) / (double) (highGranulePos - num1)));
        long pageGranulePos;
        if (!this.GetPageRaw(pageIndex, out pageGranulePos))
          return -1;
        if (pageGranulePos > granulePos)
        {
          high = pageIndex;
          highGranulePos = pageGranulePos;
        }
        else
        {
          if (pageGranulePos >= granulePos)
            return pageIndex + 1;
          low = pageIndex + 1;
          num1 = pageGranulePos + 1L;
        }
      }
      return low;
    }

    private bool GetPageRaw(int pageIndex, out long pageGranulePos)
    {
      long offset = this._pageOffsets[pageIndex];
      if (offset < 0L)
        offset = -offset;
      this._reader.Lock();
      try
      {
        if (this._reader.ReadPageAt(offset))
        {
          pageGranulePos = this._reader.GranulePosition;
          return true;
        }
        pageGranulePos = 0L;
        return false;
      }
      finally
      {
        this._reader.Release();
      }
    }

    public bool GetPage(
      int pageIndex,
      out long granulePos,
      out bool isResync,
      out bool isContinuation,
      out bool isContinued,
      out int packetCount,
      out int pageOverhead)
    {
      if (this._lastPageIndex == pageIndex)
      {
        granulePos = this._lastPageGranulePos;
        isResync = this._lastPageIsResync;
        isContinuation = this._lastPageIsContinuation;
        isContinued = this._lastPageIsContinued;
        packetCount = this._lastPagePacketCount;
        pageOverhead = this._lastPageOverhead;
        return true;
      }
      this._reader.Lock();
      try
      {
        while (pageIndex >= this._pageOffsets.Count)
        {
          if (!this.HasAllPages)
          {
            if (this._reader.ReadNextPage())
            {
              if (pageIndex < this._pageOffsets.Count)
              {
                isResync = this._reader.IsResync.Value;
                this.ReadPageData(pageIndex, out granulePos, out isContinuation, out isContinued, out packetCount, out pageOverhead);
                return true;
              }
            }
            else
              break;
          }
          else
            break;
        }
      }
      finally
      {
        this._reader.Release();
      }
      if (pageIndex < this._pageOffsets.Count)
      {
        long offset = this._pageOffsets[pageIndex];
        if (offset < 0L)
        {
          isResync = true;
          offset = -offset;
        }
        else
          isResync = false;
        this._reader.Lock();
        try
        {
          if (this._reader.ReadPageAt(offset))
          {
            this._lastPageIsResync = isResync;
            this.ReadPageData(pageIndex, out granulePos, out isContinuation, out isContinued, out packetCount, out pageOverhead);
            return true;
          }
        }
        finally
        {
          this._reader.Release();
        }
      }
      granulePos = 0L;
      isResync = false;
      isContinuation = false;
      isContinued = false;
      packetCount = 0;
      pageOverhead = 0;
      return false;
    }

    private void ReadPageData(
      int pageIndex,
      out long granulePos,
      out bool isContinuation,
      out bool isContinued,
      out int packetCount,
      out int pageOverhead)
    {
      this._cachedPagePackets = (Memory<byte>[]) null;
      this._lastPageGranulePos = granulePos = this._reader.GranulePosition;
      this._lastPageIsContinuation = isContinuation = (this._reader.PageFlags & PageFlags.ContinuesPacket) != 0;
      this._lastPageIsContinued = isContinued = this._reader.IsContinued;
      this._lastPagePacketCount = packetCount = (int) this._reader.PacketCount;
      this._lastPageOverhead = pageOverhead = this._reader.PageOverhead;
      this._lastPageIndex = pageIndex;
    }

    public void SetEndOfStream() => this.HasAllPages = true;

    public int PageCount => this._pageOffsets.Count;

    public bool HasAllPages { get; private set; }

    public long? MaxGranulePosition
    {
      get => !this.HasAllPages ? new long?() : new long?(this._maxGranulePos);
    }

    public int FirstDataPageIndex => this.FindFirstDataPage();
  }
}
