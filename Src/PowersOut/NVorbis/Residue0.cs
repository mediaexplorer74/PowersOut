
// Type: NVorbis.Residue0
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.IO;

#nullable disable
namespace NVorbis
{
  internal class Residue0 : IResidue
  {
    private int _channels;
    private int _begin;
    private int _end;
    private int _partitionSize;
    private int _classifications;
    private int _maxStages;
    private ICodebook[][] _books;
    private ICodebook _classBook;
    private int[] _cascade;
    private int[][] _decodeMap;

    private static int icount(int v)
    {
      int num = 0;
      for (; v != 0; v >>= 1)
        num += v & 1;
      return num;
    }

    public virtual void Init(IPacket packet, int channels, ICodebook[] codebooks)
    {
      this._begin = (int) packet.ReadBits(24);
      this._end = (int) packet.ReadBits(24);
      this._partitionSize = (int) packet.ReadBits(24) + 1;
      this._classifications = (int) packet.ReadBits(6) + 1;
      this._classBook = codebooks[(int) packet.ReadBits(8)];
      this._cascade = new int[this._classifications];
      int length1 = 0;
      for (int index = 0; index < this._classifications; ++index)
      {
        int num = (int) packet.ReadBits(3);
        this._cascade[index] = !packet.ReadBit() ? num : (int) packet.ReadBits(5) << 3 | num;
        length1 += Residue0.icount(this._cascade[index]);
      }
      int[] numArray = new int[length1];
      for (int index = 0; index < length1; ++index)
      {
        numArray[index] = (int) packet.ReadBits(8);
        if (codebooks[numArray[index]].MapType == 0)
          throw new InvalidDataException();
      }
      int entries = this._classBook.Entries;
      int dimensions = this._classBook.Dimensions;
      int length2 = 1;
      for (; dimensions > 0; --dimensions)
      {
        length2 *= this._classifications;
        if (length2 > entries)
          throw new InvalidDataException();
      }
      this._books = new ICodebook[this._classifications][];
      int num1 = 0;
      int num2 = 0;
      for (int index1 = 0; index1 < this._classifications; ++index1)
      {
        int length3 = Utils.ilog(this._cascade[index1]);
        this._books[index1] = new ICodebook[length3];
        if (length3 > 0)
        {
          num2 = Math.Max(num2, length3);
          for (int index2 = 0; index2 < length3; ++index2)
          {
            if ((this._cascade[index1] & 1 << index2) > 0)
              this._books[index1][index2] = codebooks[numArray[num1++]];
          }
        }
      }
      this._maxStages = num2;
      this._decodeMap = new int[length2][];
      for (int index3 = 0; index3 < length2; ++index3)
      {
        int num3 = index3;
        int num4 = length2 / this._classifications;
        this._decodeMap[index3] = new int[this._classBook.Dimensions];
        for (int index4 = 0; index4 < this._classBook.Dimensions; ++index4)
        {
          int num5 = num3 / num4;
          num3 -= num5 * num4;
          num4 /= this._classifications;
          this._decodeMap[index3][index4] = num5;
        }
      }
      this._channels = channels;
    }

    public virtual void Decode(
      IPacket packet,
      bool[] doNotDecodeChannel,
      int blockSize,
      float[][] buffer)
    {
      int num1 = (this._end < blockSize / 2 ? this._end : blockSize / 2) - this._begin;
      if (num1 <= 0 || Array.IndexOf<bool>(doNotDecodeChannel, false) == -1)
        return;
      int num2 = num1 / this._partitionSize;
      int[,][] numArray = new int[this._channels, (num2 + this._classBook.Dimensions - 1) / this._classBook.Dimensions][];
      for (int index1 = 0; index1 < this._maxStages; ++index1)
      {
        int num3 = 0;
        int index2 = 0;
        while (num3 < num2)
        {
          if (index1 == 0)
          {
            for (int index3 = 0; index3 < this._channels; ++index3)
            {
              int index4 = this._classBook.DecodeScalar(packet);
              if (index4 >= 0 && index4 < this._decodeMap.Length)
              {
                numArray[index3, index2] = this._decodeMap[index4];
              }
              else
              {
                num3 = num2;
                index1 = this._maxStages;
                break;
              }
            }
          }
          for (int index5 = 0; num3 < num2 && index5 < this._classBook.Dimensions; ++num3)
          {
            int offset = this._begin + num3 * this._partitionSize;
            for (int channel = 0; channel < this._channels; ++channel)
            {
              int index6 = numArray[channel, index2][index5];
              if ((this._cascade[index6] & 1 << index1) != 0)
              {
                ICodebook codebook = this._books[index6][index1];
                if (codebook != null && this.WriteVectors(codebook, packet, buffer, channel, offset, this._partitionSize))
                {
                  num3 = num2;
                  index1 = this._maxStages;
                  break;
                }
              }
            }
            ++index5;
          }
          ++index2;
        }
      }
    }

    protected virtual bool WriteVectors(
      ICodebook codebook,
      IPacket packet,
      float[][] residue,
      int channel,
      int offset,
      int partitionSize)
    {
      float[] numArray1 = residue[channel];
      int length = partitionSize / codebook.Dimensions;
      int[] numArray2 = new int[length];
      for (int index = 0; index < length; ++index)
      {
        if ((numArray2[index] = codebook.DecodeScalar(packet)) == -1)
          return true;
      }
      for (int dim = 0; dim < codebook.Dimensions; ++dim)
      {
        int index = 0;
        while (index < length)
        {
          numArray1[offset] += codebook[numArray2[index], dim];
          ++index;
          ++offset;
        }
      }
      return false;
    }
  }
}
