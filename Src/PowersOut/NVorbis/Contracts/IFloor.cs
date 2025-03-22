
// Type: NVorbis.Contracts.IFloor
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

#nullable disable
namespace NVorbis.Contracts
{
  internal interface IFloor
  {
    void Init(
      IPacket packet,
      int channels,
      int block0Size,
      int block1Size,
      ICodebook[] codebooks);

    IFloorData Unpack(IPacket packet, int blockSize, int channel);

    void Apply(IFloorData floorData, int blockSize, float[] residue);
  }
}
