
// Type: NVorbis.Residue1
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;

#nullable disable
namespace NVorbis
{
  internal class Residue1 : Residue0
  {
    protected override bool WriteVectors(
      ICodebook codebook,
      IPacket packet,
      float[][] residue,
      int channel,
      int offset,
      int partitionSize)
    {
      float[] numArray = residue[channel];
      int num = 0;
      while (num < partitionSize)
      {
        int entry = codebook.DecodeScalar(packet);
        if (entry == -1)
          return true;
        for (int dim = 0; dim < codebook.Dimensions; ++dim)
        {
          numArray[offset + num] += codebook[entry, dim];
          ++num;
        }
      }
      return false;
    }
  }
}
