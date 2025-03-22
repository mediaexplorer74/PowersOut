
// Type: NVorbis.StreamStats
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;

#nullable disable
namespace NVorbis
{
  internal class StreamStats : IStreamStats
  {
    private int _sampleRate;
    private readonly int[] _packetBits = new int[2];
    private readonly int[] _packetSamples = new int[2];
    private int _packetIndex;
    private long _totalSamples;
    private long _audioBits;
    private long _headerBits;
    private long _containerBits;
    private long _wasteBits;
    private object _lock = new object();
    private int _packetCount;

    public int EffectiveBitRate
    {
      get
      {
        long totalSamples;
        long num;
        lock (this._lock)
        {
          totalSamples = this._totalSamples;
          num = this._audioBits + this._headerBits + this._containerBits + this._wasteBits;
        }
        return totalSamples > 0L ? (int) ((double) num / (double) totalSamples * (double) this._sampleRate) : 0;
      }
    }

    public int InstantBitRate
    {
      get
      {
        int num1;
        int num2;
        lock (this._lock)
        {
          num1 = this._packetBits[0] + this._packetBits[1];
          num2 = this._packetSamples[0] + this._packetSamples[1];
        }
        return num2 > 0 ? (int) ((double) num1 / (double) num2 * (double) this._sampleRate) : 0;
      }
    }

    public long ContainerBits => this._containerBits;

    public long OverheadBits => this._headerBits;

    public long AudioBits => this._audioBits;

    public long WasteBits => this._wasteBits;

    public int PacketCount => this._packetCount;

    public void ResetStats()
    {
      lock (this._lock)
      {
        this._packetBits[0] = this._packetBits[1] = 0;
        this._packetSamples[0] = this._packetSamples[1] = 0;
        this._packetIndex = 0;
        this._packetCount = 0;
        this._audioBits = 0L;
        this._totalSamples = 0L;
        this._headerBits = 0L;
        this._containerBits = 0L;
        this._wasteBits = 0L;
      }
    }

    internal void SetSampleRate(int sampleRate)
    {
      lock (this._lock)
      {
        this._sampleRate = sampleRate;
        this.ResetStats();
      }
    }

    internal void AddPacket(int samples, int bits, int waste, int container)
    {
      lock (this._lock)
      {
        if (samples >= 0)
        {
          this._audioBits += (long) bits;
          this._wasteBits += (long) waste;
          this._containerBits += (long) container;
          this._totalSamples += (long) samples;
          this._packetBits[this._packetIndex] = bits + waste;
          this._packetSamples[this._packetIndex] = samples;
          if (++this._packetIndex != 2)
            return;
          this._packetIndex = 0;
        }
        else
        {
          this._headerBits += (long) bits;
          this._wasteBits += (long) waste;
          this._containerBits += (long) container;
        }
      }
    }
  }
}
