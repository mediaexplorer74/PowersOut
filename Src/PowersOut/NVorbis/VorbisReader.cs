
// Type: NVorbis.VorbisReader
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using NVorbis.Ogg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace NVorbis
{
  public sealed class VorbisReader : IVorbisReader, IDisposable
  {
    private readonly List<IStreamDecoder> _decoders;
    private readonly NVorbis.Contracts.IContainerReader _containerReader;
    private readonly bool _closeOnDispose;
    private IStreamDecoder _streamDecoder;

    internal static Func<Stream, bool, NVorbis.Contracts.IContainerReader> CreateContainerReader { get; set; } = (Func<Stream, bool, NVorbis.Contracts.IContainerReader>) ((s, cod) => (NVorbis.Contracts.IContainerReader) new ContainerReader(s, cod));

    internal static Func<NVorbis.Contracts.IPacketProvider, IStreamDecoder> CreateStreamDecoder { get; set; } = (Func<NVorbis.Contracts.IPacketProvider, IStreamDecoder>) (pp => (IStreamDecoder) new StreamDecoder(pp, (IFactory) new Factory()));

    public event EventHandler<NewStreamEventArgs> NewStream;

    public VorbisReader(string fileName)
      : this((Stream) File.OpenRead(fileName))
    {
    }

    public VorbisReader(Stream stream, bool closeOnDispose = true)
    {
      this._decoders = new List<IStreamDecoder>();
      NVorbis.Contracts.IContainerReader containerReader = VorbisReader.CreateContainerReader(stream, closeOnDispose);
      containerReader.NewStreamCallback = new NewStreamHandler(this.ProcessNewStream);
      if (!containerReader.TryInit() || this._decoders.Count == 0)
      {
        containerReader.NewStreamCallback = (NewStreamHandler) null;
        containerReader.Dispose();
        if (closeOnDispose)
          stream.Dispose();
        throw new ArgumentException("Could not load the specified container!", "containerReader");
      }
      this._closeOnDispose = closeOnDispose;
      this._containerReader = containerReader;
      this._streamDecoder = this._decoders[0];
    }

    [Obsolete("Use \"new StreamDecoder(Contracts.IPacketProvider)\" and the container's NewStreamCallback or Streams property instead.", true)]
    public VorbisReader(NVorbis.Contracts.IContainerReader containerReader)
    {
      throw new NotSupportedException();
    }

    [Obsolete("Use \"new StreamDecoder(Contracts.IPacketProvider)\" instead.", true)]
    public VorbisReader(NVorbis.Contracts.IPacketProvider packetProvider)
    {
      throw new NotSupportedException();
    }

    private bool ProcessNewStream(NVorbis.Contracts.IPacketProvider packetProvider)
    {
      IStreamDecoder streamDecoder = VorbisReader.CreateStreamDecoder(packetProvider);
      streamDecoder.ClipSamples = true;
      NewStreamEventArgs newStreamEventArgs = new NewStreamEventArgs(streamDecoder);
      EventHandler<NewStreamEventArgs> newStream = this.NewStream;
      if (newStream != null)
        newStream((object) this, newStreamEventArgs);
      if (newStreamEventArgs.IgnoreStream)
        return false;
      this._decoders.Add(streamDecoder);
      return true;
    }

    public void Dispose()
    {
      if (this._decoders != null)
      {
        foreach (IDisposable decoder in this._decoders)
          decoder.Dispose();
        this._decoders.Clear();
      }
      if (this._containerReader == null)
        return;
      this._containerReader.NewStreamCallback = (NewStreamHandler) null;
      if (!this._closeOnDispose)
        return;
      this._containerReader.Dispose();
    }

    public IReadOnlyList<IStreamDecoder> Streams => (IReadOnlyList<IStreamDecoder>) this._decoders;

    public int Channels => this._streamDecoder.Channels;

    public int SampleRate => this._streamDecoder.SampleRate;

    public int UpperBitrate => this._streamDecoder.UpperBitrate;

    public int NominalBitrate => this._streamDecoder.NominalBitrate;

    public int LowerBitrate => this._streamDecoder.LowerBitrate;

    public ITagData Tags => this._streamDecoder.Tags;

    [Obsolete("Use .Tags.EncoderVendor instead.")]
    public string Vendor => this._streamDecoder.Tags.EncoderVendor;

    [Obsolete("Use .Tags.All instead.")]
    public string[] Comments
    {
      get
      {
        return Enumerable.ToArray<string>(Enumerable.SelectMany<KeyValuePair<string, IReadOnlyList<string>>, string, string>((IEnumerable<KeyValuePair<string, IReadOnlyList<string>>>) this._streamDecoder.Tags.All, (Func<KeyValuePair<string, IReadOnlyList<string>>, IEnumerable<string>>) (k => (IEnumerable<string>) k.Value), (Func<KeyValuePair<string, IReadOnlyList<string>>, string, string>) ((kvp, Item) => kvp.Key + "=" + Item)));
      }
    }

    [Obsolete("No longer supported.  Will receive a new stream when parameters change.", true)]
    public bool IsParameterChange => throw new NotSupportedException();

    public long ContainerOverheadBits
    {
      get
      {
        NVorbis.Contracts.IContainerReader containerReader = this._containerReader;
        return containerReader == null ? 0L : containerReader.ContainerBits;
      }
    }

    public long ContainerWasteBits
    {
      get
      {
        NVorbis.Contracts.IContainerReader containerReader = this._containerReader;
        return containerReader == null ? 0L : containerReader.WasteBits;
      }
    }

    public int StreamIndex => this._decoders.IndexOf(this._streamDecoder);

    [Obsolete("Use .Streams.Count instead.")]
    public int StreamCount => this._decoders.Count;

    [Obsolete("Use VorbisReader.TimePosition instead.")]
    public TimeSpan DecodedTime
    {
      get => this._streamDecoder.TimePosition;
      set => this.TimePosition = value;
    }

    [Obsolete("Use VorbisReader.SamplePosition instead.")]
    public long DecodedPosition
    {
      get => this._streamDecoder.SamplePosition;
      set => this.SamplePosition = value;
    }

    public TimeSpan TotalTime => this._streamDecoder.TotalTime;

    public long TotalSamples => this._streamDecoder.TotalSamples;

    public TimeSpan TimePosition
    {
      get => this._streamDecoder.TimePosition;
      set => this._streamDecoder.TimePosition = value;
    }

    public long SamplePosition
    {
      get => this._streamDecoder.SamplePosition;
      set => this._streamDecoder.SamplePosition = value;
    }

    public bool IsEndOfStream => this._streamDecoder.IsEndOfStream;

    public bool ClipSamples
    {
      get => this._streamDecoder.ClipSamples;
      set => this._streamDecoder.ClipSamples = value;
    }

    public bool HasClipped => this._streamDecoder.HasClipped;

    public IStreamStats StreamStats => this._streamDecoder.Stats;

    [Obsolete("Use Streams[*].Stats instead.", true)]
    public IVorbisStreamStatus[] Stats => throw new NotSupportedException();

    public bool FindNextStream()
    {
      return this._containerReader != null && this._containerReader.FindNextStream();
    }

    public bool SwitchStreams(int index)
    {
      if (index < 0 || index >= this._decoders.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      IStreamDecoder decoder = this._decoders[index];
      IStreamDecoder streamDecoder = this._streamDecoder;
      if (decoder == streamDecoder)
        return false;
      decoder.ClipSamples = streamDecoder.ClipSamples;
      this._streamDecoder = decoder;
      return decoder.Channels != streamDecoder.Channels || decoder.SampleRate != streamDecoder.SampleRate;
    }

    public void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = 0)
    {
      this._streamDecoder.SeekTo(timePosition, seekOrigin);
    }

    public void SeekTo(long samplePosition, SeekOrigin seekOrigin = 0)
    {
      this._streamDecoder.SeekTo(samplePosition, seekOrigin);
    }

    public int ReadSamples(float[] buffer, int offset, int count)
    {
      count -= count % this._streamDecoder.Channels;
      return count > 0 ? this._streamDecoder.Read(buffer, offset, count) : 0;
    }

    public int ReadSamples(Span<float> buffer)
    {
      int count = buffer.Length - buffer.Length % this._streamDecoder.Channels;
      return !buffer.IsEmpty ? this._streamDecoder.Read(buffer, 0, count) : 0;
    }

    //[Obsolete("No longer needed.", true)]
    //public void ClearParameterChange() => throw new NotSupportedException();
  }
}
