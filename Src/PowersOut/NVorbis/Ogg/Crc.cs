
// Type: NVorbis.Ogg.Crc
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts.Ogg;

#nullable disable
namespace NVorbis.Ogg
{
  internal class Crc : ICrc
  {
    private const uint CRC32_POLY = 79764919;
    private static readonly uint[] s_crcTable = new uint[256];
    private uint _crc;

    static Crc()
    {
      for (uint index1 = 0; index1 < 256U; ++index1)
      {
        uint num = index1 << 24;
        for (int index2 = 0; index2 < 8; ++index2)
          num = (uint) ((int) num << 1 ^ (num >= 2147483648U ? 79764919 : 0));
        Crc.s_crcTable[(int) index1] = num;
      }
    }

    public Crc() => this.Reset();

    public void Reset() => this._crc = 0U;

    public void Update(int nextVal)
    {
      this._crc = this._crc << 8 ^ Crc.s_crcTable[(long) nextVal ^ (long) (this._crc >> 24)];
    }

    public bool Test(uint checkCrc) => (int) this._crc == (int) checkCrc;
  }
}
