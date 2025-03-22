
// Type: NVorbis.Ogg.ContainerReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using NVorbis.Contracts.Ogg;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NVorbis.Ogg
{
  public sealed class ContainerReader : NVorbis.Contracts.IContainerReader, IDisposable
  {
    private IPageReader _reader;
    private List<WeakReference<NVorbis.Contracts.IPacketProvider>> _packetProviders;
    private bool _foundStream;

    internal static Func<Stream, bool, Func<NVorbis.Contracts.IPacketProvider, bool>, IPageReader> CreatePageReader { get; set; } = (Func<Stream, bool, Func<NVorbis.Contracts.IPacketProvider, bool>, IPageReader>) ((s, cod, cb) => (IPageReader) new PageReader(s, cod, cb));

    internal static Func<Stream, bool, Func<NVorbis.Contracts.IPacketProvider, bool>, IPageReader> CreateForwardOnlyPageReader { get; set; } = (Func<Stream, bool, Func<NVorbis.Contracts.IPacketProvider, bool>, IPageReader>) ((s, cod, cb) => (IPageReader) new ForwardOnlyPageReader(s, cod, cb));

    public NewStreamHandler NewStreamCallback { get; set; }

    public IReadOnlyList<NVorbis.Contracts.IPacketProvider> GetStreams()
    {
      List<NVorbis.Contracts.IPacketProvider> streams = new List<NVorbis.Contracts.IPacketProvider>(this._packetProviders.Count);
      for (int index = 0; index < this._packetProviders.Count; ++index)
      {
        NVorbis.Contracts.IPacketProvider packetProvider;
        if (this._packetProviders[index].TryGetTarget(out packetProvider))
        {
          streams.Add(packetProvider);
        }
        else
        {
          streams.RemoveAt(index);
          --index;
        }
      }
      return (IReadOnlyList<NVorbis.Contracts.IPacketProvider>) streams;
    }

    public bool CanSeek { get; }

    public long WasteBits => this._reader.WasteBits;

    public long ContainerBits => this._reader.ContainerBits;

    public ContainerReader(Stream stream, bool closeOnDispose)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      this._packetProviders = new List<WeakReference<NVorbis.Contracts.IPacketProvider>>();
      if (stream.CanSeek)
      {
        this._reader = ContainerReader.CreatePageReader(stream, closeOnDispose, new Func<NVorbis.Contracts.IPacketProvider, bool>(this.ProcessNewStream));
        this.CanSeek = true;
      }
      else
        this._reader = ContainerReader.CreateForwardOnlyPageReader(stream, closeOnDispose, new Func<NVorbis.Contracts.IPacketProvider, bool>(this.ProcessNewStream));
    }

    public bool TryInit() => this.FindNextStream();

    public bool FindNextStream()
    {
      this._reader.Lock();
      try
      {
        this._foundStream = false;
        while (this._reader.ReadNextPage())
        {
          if (this._foundStream)
            return true;
        }
        return false;
      }
      finally
      {
        this._reader.Release();
      }
    }

    private bool ProcessNewStream(NVorbis.Contracts.IPacketProvider packetProvider)
    {
      bool flag = this._reader.Release();
      try
      {
        NewStreamHandler newStreamCallback = this.NewStreamCallback;
        if ((newStreamCallback != null ? (newStreamCallback(packetProvider) ? 1 : 0) : 1) == 0)
          return false;
        this._packetProviders.Add(new WeakReference<NVorbis.Contracts.IPacketProvider>(packetProvider));
        this._foundStream = true;
        return true;
      }
      finally
      {
        if (flag)
          this._reader.Lock();
      }
    }

    public void Dispose()
    {
      this._reader?.Dispose();
      this._reader = (IPageReader) null;
    }
  }
}
