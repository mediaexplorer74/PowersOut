
// Type: NVorbis.StreamDecoder
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace NVorbis
{
  public sealed class StreamDecoder : IStreamDecoder, IDisposable
  {
    private NVorbis.Contracts.IPacketProvider _packetProvider;
    private IFactory _factory;
    private StreamStats _stats;
    private byte _channels;
    private int _sampleRate;
    private int _block0Size;
    private int _block1Size;
    private IMode[] _modes;
    private int _modeFieldBits;
    private string _vendor;
    private string[] _comments;
    private ITagData _tags;
    private long _currentPosition;
    private bool _hasClipped;
    private bool _hasPosition;
    private bool _eosFound;
    private float[][] _nextPacketBuf;
    private float[][] _prevPacketBuf;
    private int _prevPacketStart;
    private int _prevPacketEnd;
    private int _prevPacketStop;
    private static readonly byte[] PacketSignatureStream = new byte[11]
    {
      (byte) 1,
      (byte) 118,
      (byte) 111,
      (byte) 114,
      (byte) 98,
      (byte) 105,
      (byte) 115,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private static readonly byte[] PacketSignatureComments = new byte[7]
    {
      (byte) 3,
      (byte) 118,
      (byte) 111,
      (byte) 114,
      (byte) 98,
      (byte) 105,
      (byte) 115
    };
    private static readonly byte[] PacketSignatureBooks = new byte[7]
    {
      (byte) 5,
      (byte) 118,
      (byte) 111,
      (byte) 114,
      (byte) 98,
      (byte) 105,
      (byte) 115
    };

    internal static Func<IFactory> CreateFactory { get; set; } = (Func<IFactory>) (() => (IFactory) new Factory());

    public StreamDecoder(NVorbis.Contracts.IPacketProvider packetProvider)
      : this(packetProvider, (IFactory) new Factory())
    {
    }

    internal StreamDecoder(NVorbis.Contracts.IPacketProvider packetProvider, IFactory factory)
    {
      this._packetProvider = packetProvider ?? throw new ArgumentNullException(nameof (packetProvider));
      this._factory = factory ?? throw new ArgumentNullException(nameof (factory));
      this._stats = new StreamStats();
      this._currentPosition = 0L;
      this.ClipSamples = true;
      IPacket packet = this._packetProvider.PeekNextPacket();
      if (!this.ProcessHeaderPackets(packet))
      {
        this._packetProvider = (NVorbis.Contracts.IPacketProvider) null;
        packet.Reset();
        throw StreamDecoder.GetInvalidStreamException(packet);
      }
    }

    private static Exception GetInvalidStreamException(IPacket packet)
    {
      try
      {
        ulong num = packet.ReadBits(64);
        if (num == 7233173838382854223UL)
          return (Exception) new ArgumentException("Found OPUS bitstream.");
        if (((long) num & (long) byte.MaxValue) == (long) sbyte.MaxValue)
          return (Exception) new ArgumentException("Found FLAC bitstream.");
        if (num == 2314885909937746003UL)
          return (Exception) new ArgumentException("Found Speex bitstream.");
        if (num == 28254585843050854UL)
          return (Exception) new ArgumentException("Found Skeleton metadata bitstream.");
        return ((long) num & 72057594037927680L) == 27428895509214208L ? (Exception) new ArgumentException("Found Theora bitsream.") : (Exception) new ArgumentException("Could not find Vorbis data to decode.");
      }
      finally
      {
        packet.Reset();
      }
    }

    private bool ProcessHeaderPackets(IPacket packet)
    {
      if (!StreamDecoder.ProcessHeaderPacket(packet, new Func<IPacket, bool>(this.LoadStreamHeader), (Action<IPacket>) (_ => this._packetProvider.GetNextPacket().Done())) || !StreamDecoder.ProcessHeaderPacket(this._packetProvider.GetNextPacket(), new Func<IPacket, bool>(this.LoadComments), (Action<IPacket>) (pkt => pkt.Done())) || !StreamDecoder.ProcessHeaderPacket(this._packetProvider.GetNextPacket(), new Func<IPacket, bool>(this.LoadBooks), (Action<IPacket>) (pkt => pkt.Done())))
        return false;
      this._currentPosition = 0L;
      this.ResetDecoder();
      return true;
    }

    private static bool ProcessHeaderPacket(
      IPacket packet,
      Func<IPacket, bool> processAction,
      Action<IPacket> doneAction)
    {
      if (packet == null)
        return false;
      try
      {
        return processAction(packet);
      }
      finally
      {
        doneAction(packet);
      }
    }

    private static bool ValidateHeader(IPacket packet, byte[] expected)
    {
      for (int index = 0; index < expected.Length; ++index)
      {
        if ((long) expected[index] != (long) packet.ReadBits(8))
          return false;
      }
      return true;
    }

    private static string ReadString(IPacket packet)
    {
      int count = (int) packet.ReadBits(32);
      if (count == 0)
        return string.Empty;
      byte[] buffer = new byte[count];
      if (packet.Read(buffer, 0, count) < count)
        throw new InvalidDataException("Could not read full string!");
      return Encoding.UTF8.GetString(buffer);
    }

    private bool LoadStreamHeader(IPacket packet)
    {
      if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureStream))
        return false;
      this._channels = (byte) packet.ReadBits(8);
      this._sampleRate = (int) packet.ReadBits(32);
      this.UpperBitrate = (int) packet.ReadBits(32);
      this.NominalBitrate = (int) packet.ReadBits(32);
      this.LowerBitrate = (int) packet.ReadBits(32);
      this._block0Size = 1 << (int) packet.ReadBits(4);
      this._block1Size = 1 << (int) packet.ReadBits(4);
      if (this.NominalBitrate == 0 && this.UpperBitrate > 0 && this.LowerBitrate > 0)
        this.NominalBitrate = (this.UpperBitrate + this.LowerBitrate) / 2;
      this._stats.SetSampleRate(this._sampleRate);
      this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
      return true;
    }

    private bool LoadComments(IPacket packet)
    {
      if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureComments))
        return false;
      this._vendor = StreamDecoder.ReadString(packet);
      this._comments = new string[packet.ReadBits(32)];
      for (int index = 0; index < this._comments.Length; ++index)
        this._comments[index] = StreamDecoder.ReadString(packet);
      this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
      return true;
    }

    private bool LoadBooks(IPacket packet)
    {
      if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureBooks))
        return false;
      IMdct mdct = this._factory.CreateMdct();
      IHuffman huffman = this._factory.CreateHuffman();
      ICodebook[] codebooks = new ICodebook[checked ((ulong) unchecked ((long) packet.ReadBits(8) + 1L))];
      for (int index = 0; index < codebooks.Length; ++index)
      {
        codebooks[index] = this._factory.CreateCodebook();
        codebooks[index].Init(packet, huffman);
      }
      int num = (int) packet.ReadBits(6) + 1;
      packet.SkipBits(16 * num);
      IFloor[] floors = new IFloor[checked ((ulong) unchecked ((long) packet.ReadBits(6) + 1L))];
      for (int index = 0; index < floors.Length; ++index)
      {
        floors[index] = this._factory.CreateFloor(packet);
        floors[index].Init(packet, (int) this._channels, this._block0Size, this._block1Size, codebooks);
      }
      IResidue[] residues = new IResidue[checked ((ulong) unchecked ((long) packet.ReadBits(6) + 1L))];
      for (int index = 0; index < residues.Length; ++index)
      {
        residues[index] = this._factory.CreateResidue(packet);
        residues[index].Init(packet, (int) this._channels, codebooks);
      }
      IMapping[] mappings = new IMapping[checked ((ulong) unchecked ((long) packet.ReadBits(6) + 1L))];
      for (int index = 0; index < mappings.Length; ++index)
      {
        mappings[index] = this._factory.CreateMapping(packet);
        mappings[index].Init(packet, (int) this._channels, floors, residues, mdct);
      }
      this._modes = new IMode[checked ((ulong) unchecked ((long) packet.ReadBits(6) + 1L))];
      for (int index = 0; index < this._modes.Length; ++index)
      {
        this._modes[index] = this._factory.CreateMode();
        this._modes[index].Init(packet, (int) this._channels, this._block0Size, this._block1Size, mappings);
      }
      if (!packet.ReadBit())
        throw new InvalidDataException("Book packet did not end on correct bit!");
      this._modeFieldBits = Utils.ilog(this._modes.Length - 1);
      this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
      return true;
    }

    private void ResetDecoder()
    {
      this._prevPacketBuf = (float[][]) null;
      this._prevPacketStart = 0;
      this._prevPacketEnd = 0;
      this._prevPacketStop = 0;
      this._nextPacketBuf = (float[][]) null;
      this._eosFound = false;
      this._hasClipped = false;
      this._hasPosition = false;
    }

    public int Read(Span<float> buffer, int offset, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));

      if (offset < 0 || offset + count > buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (offset));

      if (count % (int) this._channels != 0)
        throw new ArgumentOutOfRangeException(nameof (count), "Must be a multiple of Channels!");

      if (this._packetProvider == null)
        throw new ObjectDisposedException(nameof (StreamDecoder));
      if (count == 0)
        return 0;
      int targetIndex = offset;
      int num = offset + count;
      while (targetIndex < num)
      {
        if (this._prevPacketStart == this._prevPacketEnd)
        {
          if (this._eosFound)
          {
            this._nextPacketBuf = (float[][]) null;
            this._prevPacketBuf = (float[][]) null;
            break;
          }
          long? samplePosition;
          if (!this.ReadNextPacket((targetIndex - offset) / (int) this._channels, out samplePosition))
            this._prevPacketEnd = this._prevPacketStop;
          if (samplePosition.HasValue && !this._hasPosition)
          {
            this._hasPosition = true;
            this._currentPosition = samplePosition.Value - (long) (this._prevPacketEnd - this._prevPacketStart) - (long) ((targetIndex - offset) / (int) this._channels);
          }
        }
        int count1 = Math.Min((num - targetIndex) / (int) this._channels, this._prevPacketEnd - this._prevPacketStart);
        if (count1 > 0)
        {
          if (this.ClipSamples)
            targetIndex += this.ClippingCopyBuffer(buffer, targetIndex, count1);
          else
            targetIndex += this.CopyBuffer(buffer, targetIndex, count1);
        }
      }
      count = targetIndex - offset;
      this._currentPosition += (long) (count / (int) this._channels);
      return count;
    }

    private int ClippingCopyBuffer(Span<float> target, int targetIndex, int count)
    {
      int num = targetIndex;
      for (; count > 0; --count)
      {
        for (int index = 0; index < (int) this._channels; ++index)
          target[num++] = Utils.ClipValue(this._prevPacketBuf[index][this._prevPacketStart], ref this._hasClipped);
        ++this._prevPacketStart;
      }
      return num - targetIndex;
    }

    private int CopyBuffer(Span<float> target, int targetIndex, int count)
    {
      int num = targetIndex;
      for (; count > 0; --count)
      {
        for (int index = 0; index < (int) this._channels; ++index)
          target[num++] = this._prevPacketBuf[index][this._prevPacketStart];
        ++this._prevPacketStart;
      }
      return num - targetIndex;
    }

    private bool ReadNextPacket(int bufferedSamples, out long? samplePosition)
    {
      int packetStartindex;
      int packetValidLength;
      int packetTotalLength;
      bool isEndOfStream;
      int bitsRead;
      int bitsRemaining;
      int containerOverheadBits;
      float[][] next = this.DecodeNextPacket(out packetStartindex, out packetValidLength, out packetTotalLength, out isEndOfStream, out samplePosition, out bitsRead, out bitsRemaining, out containerOverheadBits);
      this._eosFound |= isEndOfStream;
      if (next == null)
      {
        this._stats.AddPacket(0, bitsRead, bitsRemaining, containerOverheadBits);
        return false;
      }
      if (samplePosition.HasValue & isEndOfStream)
      {
        long num1 = this._currentPosition + (long) bufferedSamples + (long) packetValidLength - (long) packetStartindex;
        int num2 = (int) (samplePosition.Value - num1);
        if (num2 < 0)
          packetValidLength += num2;
      }
      if (this._prevPacketEnd > 0)
      {
        StreamDecoder.OverlapBuffers(this._prevPacketBuf, next, this._prevPacketStart, this._prevPacketStop, packetStartindex, (int) this._channels);
        this._prevPacketStart = packetStartindex;
      }
      else if (this._prevPacketBuf == null)
        this._prevPacketStart = packetValidLength;
      this._stats.AddPacket(packetValidLength - this._prevPacketStart, bitsRead, bitsRemaining, containerOverheadBits);
      this._nextPacketBuf = this._prevPacketBuf;
      this._prevPacketEnd = packetValidLength;
      this._prevPacketStop = packetTotalLength;
      this._prevPacketBuf = next;
      return true;
    }

    private float[][] DecodeNextPacket(
      out int packetStartindex,
      out int packetValidLength,
      out int packetTotalLength,
      out bool isEndOfStream,
      out long? samplePosition,
      out int bitsRead,
      out int bitsRemaining,
      out int containerOverheadBits)
    {
      IPacket packet = (IPacket) null;
      try
      {
        if ((packet = this._packetProvider.GetNextPacket()) == null)
        {
          isEndOfStream = true;
        }
        else
        {
          isEndOfStream = packet.IsEndOfStream;
          if (packet.IsResync)
            this._hasPosition = false;
          containerOverheadBits = packet.ContainerOverheadBits;
          if (packet.ReadBit())
          {
            bitsRemaining = packet.BitsRemaining + 1;
          }
          else
          {
            IMode mode = this._modes[(int) packet.ReadBits(this._modeFieldBits)];
            if (this._nextPacketBuf == null)
            {
              this._nextPacketBuf = new float[(int) this._channels][];
              for (int index = 0; index < (int) this._channels; ++index)
                this._nextPacketBuf[index] = new float[this._block1Size];
            }
            if (mode.Decode(packet, this._nextPacketBuf, out packetStartindex, out packetValidLength, out packetTotalLength))
            {
              samplePosition = packet.GranulePosition;
              bitsRead = packet.BitsRead;
              bitsRemaining = packet.BitsRemaining;
              return this._nextPacketBuf;
            }
            bitsRemaining = packet.BitsRead + packet.BitsRemaining;
          }
        }
        packetStartindex = 0;
        packetValidLength = 0;
        packetTotalLength = 0;
        samplePosition = new long?();
        bitsRead = 0;
        bitsRemaining = 0;
        containerOverheadBits = 0;
        return (float[][]) null;
      }
      finally
      {
        packet?.Done();
      }
    }

    private static void OverlapBuffers(
      float[][] previous,
      float[][] next,
      int prevStart,
      int prevLen,
      int nextStart,
      int channels)
    {
      while (prevStart < prevLen)
      {
        for (int index = 0; index < channels; ++index)
          next[index][nextStart] += previous[index][prevStart];
        ++prevStart;
        ++nextStart;
      }
    }

    public void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = 0)
    {
      this.SeekTo((long) ((double) this.SampleRate * timePosition.TotalSeconds), seekOrigin);
    }

    public void SeekTo(long samplePosition, SeekOrigin seekOrigin = 0)
    {
      if (this._packetProvider == null)
        throw new ObjectDisposedException(nameof (StreamDecoder));
      if (!this._packetProvider.CanSeek)
        throw new InvalidOperationException("Seek is not supported by the Contracts.IPacketProvider instance.");
      switch ((int) seekOrigin)
      {
        case 0:
          if (samplePosition < 0L)
            throw new ArgumentOutOfRangeException(nameof (samplePosition));
          int num1;
          if (samplePosition == 0L)
          {
            this._packetProvider.SeekTo(0L, 0, new GetPacketGranuleCount(this.GetPacketGranules));
            num1 = 0;
          }
          else
          {
            long num2 = this._packetProvider.SeekTo(samplePosition, 1, new GetPacketGranuleCount(this.GetPacketGranules));
            num1 = (int) (samplePosition - num2);
          }
          this.ResetDecoder();
          this._hasPosition = true;
          long? samplePosition1;
          if (!this.ReadNextPacket(0, out samplePosition1))
          {
            this._eosFound = true;
            if (this._packetProvider.GetGranuleCount() != samplePosition)
              throw new InvalidOperationException("Could not read pre-roll packet!  Try seeking again prior to reading more samples.");
            this._prevPacketStart = this._prevPacketStop;
            this._currentPosition = samplePosition;
            break;
          }
          if (!this.ReadNextPacket(0, out samplePosition1))
          {
            this.ResetDecoder();
            this._eosFound = true;
            throw new InvalidOperationException("Could not read pre-roll packet!  Try seeking again prior to reading more samples.");
          }
          this._prevPacketStart += num1;
          this._currentPosition = samplePosition;
          break;
        case 1:
          samplePosition = this.SamplePosition - samplePosition;
          goto case 0;
        case 2:
          samplePosition = this.TotalSamples - samplePosition;
          goto case 0;
        default:
          throw new ArgumentOutOfRangeException(nameof (seekOrigin));
      }
    }

    private int GetPacketGranules(IPacket curPacket, bool isLastInPage)
    {
      if (curPacket.IsResync || curPacket.ReadBit())
        return 0;
      int index = (int) curPacket.ReadBits(this._modeFieldBits);
      return index < 0 || index >= this._modes.Length ? 0 : this._modes[index].GetPacketSampleCount(curPacket, isLastInPage);
    }

    public void Dispose()
    {
      if (this._packetProvider is IDisposable packetProvider)
        packetProvider.Dispose();
      this._packetProvider = (NVorbis.Contracts.IPacketProvider) null;
    }

    public int Channels => (int) this._channels;

    public int SampleRate => this._sampleRate;

    public int UpperBitrate { get; private set; }

    public int NominalBitrate { get; private set; }

    public int LowerBitrate { get; private set; }

    public ITagData Tags
    {
      get => this._tags ?? (this._tags = (ITagData) new TagData(this._vendor, this._comments));
    }

    public TimeSpan TotalTime
    {
      get => TimeSpan.FromSeconds((double) this.TotalSamples / (double) this._sampleRate);
    }

    public long TotalSamples
    {
      get
      {
        return (this._packetProvider ?? throw new ObjectDisposedException(nameof (StreamDecoder))).GetGranuleCount();
      }
    }

    public TimeSpan TimePosition
    {
      get => TimeSpan.FromSeconds((double) this._currentPosition / (double) this._sampleRate);
      set => this.SeekTo(value, (SeekOrigin) 0);
    }

    public long SamplePosition
    {
      get => this._currentPosition;
      set => this.SeekTo(value, (SeekOrigin) 0);
    }

    public bool ClipSamples { get; set; }

    public bool HasClipped => this._hasClipped;

    public bool IsEndOfStream => this._eosFound && this._prevPacketBuf == null;

    public IStreamStats Stats => (IStreamStats) this._stats;
  }
}
