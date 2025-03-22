
// Type: NVorbis.IContainerReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;

#nullable disable
namespace NVorbis
{
  [Obsolete("Moved to NVorbis.Contracts.IContainerReader", true)]
  public interface IContainerReader : NVorbis.Contracts.IContainerReader, IDisposable
  {
    [Obsolete("Use Streams.Select(s => s.StreamSerial).ToArray() instead.", true)]
    int[] StreamSerials { get; }

    [Obsolete("No longer supported.", true)]
    int PagesRead { get; }

    [Obsolete("Moved to NewStreamCallback.", true)]
    event EventHandler<NewStreamEventArgs> NewStream;

    [Obsolete("Renamed to TryInit().", true)]
    bool Init();

    [Obsolete("No longer supported.", true)]
    int GetTotalPageCount();
  }
}
