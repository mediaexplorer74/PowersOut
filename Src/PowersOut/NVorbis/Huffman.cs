
// Type: NVorbis.Huffman
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.Collections.Generic;

#nullable disable
namespace NVorbis
{
  internal class Huffman : IHuffman, IComparer<HuffmanListNode>
  {
    private const int MAX_TABLE_BITS = 10;

    public int TableBits { get; private set; }

    public IReadOnlyList<HuffmanListNode> PrefixTree { get; private set; }

    public IReadOnlyList<HuffmanListNode> OverflowList { get; private set; }

    public void GenerateTable(IReadOnlyList<int> values, int[] lengthList, int[] codeList)
    {
      HuffmanListNode[] huffmanListNodeArray = new HuffmanListNode[lengthList.Length];
      int num1 = 0;
      for (int index = 0; index < huffmanListNodeArray.Length; ++index)
      {
        huffmanListNodeArray[index] = new HuffmanListNode()
        {
          Value = values[index],
          Length = lengthList[index] <= 0 ? 99999 : lengthList[index],
          Bits = codeList[index],
          Mask = (1 << lengthList[index]) - 1
        };
        if (lengthList[index] > 0 && num1 < lengthList[index])
          num1 = lengthList[index];
      }
      Array.Sort<HuffmanListNode>(huffmanListNodeArray, 0, huffmanListNodeArray.Length, (IComparer<HuffmanListNode>) this);
      int num2 = num1 > 10 ? 10 : num1;
      List<HuffmanListNode> huffmanListNodeList1 = new List<HuffmanListNode>(1 << num2);
      List<HuffmanListNode> huffmanListNodeList2 = (List<HuffmanListNode>) null;
      for (int index1 = 0; index1 < huffmanListNodeArray.Length && huffmanListNodeArray[index1].Length < 99999; ++index1)
      {
        int length = huffmanListNodeArray[index1].Length;
        if (length > num2)
        {
          huffmanListNodeList2 = new List<HuffmanListNode>(huffmanListNodeArray.Length - index1);
          for (; index1 < huffmanListNodeArray.Length && huffmanListNodeArray[index1].Length < 99999; ++index1)
            huffmanListNodeList2.Add(huffmanListNodeArray[index1]);
        }
        else
        {
          int num3 = 1 << num2 - length;
          HuffmanListNode huffmanListNode = huffmanListNodeArray[index1];
          for (int index2 = 0; index2 < num3; ++index2)
          {
            int num4 = index2 << length | huffmanListNode.Bits;
            while (huffmanListNodeList1.Count <= num4)
              huffmanListNodeList1.Add((HuffmanListNode) null);
            huffmanListNodeList1[num4] = huffmanListNode;
          }
        }
      }
      while (huffmanListNodeList1.Count < 1 << num2)
        huffmanListNodeList1.Add((HuffmanListNode) null);
      this.TableBits = num2;
      this.PrefixTree = (IReadOnlyList<HuffmanListNode>) huffmanListNodeList1;
      this.OverflowList = (IReadOnlyList<HuffmanListNode>) huffmanListNodeList2;
    }

    int IComparer<HuffmanListNode>.Compare(HuffmanListNode x, HuffmanListNode y)
    {
      int num = x.Length - y.Length;
      return num == 0 ? x.Bits - y.Bits : num;
    }
  }
}
