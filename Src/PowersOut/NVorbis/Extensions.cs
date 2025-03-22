
// Type: NVorbis.Extensions
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;

#nullable disable
namespace NVorbis
{
  public static class Extensions
  {
    public static int Read(this IPacket packet, byte[] buffer, int index, int count)
    {
      if (index < 0 || index >= buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0 || index + count > buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (count));
      for (int index1 = 0; index1 < count; ++index1)
      {
        int bitsRead;
        byte num = (byte) packet.TryPeekBits(8, out bitsRead);
        if (bitsRead == 0)
          return index1;
        buffer[index++] = num;
        packet.SkipBits(8);
      }
      return count;
    }

    public static byte[] ReadBytes(this IPacket packet, int count)
    {
      byte[] buffer = new byte[count];
      int length = packet.Read(buffer, 0, count);
      if (length >= count)
        return buffer;
      byte[] numArray = new byte[length];
      Buffer.BlockCopy((Array) buffer, 0, (Array) numArray, 0, length);
      return numArray;
    }

    public static bool ReadBit(this IPacket packet) => packet.ReadBits(1) == 1UL;

    public static byte PeekByte(this IPacket packet) => (byte) packet.TryPeekBits(8, out int _);

    public static byte ReadByte(this IPacket packet) => (byte) packet.ReadBits(8);

    public static short ReadInt16(this IPacket packet) => (short) packet.ReadBits(16);

    public static int ReadInt32(this IPacket packet) => (int) packet.ReadBits(32);

    public static long ReadInt64(this IPacket packet) => (long) packet.ReadBits(64);

    public static ushort ReadUInt16(this IPacket packet) => (ushort) packet.ReadBits(16);

    public static uint ReadUInt32(this IPacket packet) => (uint) packet.ReadBits(32);

    public static ulong ReadUInt64(this IPacket packet) => packet.ReadBits(64);

    public static void SkipBytes(this IPacket packet, int count) => packet.SkipBits(count * 8);
  }
}
