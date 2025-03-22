
// Type: NVorbis.Ogg.ForwardOnlyPageReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts.Ogg;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NVorbis.Ogg
{
  internal class ForwardOnlyPageReader : PageReaderBase
  {
    private readonly Dictionary<int, IForwardOnlyPacketProvider> _packetProviders
            = new Dictionary<int, IForwardOnlyPacketProvider>();
    private readonly Func<NVorbis.Contracts.IPacketProvider, bool> _newStreamCallback;

    internal static Func<IPageReader, int, IForwardOnlyPacketProvider> CreatePacketProvider { get; set; } 
            = (Func<IPageReader, int, IForwardOnlyPacketProvider>) ((pr, ss) => (IForwardOnlyPacketProvider) new ForwardOnlyPacketProvider(pr, ss));

    public ForwardOnlyPageReader(
      Stream stream,
      bool closeOnDispose,
      Func<NVorbis.Contracts.IPacketProvider, bool> newStreamCallback)
      : base(stream, closeOnDispose)
    {
      this._newStreamCallback = newStreamCallback;
    }

    protected override bool AddPage(int streamSerial, byte[] pageBuf, bool isResync)
    {
      IForwardOnlyPacketProvider onlyPacketProvider1;
      if (this._packetProviders.TryGetValue(streamSerial, out onlyPacketProvider1))
      {
        if (onlyPacketProvider1.AddPage(pageBuf, isResync) && ((int) pageBuf[5] & 4) == 0)
          return true;
        this._packetProviders.Remove(streamSerial);
      }
      IForwardOnlyPacketProvider onlyPacketProvider2 
                = ForwardOnlyPageReader.CreatePacketProvider((IPageReader) this, streamSerial);
      if (onlyPacketProvider2.AddPage(pageBuf, isResync))
      {
        this._packetProviders.Add(streamSerial, onlyPacketProvider2);
        if (this._newStreamCallback((NVorbis.Contracts.IPacketProvider) onlyPacketProvider2))
          return true;
        this._packetProviders.Remove(streamSerial);
      }
      return false;
    }

    protected override void SetEndOfStreams()
    {
      foreach (KeyValuePair<int, IForwardOnlyPacketProvider> packetProvider in this._packetProviders)
        packetProvider.Value.SetEndOfStream();
      this._packetProviders.Clear();
    }

    public override bool ReadPageAt(long offset) => throw new NotSupportedException();
  }
}
