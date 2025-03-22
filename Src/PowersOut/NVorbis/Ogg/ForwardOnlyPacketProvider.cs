
// Type: NVorbis.Ogg.ForwardOnlyPacketProvider
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using NVorbis.Contracts.Ogg;
using System;
using System.Collections.Generic;

#nullable disable
namespace NVorbis.Ogg
{
  internal class ForwardOnlyPacketProvider : DataPacket, IForwardOnlyPacketProvider, NVorbis.Contracts.IPacketProvider
  {
    private int _lastSeqNo;
    private readonly Queue<(byte[] buf, bool isResync)> _pageQueue = new Queue<(byte[], bool)>();
    private readonly IPageReader _reader;
    private byte[] _pageBuf;
    private int _packetIndex;
    private bool _isEndOfStream;
    private int _dataStart;
    private bool _lastWasPeek;
    private Memory<byte> _packetBuf;
    private int _dataIndex;

    public ForwardOnlyPacketProvider(IPageReader reader, int streamSerial)
    {
      this._reader = reader;
      this.StreamSerial = streamSerial;
      this._packetIndex = int.MaxValue;
    }

    public bool CanSeek => false;

    public int StreamSerial { get; }

    public bool AddPage(byte[] buf, bool isResync)
    {
      if (((int) buf[5] & 2) != 0)
      {
        if (this._isEndOfStream)
          return false;
        isResync = true;
        this._lastSeqNo = BitConverter.ToInt32(buf, 18);
      }
      else
      {
        int int32 = BitConverter.ToInt32(buf, 18);
        isResync |= int32 != this._lastSeqNo + 1;
        this._lastSeqNo = int32;
      }
      int num = 0;
      for (int index = 0; index < (int) buf[26]; ++index)
        num += (int) buf[27 + index];
      if (num == 0)
        return false;
      this._pageQueue.Enqueue((buf, isResync));
      return true;
    }

    public void SetEndOfStream() => this._isEndOfStream = true;

    public IPacket GetNextPacket()
    {
      if (this._packetBuf.Length > 0)
      {
        this._lastWasPeek = this._lastWasPeek ? false : throw new InvalidOperationException("Must call Done() on previous packet first.");
        return (IPacket) this;
      }
      this._lastWasPeek = false;
      return this.GetPacket() ? (IPacket) this : (IPacket) null;
    }

    public IPacket PeekNextPacket()
    {
      if (this._packetBuf.Length > 0)
      {
        if (!this._lastWasPeek)
          throw new InvalidOperationException("Must call Done() on previous packet first.");
        return (IPacket) this;
      }
      this._lastWasPeek = true;
      return this.GetPacket() ? (IPacket) this : (IPacket) null;
    }

    private bool GetPacket()
    {
      byte[] pageBuf;
      bool isResync;
      int dataStart;
      int packetIndex1;
      bool isContinuation;
      bool isContinued;
      if (this._pageBuf != null && this._packetIndex < 27 + (int) this._pageBuf[26])
      {
        pageBuf = this._pageBuf;
        isResync = false;
        dataStart = this._dataStart;
        packetIndex1 = this._packetIndex;
        isContinuation = false;
        isContinued = pageBuf[26 + (int) pageBuf[26]] == byte.MaxValue;
      }
      else if (!this.ReadNextPage(out pageBuf, out isResync, out dataStart, out packetIndex1, out isContinuation, out isContinued))
        return false;
      int num = dataStart;
      bool flag1 = packetIndex1 == 27;
      if (isContinuation && flag1)
      {
        isResync = true;
        num += this.GetPacketLength(pageBuf, ref packetIndex1);
        if (packetIndex1 == 27 + (int) pageBuf[26])
          return this.GetPacket();
      }
      if (!flag1)
        num = 0;
      int packetLength1 = this.GetPacketLength(pageBuf, ref packetIndex1);
      Memory<byte> memory1 = new Memory<byte>(pageBuf, dataStart, packetLength1);
      dataStart += packetLength1;
      bool flag2 = packetIndex1 == 27 + (int) pageBuf[26];
      if (isContinued)
      {
        if (flag2)
        {
          flag2 = false;
        }
        else
        {
          int packetIndex2 = packetIndex1;
          this.GetPacketLength(pageBuf, ref packetIndex2);
          flag2 = packetIndex2 == 27 + (int) pageBuf[26];
        }
      }
      bool flag3 = false;
      long? nullable = new long?();
      if (flag2)
      {
        nullable = new long?(BitConverter.ToInt64(pageBuf, 6));
        if (((int) pageBuf[5] & 4) != 0 || this._isEndOfStream && this._pageQueue.Count == 0)
          flag3 = true;
      }
      else
      {
        while (isContinued && packetIndex1 == 27 + (int) pageBuf[26] && ((!this.ReadNextPage(out pageBuf, out isResync, out dataStart, out packetIndex1, out isContinuation, out isContinued) ? 0 : (!isResync ? 1 : 0)) & (isContinuation ? 1 : 0)) != 0)
        {
          num += 27 + (int) pageBuf[26];
          Memory<byte> memory2 = memory1;
          int packetLength2 = this.GetPacketLength(pageBuf, ref packetIndex1);
          memory1 = new Memory<byte>(new byte[memory2.Length + packetLength2]);
          memory2.CopyTo(memory1);
          new Memory<byte>(pageBuf, dataStart, packetLength2).CopyTo(memory1.Slice(memory2.Length));
          dataStart += packetLength2;
        }
      }
      this.IsResync = isResync;
      this.GranulePosition = nullable;
      this.IsEndOfStream = flag3;
      this.ContainerOverheadBits = num * 8;
      this._pageBuf = pageBuf;
      this._dataStart = dataStart;
      this._packetIndex = packetIndex1;
      this._packetBuf = memory1;
      this._isEndOfStream |= flag3;
      this.Reset();
      return true;
    }

    private bool ReadNextPage(
      out byte[] pageBuf,
      out bool isResync,
      out int dataStart,
      out int packetIndex,
      out bool isContinuation,
      out bool isContinued)
    {
      while (this._pageQueue.Count == 0)
      {
        if (this._isEndOfStream || !this._reader.ReadNextPage())
        {
          pageBuf = (byte[]) null;
          isResync = false;
          dataStart = 0;
          packetIndex = 0;
          isContinuation = false;
          isContinued = false;
          return false;
        }
      }
      (pageBuf, isResync) = this._pageQueue.Dequeue();
      dataStart = (int) pageBuf[26] + 27;
      packetIndex = 27;
      isContinuation = ((uint) pageBuf[5] & 1U) > 0U;
      isContinued = pageBuf[26 + (int) pageBuf[26]] == byte.MaxValue;
      return true;
    }

    private int GetPacketLength(byte[] pageBuf, ref int packetIndex)
    {
      int packetLength = 0;
      while (pageBuf[packetIndex] == byte.MaxValue && packetIndex < (int) pageBuf[26] + 27)
      {
        packetLength += (int) pageBuf[packetIndex];
        ++packetIndex;
      }
      if (packetIndex < (int) pageBuf[26] + 27)
      {
        packetLength += (int) pageBuf[packetIndex];
        ++packetIndex;
      }
      return packetLength;
    }

    protected override int TotalBits => this._packetBuf.Length * 8;

    protected override int ReadNextByte()
    {
      return this._dataIndex < this._packetBuf.Length ? (int) this._packetBuf.Span[this._dataIndex++] : -1;
    }

    public override void Reset()
    {
      this._dataIndex = 0;
      base.Reset();
    }

    public override void Done()
    {
      this._packetBuf = Memory<byte>.Empty;
      base.Done();
    }

    long NVorbis.Contracts.IPacketProvider.GetGranuleCount() => throw new NotSupportedException();

    long NVorbis.Contracts.IPacketProvider.SeekTo(
      long granulePos,
      int preRoll,
      GetPacketGranuleCount getPacketGranuleCount)
    {
      throw new NotSupportedException();
    }
  }
}
