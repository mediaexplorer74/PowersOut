
// Type: NVorbis.Codebook
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NVorbis
{
  internal class Codebook : ICodebook
  {
    private int[] _lengths;
    private float[] _lookupTable;
    private IReadOnlyList<HuffmanListNode> _overflowList;
    private IReadOnlyList<HuffmanListNode> _prefixList;
    private int _prefixBitLength;
    private int _maxBits;

    public void Init(IPacket packet, IHuffman huffman)
    {
      this.Dimensions = packet.ReadBits(24) == 5653314UL ? (int) packet.ReadBits(16) : throw new InvalidDataException("Book header had invalid signature!");
      this.Entries = (int) packet.ReadBits(24);
      this._lengths = new int[this.Entries];
      this.InitTree(packet, huffman);
      this.InitLookupTable(packet);
    }

    private void InitTree(IPacket packet, IHuffman huffman)
    {
      int num1 = 0;
      bool sparse;
      int num2;
      if (packet.ReadBit())
      {
        int num3 = (int) packet.ReadBits(5) + 1;
        int num4 = 0;
        while (num4 < this.Entries)
        {
          int num5 = (int) packet.ReadBits(Utils.ilog(this.Entries - num4));
          while (--num5 >= 0)
            this._lengths[num4++] = num3;
          ++num3;
        }
        num1 = 0;
        sparse = false;
        num2 = num3;
      }
      else
      {
        num2 = -1;
        sparse = packet.ReadBit();
        for (int index = 0; index < this.Entries; ++index)
        {
          if (!sparse || packet.ReadBit())
          {
            this._lengths[index] = (int) packet.ReadBits(5) + 1;
            ++num1;
          }
          else
            this._lengths[index] = -1;
          if (this._lengths[index] > num2)
            num2 = this._lengths[index];
        }
      }
      if ((this._maxBits = num2) <= -1)
        return;
      int[] codewordLengths = (int[]) null;
      if (sparse && num1 >= this.Entries >> 2)
      {
        codewordLengths = new int[this.Entries];
        Array.Copy((Array) this._lengths, (Array) codewordLengths, this.Entries);
        sparse = false;
      }
      int length = !sparse ? 0 : num1;
      int[] values = (int[]) null;
      int[] numArray = (int[]) null;
      if (!sparse)
        numArray = new int[this.Entries];
      else if (length != 0)
      {
        codewordLengths = new int[length];
        numArray = new int[length];
        values = new int[length];
      }
      if (!this.ComputeCodewords(sparse, numArray, codewordLengths, this._lengths, this.Entries, values))
        throw new InvalidDataException();
      IReadOnlyList<int> intList = (IReadOnlyList<int>) values ?? (IReadOnlyList<int>) Codebook.FastRange.Get(0, numArray.Length);
      huffman.GenerateTable(intList, codewordLengths ?? this._lengths, numArray);
      this._prefixList = huffman.PrefixTree;
      this._prefixBitLength = huffman.TableBits;
      this._overflowList = huffman.OverflowList;
    }

    private bool ComputeCodewords(
      bool sparse,
      int[] codewords,
      int[] codewordLengths,
      int[] len,
      int n,
      int[] values)
    {
      int num1 = 0;
      uint[] numArray = new uint[32];
      int index1 = 0;
      while (index1 < n && len[index1] <= 0)
        ++index1;
      if (index1 == n)
        return true;
      int num2 = sparse ? 1 : 0;
      int[] codewords1 = codewords;
      int[] codewordLengths1 = codewordLengths;
      int symbol1 = index1;
      int count = num1;
      int num3 = count + 1;
      int len1 = len[index1];
      int[] values1 = values;
      this.AddEntry(num2 != 0, codewords1, codewordLengths1, 0U, symbol1, count, len1, values1);
      for (int index2 = 1; index2 <= len[index1]; ++index2)
        numArray[index2] = (uint) (1 << 32 - index2);
      for (int symbol2 = index1 + 1; symbol2 < n; ++symbol2)
      {
        int index3 = len[symbol2];
        if (index3 > 0)
        {
          while (index3 > 0 && numArray[index3] == 0U)
            --index3;
          if (index3 == 0)
            return false;
          uint n1 = numArray[index3];
          numArray[index3] = 0U;
          this.AddEntry(sparse, codewords, codewordLengths, Utils.BitReverse(n1), symbol2, num3++, len[symbol2], values);
          if (index3 != len[symbol2])
          {
            for (int index4 = len[symbol2]; index4 > index3; --index4)
              numArray[index4] = n1 + (uint) (1 << 32 - index4);
          }
        }
      }
      return true;
    }

    private void AddEntry(
      bool sparse,
      int[] codewords,
      int[] codewordLengths,
      uint huffCode,
      int symbol,
      int count,
      int len,
      int[] values)
    {
      if (sparse)
      {
        codewords[count] = (int) huffCode;
        codewordLengths[count] = len;
        values[count] = symbol;
      }
      else
        codewords[symbol] = (int) huffCode;
    }

    private void InitLookupTable(IPacket packet)
    {
      this.MapType = (int) packet.ReadBits(4);
      if (this.MapType == 0)
        return;
      float num1 = Utils.ConvertFromVorbisFloat32((uint) packet.ReadBits(32));
      float num2 = Utils.ConvertFromVorbisFloat32((uint) packet.ReadBits(32));
      int count = (int) packet.ReadBits(4) + 1;
      bool flag = packet.ReadBit();
      int length = this.Entries * this.Dimensions;
      float[] numArray1 = new float[length];
      if (this.MapType == 1)
        length = this.lookup1_values();
      uint[] numArray2 = new uint[length];
      for (int index = 0; index < length; ++index)
        numArray2[index] = (uint) packet.ReadBits(count);
      if (this.MapType == 1)
      {
        for (int index1 = 0; index1 < this.Entries; ++index1)
        {
          double num3 = 0.0;
          int num4 = 1;
          for (int index2 = 0; index2 < this.Dimensions; ++index2)
          {
            int index3 = index1 / num4 % length;
            double num5 = (double) numArray2[index3] * (double) num2 + (double) num1 + num3;
            numArray1[index1 * this.Dimensions + index2] = (float) num5;
            if (flag)
              num3 = num5;
            num4 *= length;
          }
        }
      }
      else
      {
        for (int index4 = 0; index4 < this.Entries; ++index4)
        {
          double num6 = 0.0;
          int index5 = index4 * this.Dimensions;
          for (int index6 = 0; index6 < this.Dimensions; ++index6)
          {
            double num7 = (double) numArray2[index5] * (double) num2 + (double) num1 + num6;
            numArray1[index4 * this.Dimensions + index6] = (float) num7;
            if (flag)
              num6 = num7;
            ++index5;
          }
        }
      }
      this._lookupTable = numArray1;
    }

    private int lookup1_values()
    {
      int num = (int) Math.Floor(Math.Exp(Math.Log((double) this.Entries) / (double) this.Dimensions));
      if (Math.Floor(Math.Pow((double) (num + 1), (double) this.Dimensions)) <= (double) this.Entries)
        ++num;
      return num;
    }

    public int DecodeScalar(IPacket packet)
    {
      int bitsRead;
      int num1 = (int) packet.TryPeekBits(this._prefixBitLength, out bitsRead);
      if (bitsRead == 0)
        return -1;
      HuffmanListNode prefix = this._prefixList[num1];
      if (prefix != null)
      {
        packet.SkipBits(prefix.Length);
        return prefix.Value;
      }
      int num2 = (int) packet.TryPeekBits(this._maxBits, out int _);
      for (int index = 0; index < ((IReadOnlyCollection<HuffmanListNode>) this._overflowList).Count; ++index)
      {
        HuffmanListNode overflow = this._overflowList[index];
        if (overflow.Bits == (num2 & overflow.Mask))
        {
          packet.SkipBits(overflow.Length);
          return overflow.Value;
        }
      }
      return -1;
    }

    public float this[int entry, int dim] => this._lookupTable[entry * this.Dimensions + dim];

    public int Dimensions { get; private set; }

    public int Entries { get; private set; }

    public int MapType { get; private set; }

    private class FastRange : 
      IReadOnlyList<int>,
      IEnumerable<int>,
      IEnumerable,
      IReadOnlyCollection<int>
    {
      [ThreadStatic]
      private static Codebook.FastRange _cachedRange;
      private int _start;
      private int _count;

      internal static Codebook.FastRange Get(int start, int count)
      {
        Codebook.FastRange fastRange = Codebook.FastRange._cachedRange ?? (Codebook.FastRange._cachedRange = new Codebook.FastRange());
        fastRange._start = start;
        fastRange._count = count;
        return fastRange;
      }

      private FastRange()
      {
      }

      public int this[int index]
      {
        get
        {
          if (index > this._count)
            throw new ArgumentOutOfRangeException();
          return this._start + index;
        }
      }

      public int Count => this._count;

      public IEnumerator<int> GetEnumerator() => throw new NotSupportedException();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }
  }
}
