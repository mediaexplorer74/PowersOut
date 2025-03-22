
// Type: NVorbis.Ogg.PacketProvider
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using NVorbis.Contracts.Ogg;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NVorbis.Ogg
{
  internal class PacketProvider : NVorbis.Contracts.IPacketProvider, IPacketReader
  {
    private IStreamPageReader _reader;
    private int _pageIndex;
    private int _packetIndex;
    private int _lastPacketPageIndex;
    private int _lastPacketPacketIndex;
    private Packet _lastPacket;
    private int _nextPacketPageIndex;
    private int _nextPacketPacketIndex;

    public bool CanSeek => true;

    public int StreamSerial { get; }

    internal PacketProvider(IStreamPageReader reader, int streamSerial)
    {
      this._reader = reader ?? throw new ArgumentNullException(nameof (reader));
      this.StreamSerial = streamSerial;
    }

    public long GetGranuleCount()
    {
      if (this._reader == null)
        throw new ObjectDisposedException(nameof (PacketProvider));
      if (!this._reader.HasAllPages)
        this._reader.GetPage(int.MaxValue, out long _, out bool _, out bool _, out bool _, out int _, out int _);
      return this._reader.MaxGranulePosition.Value;
    }

    public IPacket GetNextPacket()
    {
      return (IPacket) this.GetNextPacket(ref this._pageIndex, ref this._packetIndex);
    }

    public IPacket PeekNextPacket()
    {
      int pageIndex = this._pageIndex;
      int packetIndex = this._packetIndex;
      return (IPacket) this.GetNextPacket(ref pageIndex, ref packetIndex);
    }

    public long SeekTo(long granulePos, int preRoll, GetPacketGranuleCount getPacketGranuleCount)
    {
      if (this._reader == null)
        throw new ObjectDisposedException(nameof (PacketProvider));
      int pageIndex;
      int packetIndex;
      if (granulePos == 0L)
      {
        pageIndex = this._reader.FirstDataPageIndex;
        packetIndex = 0;
      }
      else
      {
        pageIndex = this._reader.FindPage(granulePos);
        if (this._reader.HasAllPages)
        {
          long? maxGranulePosition = this._reader.MaxGranulePosition;
          long num = granulePos;
          if (maxGranulePosition.GetValueOrDefault() == num & maxGranulePosition.HasValue)
          {
            this._lastPacket = (Packet) null;
            this._pageIndex = pageIndex;
            this._packetIndex = 0;
            return granulePos;
          }
        }
        packetIndex = this.FindPacket(pageIndex, ref granulePos, getPacketGranuleCount) - preRoll;
      }
      if (!this.NormalizePacketIndex(ref pageIndex, ref packetIndex))
        throw new ArgumentOutOfRangeException(nameof (granulePos));
      if (pageIndex < this._reader.FirstDataPageIndex)
      {
        pageIndex = this._reader.FirstDataPageIndex;
        packetIndex = 0;
      }
      this._lastPacket = (Packet) null;
      this._pageIndex = pageIndex;
      this._packetIndex = packetIndex;
      return granulePos;
    }

    private int FindPacket(
      int pageIndex,
      ref long granulePos,
      GetPacketGranuleCount getPacketGranuleCount)
    {
      int num = 0;
      bool isContinued;
      int pageOverhead;
      if (!this._reader.GetPage(pageIndex - 1, out long _, out bool _, out bool _, out isContinued, out int _, out pageOverhead))
        throw new InvalidDataException("Could not get page?!");
      if (isContinued)
        num = 1;
      long granulePos1;
      bool isResync;
      bool isContinuation;
      int packetCount;
      if (!this._reader.GetPage(pageIndex, out granulePos1, out isResync, out isContinuation, out isContinued, out packetCount, out pageOverhead))
        throw new InvalidDataException("Could not get found page?!");
      if (isContinued)
        --packetCount;
      int packetIndex1 = packetCount;
      bool isLastInPage = !isContinued;
      long granulePos2 = granulePos1;
      while (granulePos2 > granulePos && --packetIndex1 >= num)
      {
        Packet packet = this.CreatePacket(ref pageIndex, ref packetIndex1, false, granulePos1, packetIndex1 == 0 & isResync, isContinued, packetCount, 0);
        if (packet == null)
          throw new InvalidDataException("Could not find end of continuation!");
        granulePos2 -= (long) getPacketGranuleCount((IPacket) packet, isLastInPage);
        isLastInPage = false;
      }
      if (packetIndex1 < num)
      {
        int pageIndex1 = pageIndex;
        int packetIndex2 = -1;
        if (!this.NormalizePacketIndex(ref pageIndex1, ref packetIndex2))
          throw new InvalidDataException("Failed to normalize packet index?");
        Packet packet = this.CreatePacket(ref pageIndex1, ref packetIndex2, false, granulePos2, false, isContinuation, packetIndex2 + 1, 0);
        if (packet == null)
          throw new InvalidDataException("Could not load previous packet!");
        granulePos = granulePos2 - (long) getPacketGranuleCount((IPacket) packet, false);
        return -1;
      }
      granulePos = granulePos2;
      return packetIndex1;
    }

    private bool NormalizePacketIndex(ref int pageIndex, ref int packetIndex)
    {
      long granulePos;
      bool isResync;
      bool isContinuation;
      int pageOverhead;
      if (!this._reader.GetPage(pageIndex, out granulePos, out isResync, out isContinuation, out bool _, out int _, out pageOverhead))
        return false;
      int num1 = pageIndex;
      int num2;
      bool flag;
      int packetCount;
      for (num2 = packetIndex; num2 < (isContinuation ? 1 : 0); num2 += packetCount - (flag ? 1 : 0))
      {
        if (isContinuation & isResync)
          return false;
        flag = isContinuation;
        bool isContinued;
        if (!this._reader.GetPage(--num1, out granulePos, out isResync, out isContinuation, out isContinued, out packetCount, out pageOverhead) || flag && !isContinued)
          return false;
      }
      pageIndex = num1;
      packetIndex = num2;
      return true;
    }

    private Packet GetNextPacket(ref int pageIndex, ref int packetIndex)
    {
      if (this._reader == null)
        throw new ObjectDisposedException(nameof (PacketProvider));
      if (this._lastPacketPacketIndex != packetIndex || this._lastPacketPageIndex != pageIndex || this._lastPacket == null)
      {
        this._lastPacket = (Packet) null;
        long granulePos;
        bool isResync;
        bool isContinued;
        int packetCount;
        int pageOverhead;
        if (this._reader.GetPage(pageIndex, out granulePos, out isResync, out bool _, out isContinued, out packetCount, out pageOverhead))
        {
          this._lastPacketPageIndex = pageIndex;
          this._lastPacketPacketIndex = packetIndex;
          this._lastPacket = this.CreatePacket(ref pageIndex, ref packetIndex, true, granulePos, isResync, isContinued, packetCount, pageOverhead);
          this._nextPacketPageIndex = pageIndex;
          this._nextPacketPacketIndex = packetIndex;
        }
      }
      else
      {
        pageIndex = this._nextPacketPageIndex;
        packetIndex = this._nextPacketPacketIndex;
      }
      return this._lastPacket;
    }

    private Packet CreatePacket(
      ref int pageIndex,
      ref int packetIndex,
      bool advance,
      long granulePos,
      bool isResync,
      bool isContinued,
      int packetCount,
      int pageOverhead)
    {
      Memory<byte> pagePacket = this._reader.GetPagePackets(pageIndex)[packetIndex];
      List<int> intList = new List<int>(2);
      intList.Add(pageIndex << 8 | packetIndex);
      List<int> dataParts = intList;
      int num1 = pageIndex;
      bool flag1;
      bool flag2;
      if (isContinued && packetIndex == packetCount - 1)
      {
        flag1 = true;
        if (packetIndex > 0)
          pageOverhead = 0;
        int num2 = pageIndex;
        while (isContinued)
        {
          bool isContinuation;
          int pageOverhead1;
          if (!this._reader.GetPage(++num2, out granulePos, out isResync, out isContinuation, out isContinued, out packetCount, out pageOverhead1))
            return (Packet) null;
          pageOverhead += pageOverhead1;
          if (!(!isContinuation | isResync))
          {
            if (isContinued && packetCount > 1)
              isContinued = false;
            dataParts.Add(num2 << 8);
          }
          else
            break;
        }
        flag2 = packetCount == 1;
        num1 = num2;
      }
      else
      {
        flag1 = packetIndex == 0;
        flag2 = packetIndex == packetCount - 1;
      }
      Packet packet1 = new Packet((IReadOnlyList<int>) dataParts, (IPacketReader) this, pagePacket);
      packet1.IsResync = isResync;
      Packet packet2 = packet1;
      if (flag1)
        packet2.ContainerOverheadBits = pageOverhead * 8;
      if (flag2)
      {
        packet2.GranulePosition = new long?(granulePos);
        if (this._reader.HasAllPages && num1 == this._reader.PageCount - 1)
          packet2.IsEndOfStream = true;
      }
      if (advance)
      {
        if (num1 != pageIndex)
        {
          pageIndex = num1;
          packetIndex = 0;
        }
        if (packetIndex == packetCount - 1)
        {
          ++pageIndex;
          packetIndex = 0;
        }
        else
          ++packetIndex;
      }
      return packet2;
    }

    Memory<byte> IPacketReader.GetPacketData(int pagePacketIndex)
    {
      int pageIndex = pagePacketIndex >> 8 & 16777215;
      int index = pagePacketIndex & (int) byte.MaxValue;
      Memory<byte>[] pagePackets = this._reader.GetPagePackets(pageIndex);
      return index < pagePackets.Length ? pagePackets[index] : Memory<byte>.Empty;
    }

    void IPacketReader.InvalidatePacketCache(IPacket packet)
    {
      if (this._lastPacket != packet)
        return;
      this._lastPacket = (Packet) null;
    }
  }
}
