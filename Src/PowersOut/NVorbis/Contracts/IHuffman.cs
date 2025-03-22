
// Type: NVorbis.Contracts.IHuffman
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System.Collections.Generic;

#nullable disable
namespace NVorbis.Contracts
{
  internal interface IHuffman
  {
    int TableBits { get; }

    IReadOnlyList<HuffmanListNode> PrefixTree { get; }

    IReadOnlyList<HuffmanListNode> OverflowList { get; }

    void GenerateTable(IReadOnlyList<int> value, int[] lengthList, int[] codeList);
  }
}
