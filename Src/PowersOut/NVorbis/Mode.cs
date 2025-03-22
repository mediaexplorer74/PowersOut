
// Type: NVorbis.Mode
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.IO;

#nullable disable
namespace NVorbis
{
  internal class Mode : IMode
  {
    private const float M_PI2 = 1.57079637f;
    private int _channels;
    private bool _blockFlag;
    private int _block0Size;
    private int _block1Size;
    private IMapping _mapping;

    public void Init(
      IPacket packet,
      int channels,
      int block0Size,
      int block1Size,
      IMapping[] mappings)
    {
      this._channels = channels;
      this._block0Size = block0Size;
      this._block1Size = block1Size;
      this._blockFlag = packet.ReadBit();
      int index = packet.ReadBits(32) == 0UL ? (int) packet.ReadBits(8) : throw new InvalidDataException("Mode header had invalid window or transform type!");
      if (index >= mappings.Length)
        throw new InvalidDataException("Mode header had invalid mapping index!");
      this._mapping = mappings[index];
      if (this._blockFlag)
        this.Windows = new float[4][]
        {
          new float[this._block1Size],
          new float[this._block1Size],
          new float[this._block1Size],
          new float[this._block1Size]
        };
      else
        this.Windows = new float[1][]
        {
          new float[this._block0Size]
        };
      this.CalcWindows();
    }

    private void CalcWindows()
    {
      for (int index1 = 0; index1 < this.Windows.Length; ++index1)
      {
        float[] window = this.Windows[index1];
        int num1 = ((index1 & 1) == 0 ? this._block0Size : this._block1Size) / 2;
        int blockSize = this.BlockSize;
        int num2 = ((index1 & 2) == 0 ? this._block0Size : this._block1Size) / 2;
        int num3 = blockSize / 4 - num1 / 2;
        int num4 = blockSize - blockSize / 4 - num2 / 2;
        for (int index2 = 0; index2 < num1; ++index2)
        {
          float num5 = (float) Math.Sin(((double) index2 + 0.5) / (double) num1 * 1.5707963705062866);
          float num6 = num5 * num5;
          window[num3 + index2] = (float) Math.Sin((double) num6 * 1.5707963705062866);
        }
        for (int index3 = num3 + num1; index3 < num4; ++index3)
          window[index3] = 1f;
        for (int index4 = 0; index4 < num2; ++index4)
        {
          float num7 = (float) Math.Sin(((double) (num2 - index4) - 0.5) / (double) num2 * 1.5707963705062866);
          float num8 = num7 * num7;
          window[num4 + index4] = (float) Math.Sin((double) num8 * 1.5707963705062866);
        }
      }
    }

    private bool GetPacketInfo(
      IPacket packet,
      bool isLastInPage,
      out int blockSize,
      out int windowIndex,
      out int leftOverlapHalfSize,
      out int packetStartIndex,
      out int packetValidLength,
      out int packetTotalLength)
    {
      bool flag1;
      bool flag2;
      if (this._blockFlag)
      {
        blockSize = this._block1Size;
        flag1 = packet.ReadBit();
        flag2 = packet.ReadBit();
      }
      else
      {
        blockSize = this._block0Size;
        flag1 = flag2 = false;
      }
      if (packet.IsShort)
      {
        windowIndex = 0;
        leftOverlapHalfSize = 0;
        packetStartIndex = 0;
        packetValidLength = 0;
        packetTotalLength = 0;
        return false;
      }
      int num = (flag2 ? this._block1Size : this._block0Size) / 4;
      windowIndex = (flag1 ? 1 : 0) + (flag2 ? 2 : 0);
      leftOverlapHalfSize = (flag1 ? this._block1Size : this._block0Size) / 4;
      packetStartIndex = blockSize / 4 - leftOverlapHalfSize;
      packetTotalLength = blockSize / 4 * 3 + num;
      packetValidLength = packetTotalLength - num * 2;
      if (isLastInPage && this._blockFlag && !flag2)
        packetValidLength -= this._block1Size / 4 - this._block0Size / 4;
      return true;
    }

    public bool Decode(
      IPacket packet,
      float[][] buffer,
      out int packetStartindex,
      out int packetValidLength,
      out int packetTotalLength)
    {
      int blockSize;
      int windowIndex;
      if (!this.GetPacketInfo(packet, false, out blockSize, out windowIndex, out int _, out packetStartindex, out packetValidLength, out packetTotalLength))
        return false;
      this._mapping.DecodePacket(packet, blockSize, this._channels, buffer);
      float[] window = this.Windows[windowIndex];
      for (int index1 = 0; index1 < blockSize; ++index1)
      {
        for (int index2 = 0; index2 < this._channels; ++index2)
          buffer[index2][index1] *= window[index1];
      }
      return true;
    }

    public int GetPacketSampleCount(IPacket packet, bool isLastInPage)
    {
      int packetStartIndex;
      int packetValidLength;
      this.GetPacketInfo(packet, isLastInPage, out int _, out int _, out int _, out packetStartIndex, out packetValidLength, out int _);
      return packetValidLength - packetStartIndex;
    }

    public int BlockSize => !this._blockFlag ? this._block0Size : this._block1Size;

    public float[][] Windows { get; private set; }
  }
}
