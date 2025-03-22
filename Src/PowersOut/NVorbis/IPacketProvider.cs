
// Type: NVorbis.IPacketProvider
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;

#nullable disable
namespace NVorbis
{
  [Obsolete("Moved to NVorbis.Contracts.IPacketProvider", true)]
  public interface IPacketProvider : NVorbis.Contracts.IPacketProvider
  {
    [Obsolete("Moved to per-stream IStreamStats instance on IStreamDecoder.Stats or VorbisReader.Stats.", true)]
    long ContainerBits { get; }

    [Obsolete("No longer supported.", true)]
    int GetTotalPageCount();

    [Obsolete("Getting a packet by index is no longer supported.", true)]
    DataPacket GetPacket(int packetIndex);

    [Obsolete("Moved to long SeekTo(long, int, GetPacketGranuleCount)", true)]
    DataPacket FindPacket(
      long granulePos,
      Func<DataPacket, DataPacket, int> packetGranuleCountCallback);

    [Obsolete("Seeking to a specified packet is no longer supported.  See SeekTo(...) instead.", true)]
    void SeekToPacket(DataPacket packet, int preRoll);

    [Obsolete("No longer supported.", true)]
    event EventHandler ParameterChange;
  }
}
