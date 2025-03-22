
// Type: NVorbis.DataPacket
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;

#nullable disable
namespace NVorbis
{
  public abstract class DataPacket : IPacket
  {
    private ulong _bitBucket;
    private int _bitCount;
    private byte _overflowBits;
    private DataPacket.PacketFlags _packetFlags;
    private int _readBits;

    public int ContainerOverheadBits { get; set; }

    public long? GranulePosition { get; set; }

    public bool IsResync
    {
      get => this.GetFlag(DataPacket.PacketFlags.IsResync);
      set => this.SetFlag(DataPacket.PacketFlags.IsResync, value);
    }

    public bool IsShort
    {
      get => this.GetFlag(DataPacket.PacketFlags.IsShort);
      private set => this.SetFlag(DataPacket.PacketFlags.IsShort, value);
    }

    public bool IsEndOfStream
    {
      get => this.GetFlag(DataPacket.PacketFlags.IsEndOfStream);
      set => this.SetFlag(DataPacket.PacketFlags.IsEndOfStream, value);
    }

    public int BitsRead => this._readBits;

    public int BitsRemaining => this.TotalBits - this._readBits;

    protected abstract int TotalBits { get; }

    private bool GetFlag(DataPacket.PacketFlags flag) => this._packetFlags.HasFlag((Enum) flag);

    private void SetFlag(DataPacket.PacketFlags flag, bool value)
    {
      if (value)
        this._packetFlags |= flag;
      else
        this._packetFlags &= ~flag;
    }

    protected abstract int ReadNextByte();

    public virtual void Done()
    {
    }

    public virtual void Reset()
    {
      this._bitBucket = 0UL;
      this._bitCount = 0;
      this._overflowBits = (byte) 0;
      this._readBits = 0;
    }

    ulong IPacket.ReadBits(int count)
    {
      if (count == 0)
        return 0;
      long num = (long) this.TryPeekBits(count, out int _);
      this.SkipBits(count);
      return (ulong) num;
    }

    public ulong TryPeekBits(int count, out int bitsRead)
    {
      if (count < 0 || count > 64)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (count == 0)
      {
        bitsRead = 0;
        return 0;
      }
      while (this._bitCount < count)
      {
        int num = this.ReadNextByte();
        if (num == -1)
        {
          bitsRead = this._bitCount;
          return this._bitBucket;
        }
        this._bitBucket = (ulong) (num & (int) byte.MaxValue) << this._bitCount | this._bitBucket;
        this._bitCount += 8;
        if (this._bitCount > 64)
          this._overflowBits = (byte) (num >> 72 - this._bitCount);
      }
      ulong bitBucket = this._bitBucket;
      if (count < 64)
        bitBucket &= (ulong) ((1L << count) - 1L);
      bitsRead = count;
      return bitBucket;
    }

    public void SkipBits(int count)
    {
      if (count <= 0)
        return;
      if (this._bitCount > count)
      {
        if (count > 63)
          this._bitBucket = 0UL;
        else
          this._bitBucket >>= count;
        if (this._bitCount > 64)
        {
          int num = this._bitCount - 64;
          this._bitBucket |= (ulong) this._overflowBits << this._bitCount - count - num;
          if (num > count)
            this._overflowBits >>= count;
        }
        this._bitCount -= count;
        this._readBits += count;
      }
      else if (this._bitCount == count)
      {
        this._bitBucket = 0UL;
        this._bitCount = 0;
        this._readBits += count;
      }
      else
      {
        count -= this._bitCount;
        this._readBits += this._bitCount;
        this._bitCount = 0;
        this._bitBucket = 0UL;
        while (count > 8)
        {
          if (this.ReadNextByte() == -1)
          {
            count = 0;
            this.IsShort = true;
            break;
          }
          count -= 8;
          this._readBits += 8;
        }
        if (count <= 0)
          return;
        int num = this.ReadNextByte();
        if (num == -1)
        {
          this.IsShort = true;
        }
        else
        {
          this._bitBucket = (ulong) (num >> count);
          this._bitCount = 8 - count;
          this._readBits += count;
        }
      }
    }

    [Flags]
    protected enum PacketFlags : byte
    {
      IsResync = 1,
      IsEndOfStream = 2,
      IsShort = 4,
      User0 = 8,
      User1 = 16, // 0x10
      User2 = 32, // 0x20
      User3 = 64, // 0x40
      User4 = 128, // 0x80
    }
  }
}
