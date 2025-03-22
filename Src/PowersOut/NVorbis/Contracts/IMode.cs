﻿
// Type: NVorbis.Contracts.IMode
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

#nullable disable
namespace NVorbis.Contracts
{
  internal interface IMode
  {
    int BlockSize { get; }

    float[][] Windows { get; }

    void Init(IPacket packet, int channels, int block0Size, int block1Size, IMapping[] mappings);

    bool Decode(
      IPacket packet,
      float[][] buffer,
      out int packetStartindex,
      out int packetValidLength,
      out int packetTotalLength);

    int GetPacketSampleCount(IPacket packet, bool isLastInPage);
  }
}
