
// Type: NVorbis.Contracts.IPacket
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

#nullable disable
namespace NVorbis.Contracts
{
  public interface IPacket
  {
    bool IsResync { get; }

    bool IsShort { get; }

    long? GranulePosition { get; }

    bool IsEndOfStream { get; }

    int BitsRead { get; }

    int BitsRemaining { get; }

    int ContainerOverheadBits { get; }

    ulong TryPeekBits(int count, out int bitsRead);

    void SkipBits(int count);

    ulong ReadBits(int count);

    void Done();

    void Reset();
  }
}
