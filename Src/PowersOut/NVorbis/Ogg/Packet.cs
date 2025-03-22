
// Type: NVorbis.Ogg.Packet
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
  internal class Packet : DataPacket
  {
    private IReadOnlyList<int> _dataParts;
    private IPacketReader _packetReader;
    private int _dataCount;
    private Memory<byte> _data;
    private int _dataIndex;
    private int _dataOfs;

    internal Packet(
      IReadOnlyList<int> dataParts,
      IPacketReader packetReader,
      Memory<byte> initialData)
    {
      this._dataParts = dataParts;
      this._packetReader = packetReader;
      this._data = initialData;
    }

    protected override int TotalBits => (this._dataCount + this._data.Length) * 8;

    protected override int ReadNextByte()
    {
      if (this._dataIndex == ((IReadOnlyCollection<int>) this._dataParts).Count)
        return -1;
      int num = (int) this._data.Span[this._dataOfs];
      if (++this._dataOfs != this._data.Length)
        return num;
      this._dataOfs = 0;
      this._dataCount += this._data.Length;
      if (++this._dataIndex < ((IReadOnlyCollection<int>) this._dataParts).Count)
      {
        this._data = this._packetReader.GetPacketData(this._dataParts[this._dataIndex]);
        return num;
      }
      this._data = Memory<byte>.Empty;
      return num;
    }

    public override void Reset()
    {
      this._dataIndex = 0;
      this._dataOfs = 0;
      if (((IReadOnlyCollection<int>) this._dataParts).Count > 0)
        this._data = this._packetReader.GetPacketData(this._dataParts[0]);
      base.Reset();
    }

    public override void Done()
    {
      this._packetReader?.InvalidatePacketCache((IPacket) this);
      base.Done();
    }
  }
}
