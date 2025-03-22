﻿
// Type: NVorbis.Contracts.ICodebook
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

#nullable disable
namespace NVorbis.Contracts
{
  internal interface ICodebook
  {
    void Init(IPacket packet, IHuffman huffman);

    int Dimensions { get; }

    int Entries { get; }

    int MapType { get; }

    float this[int entry, int dim] { get; }

    int DecodeScalar(IPacket packet);
  }
}
