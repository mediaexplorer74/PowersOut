
// Type: NVorbis.Residue2
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;

#nullable disable
namespace NVorbis
{
  internal class Residue2 : Residue0
  {
    private int _channels;

    public override void Init(IPacket packet, int channels, ICodebook[] codebooks)
    {
      this._channels = channels;
      base.Init(packet, 1, codebooks);
    }

    public override void Decode(
      IPacket packet,
      bool[] doNotDecodeChannel,
      int blockSize,
      float[][] buffer)
    {
      base.Decode(packet, doNotDecodeChannel, blockSize * this._channels, buffer);
    }

    protected override bool WriteVectors(
      ICodebook codebook,
      IPacket packet,
      float[][] residue,
      int channel,
      int offset,
      int partitionSize)
    {
      int index = 0;
      offset /= this._channels;
      int num = 0;
      while (num < partitionSize)
      {
        int entry = codebook.DecodeScalar(packet);
        if (entry == -1)
          return true;
        int dim = 0;
        while (dim < codebook.Dimensions)
        {
          residue[index][offset] += codebook[entry, dim];
          if (++index == this._channels)
          {
            index = 0;
            ++offset;
          }
          ++dim;
          ++num;
        }
      }
      return false;
    }
  }
}
