
// Type: NVorbis.Contracts.Ogg.IPageReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;

#nullable disable
namespace NVorbis.Contracts.Ogg
{
  internal interface IPageReader : IDisposable
  {
    void Lock();

    bool Release();

    long ContainerBits { get; }

    long WasteBits { get; }

    bool ReadNextPage();

    bool ReadPageAt(long offset);
  }
}
