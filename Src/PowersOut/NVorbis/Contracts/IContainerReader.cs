
// Type: NVorbis.Contracts.IContainerReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;
using System.Collections.Generic;

#nullable disable
namespace NVorbis.Contracts
{
  public interface IContainerReader : IDisposable
  {
    NewStreamHandler NewStreamCallback { get; set; }

    IReadOnlyList<IPacketProvider> GetStreams();

    bool CanSeek { get; }

    long ContainerBits { get; }

    long WasteBits { get; }

    bool TryInit();

    bool FindNextStream();
  }
}
