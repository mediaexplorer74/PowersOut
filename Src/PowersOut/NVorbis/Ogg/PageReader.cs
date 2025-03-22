
// Type: NVorbis.Ogg.PageReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts.Ogg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

#nullable disable
namespace NVorbis.Ogg
{
  internal class PageReader : PageReaderBase, IPageData, IPageReader, IDisposable
  {
    private readonly Dictionary<int, IStreamPageReader> _streamReaders = new Dictionary<int, IStreamPageReader>();
    private readonly Func<NVorbis.Contracts.IPacketProvider, bool> _newStreamCallback;
    private readonly object _readLock = new object();
    private long _nextPageOffset;
    private ushort _pageSize;
    private Memory<byte>[] _packets;

    internal static Func<IPageData, int, IStreamPageReader> CreateStreamPageReader { get; set; } = (Func<IPageData, int, IStreamPageReader>) ((pr, ss) => (IStreamPageReader) new StreamPageReader(pr, ss));

    public PageReader(
      Stream stream,
      bool closeOnDispose,
      Func<NVorbis.Contracts.IPacketProvider, bool> newStreamCallback)
      : base(stream, closeOnDispose)
    {
      this._newStreamCallback = newStreamCallback;
    }

    private ushort ParsePageHeader(byte[] pageBuf, int? streamSerial, bool? isResync)
    {
      byte num1 = pageBuf[26];
      int num2 = 0;
      int num3 = 0;
      bool flag = false;
      int num4 = 0;
      int num5 = 0;
      int index = 27;
      while (num5 < (int) num1)
      {
        byte num6 = pageBuf[index];
        num4 += (int) num6;
        num2 += (int) num6;
        if (num6 < byte.MaxValue)
        {
          if (num4 > 0)
            ++num3;
          num4 = 0;
        }
        ++num5;
        ++index;
      }
      if (num4 > 0)
      {
        flag = pageBuf[(int) num1 + 26] == byte.MaxValue;
        ++num3;
      }
      this.StreamSerial = streamSerial ?? BitConverter.ToInt32(pageBuf, 14);
      this.SequenceNumber = BitConverter.ToInt32(pageBuf, 18);
      this.PageFlags = (PageFlags) pageBuf[5];
      this.GranulePosition = BitConverter.ToInt64(pageBuf, 6);
      this.PacketCount = (short) num3;
      this.IsResync = isResync;
      this.IsContinued = flag;
      this.PageOverhead = 27 + (int) num1;
      return (ushort) (this.PageOverhead + num2);
    }

    private static Memory<byte>[] ReadPackets(
      int packetCount,
      Span<byte> segments,
      Memory<byte> dataBuffer)
    {
      Memory<byte>[] memoryArray = new Memory<byte>[packetCount];
      int index1 = 0;
      int num1 = 0;
      int num2 = 0;
      for (int index2 = 0; index2 < segments.Length; ++index2)
      {
        byte num3 = segments[index2];
        num2 += (int) num3;
        if (num3 < byte.MaxValue)
        {
          if (num2 > 0)
          {
            memoryArray[index1++] = dataBuffer.Slice(num1, num2);
            num1 += num2;
          }
          num2 = 0;
        }
      }
      if (num2 > 0)
        memoryArray[index1] = dataBuffer.Slice(num1, num2);
      return memoryArray;
    }

    public override void Lock() => Monitor.Enter(this._readLock);

    protected override bool CheckLock() => Monitor.IsEntered(this._readLock);

    public override bool Release()
    {
      if (!Monitor.IsEntered(this._readLock))
        return false;
      Monitor.Exit(this._readLock);
      return true;
    }

    protected override void SaveNextPageSearch() => this._nextPageOffset = this.StreamPosition;

    protected override void PrepareStreamForNextPage() => this.SeekStream(this._nextPageOffset);

    protected override bool AddPage(int streamSerial, byte[] pageBuf, bool isResync)
    {
      this.PageOffset = this.StreamPosition - (long) pageBuf.Length;
      int pageHeader = (int) this.ParsePageHeader(pageBuf, new int?(streamSerial), new bool?(isResync));
      if (this.PacketCount == (short) 0)
        return false;
      this._packets = PageReader.ReadPackets((int) this.PacketCount, new Span<byte>(pageBuf, 27, (int) pageBuf[26]), new Memory<byte>(pageBuf, 27 + (int) pageBuf[26], pageBuf.Length - 27 - (int) pageBuf[26]));
      IStreamPageReader streamPageReader1;
      if (this._streamReaders.TryGetValue(streamSerial, out streamPageReader1))
      {
        streamPageReader1.AddPage();
        if ((this.PageFlags & PageFlags.EndOfStream) == PageFlags.EndOfStream)
          this._streamReaders.Remove(this.StreamSerial);
      }
      else
      {
        IStreamPageReader streamPageReader2 = PageReader.CreateStreamPageReader((IPageData) this, this.StreamSerial);
        streamPageReader2.AddPage();
        this._streamReaders.Add(this.StreamSerial, streamPageReader2);
        if (!this._newStreamCallback(streamPageReader2.PacketProvider))
        {
          this._streamReaders.Remove(this.StreamSerial);
          return false;
        }
      }
      return true;
    }

    public override bool ReadPageAt(long offset)
    {
      if (!this.CheckLock())
        throw new InvalidOperationException("Must be locked prior to reading!");
      if (offset == this.PageOffset)
        return true;
      byte[] numArray = new byte[282];
      this.SeekStream(offset);
      int cnt = this.EnsureRead(numArray, 0, 27);
      this.PageOffset = offset;
      if (!this.VerifyHeader(numArray, 0, ref cnt))
        return false;
      this._packets = (Memory<byte>[]) null;
      this._pageSize = this.ParsePageHeader(numArray, new int?(), new bool?());
      return true;
    }

    protected override void SetEndOfStreams()
    {
      foreach (KeyValuePair<int, IStreamPageReader> streamReader in this._streamReaders)
        streamReader.Value.SetEndOfStream();
      this._streamReaders.Clear();
    }

    public long PageOffset { get; private set; }

    public int StreamSerial { get; private set; }

    public int SequenceNumber { get; private set; }

    public PageFlags PageFlags { get; private set; }

    public long GranulePosition { get; private set; }

    public short PacketCount { get; private set; }

    public bool? IsResync { get; private set; }

    public bool IsContinued { get; private set; }

    public int PageOverhead { get; private set; }

    public Memory<byte>[] GetPackets()
    {
      if (!this.CheckLock())
        throw new InvalidOperationException("Must be locked!");
      if (this._packets == null)
      {
        byte[] buf = new byte[(int) this._pageSize];
        this.SeekStream(this.PageOffset);
        this.EnsureRead(buf, 0, (int) this._pageSize);
        this._packets = PageReader.ReadPackets((int) this.PacketCount, new Span<byte>(buf, 27, (int) buf[26]), new Memory<byte>(buf, 27 + (int) buf[26], buf.Length - 27 - (int) buf[26]));
      }
      return this._packets;
    }
  }
}
