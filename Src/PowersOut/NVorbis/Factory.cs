
// Type: NVorbis.Factory
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System.IO;

#nullable disable
namespace NVorbis
{
  internal class Factory : IFactory
  {
    public IHuffman CreateHuffman() => (IHuffman) new Huffman();

    public IMdct CreateMdct() => (IMdct) new Mdct();

    public ICodebook CreateCodebook() => (ICodebook) new Codebook();

    public IFloor CreateFloor(IPacket packet)
    {
      switch ((int) packet.ReadBits(16))
      {
        case 0:
          return (IFloor) new Floor0();
        case 1:
          return (IFloor) new Floor1();
        default:
          throw new InvalidDataException("Invalid floor type!");
      }
    }

    public IMapping CreateMapping(IPacket packet)
    {
      if (packet.ReadBits(16) != 0UL)
        throw new InvalidDataException("Invalid mapping type!");
      return (IMapping) new Mapping();
    }

    public IMode CreateMode() => (IMode) new Mode();

    public IResidue CreateResidue(IPacket packet)
    {
      switch ((int) packet.ReadBits(16))
      {
        case 0:
          return (IResidue) new Residue0();
        case 1:
          return (IResidue) new Residue1();
        case 2:
          return (IResidue) new Residue2();
        default:
          throw new InvalidDataException("Invalid residue type!");
      }
    }
  }
}
