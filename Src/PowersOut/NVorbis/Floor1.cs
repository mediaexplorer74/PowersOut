
// Type: NVorbis.Floor1
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NVorbis
{
  internal class Floor1 : IFloor
  {
    private int[] _partitionClass;
    private int[] _classDimensions;
    private int[] _classSubclasses;
    private int[] _xList;
    private int[] _classMasterBookIndex;
    private int[] _hNeigh;
    private int[] _lNeigh;
    private int[] _sortIdx;
    private int _multiplier;
    private int _range;
    private int _yBits;
    private ICodebook[] _classMasterbooks;
    private ICodebook[][] _subclassBooks;
    private int[][] _subclassBookIndex;
    private static readonly int[] _rangeLookup = new int[4]
    {
      256,
      128,
      86,
      64
    };
    private static readonly int[] _yBitsLookup = new int[4]
    {
      8,
      7,
      7,
      6
    };
    private static readonly float[] inverse_dB_table = new float[256]
    {
      1.06498632E-07f,
      1.1341951E-07f,
      1.20790148E-07f,
      1.28639783E-07f,
      1.369995E-07f,
      1.459025E-07f,
      1.55384086E-07f,
      1.65481808E-07f,
      1.76235744E-07f,
      1.87688556E-07f,
      1.998856E-07f,
      2.128753E-07f,
      2.26709133E-07f,
      2.41441967E-07f,
      2.57132228E-07f,
      2.73842119E-07f,
      2.91637917E-07f,
      3.10590224E-07f,
      3.307741E-07f,
      3.52269666E-07f,
      3.75162131E-07f,
      3.995423E-07f,
      4.255068E-07f,
      4.53158634E-07f,
      4.82607447E-07f,
      5.1397E-07f,
      5.47370632E-07f,
      5.829419E-07f,
      6.208247E-07f,
      6.611694E-07f,
      7.041359E-07f,
      7.49894639E-07f,
      7.98627E-07f,
      8.505263E-07f,
      9.057983E-07f,
      9.646621E-07f,
      1.02735135E-06f,
      1.0941144E-06f,
      1.16521608E-06f,
      1.24093845E-06f,
      1.32158164E-06f,
      1.40746545E-06f,
      1.49893049E-06f,
      1.59633942E-06f,
      1.70007854E-06f,
      1.81055918E-06f,
      1.92821949E-06f,
      2.053526E-06f,
      2.18697573E-06f,
      2.3290977E-06f,
      2.48045581E-06f,
      2.64164964E-06f,
      2.813319E-06f,
      2.9961443E-06f,
      3.19085052E-06f,
      3.39821E-06f,
      3.619045E-06f,
      3.85423073E-06f,
      4.10470057E-06f,
      4.371447E-06f,
      4.6555283E-06f,
      4.958071E-06f,
      5.280274E-06f,
      5.623416E-06f,
      5.988857E-06f,
      6.37804669E-06f,
      6.79252844E-06f,
      7.23394533E-06f,
      7.704048E-06f,
      8.2047E-06f,
      8.737888E-06f,
      9.305725E-06f,
      9.910464E-06f,
      1.05545014E-05f,
      1.12403923E-05f,
      1.19708557E-05f,
      1.27487892E-05f,
      1.3577278E-05f,
      1.44596061E-05f,
      1.53992714E-05f,
      1.64000048E-05f,
      1.74657689E-05f,
      1.86007928E-05f,
      1.98095768E-05f,
      2.10969138E-05f,
      2.24679115E-05f,
      2.39280016E-05f,
      2.54829774E-05f,
      2.71390054E-05f,
      2.890265E-05f,
      3.078091E-05f,
      3.27812268E-05f,
      3.49115326E-05f,
      3.718028E-05f,
      3.95964671E-05f,
      4.21696677E-05f,
      4.491009E-05f,
      4.7828602E-05f,
      5.09367746E-05f,
      5.424693E-05f,
      5.77722021E-05f,
      6.152657E-05f,
      6.552491E-05f,
      6.97830837E-05f,
      7.43179844E-05f,
      7.914758E-05f,
      8.429104E-05f,
      8.976875E-05f,
      9.560242E-05f,
      0.000101815211f,
      0.000108431741f,
      0.000115478237f,
      0.000122982674f,
      0.000130974775f,
      0.000139486248f,
      0.000148550855f,
      0.000158204537f,
      0.000168485552f,
      0.00017943469f,
      0.000191095358f,
      0.000203513817f,
      0.0002167393f,
      0.000230824226f,
      0.000245824485f,
      0.000261799549f,
      0.000278812746f,
      0.000296931568f,
      0.000316227874f,
      0.000336778146f,
      0.000358663878f,
      0.000381971884f,
      0.00040679457f,
      0.000433230365f,
      0.0004613841f,
      0.0004913675f,
      1f / (703f * (float)Math.E),
      0.0005573062f,
      0.0005935231f,
      0.0006320936f,
      0.0006731706f,
      0.000716917f,
      0.0007635063f,
      0.000813123246f,
      0.000865964568f,
      0.000922239851f,
      0.0009821722f,
      0.00104599923f,
      0.00111397426f,
      0.00118636654f,
      0.00126346329f,
      0.0013455702f,
      0.00143301289f,
      0.00152613816f,
      0.00162531529f,
      0.00173093739f,
      0.00184342347f,
      0.00196321961f,
      0.00209080055f,
      0.0022266726f,
      0.00237137428f,
      0.00252547953f,
      0.00268959929f,
      0.00286438479f,
      0.0030505287f,
      0.003248769f,
      0.00345989247f,
      0.00368473586f,
      0.00392419053f,
      0.00417920668f,
      0.004450795f,
      0.004740033f,
      0.005048067f,
      0.0053761187f,
      0.005725489f,
      0.00609756354f,
      0.00649381755f,
      0.00691582263f,
      0.00736525143f,
      0.007843887f,
      0.008353627f,
      0.008896492f,
      0.009474637f,
      0.010090352f,
      0.01074608f,
      0.0114444206f,
      0.012188144f,
      0.0129801976f,
      0.0138237253f,
      0.0147220679f,
      0.0156787913f,
      0.0166976862f,
      0.0177827962f,
      0.0189384222f,
      0.0201691482f,
      0.0214798544f,
      0.0228757355f,
      0.02436233f,
      0.0259455312f,
      0.0276316181f,
      0.0294272769f,
      0.0313396268f,
      0.03337625f,
      0.0355452262f,
      0.0378551558f,
      0.0403152f,
      0.0429351069f,
      0.0457252748f,
      0.0486967564f,
      0.05186135f,
      0.05523159f,
      0.05882085f,
      0.0626433641f,
      0.06671428f,
      0.07104975f,
      0.0756669641f,
      0.08058423f,
      0.08582105f,
      0.09139818f,
      0.0973377451f,
      0.1036633f,
      0.110399932f,
      0.117574342f,
      0.125214979f,
      0.133352146f,
      0.142018124f,
      0.151247263f,
      0.161076173f,
      0.1715438f,
      0.182691678f,
      0.194564015f,
      0.207207873f,
      0.220673427f,
      0.235014021f,
      0.250286549f,
      0.266551584f,
      0.283873618f,
      0.3023213f,
      0.32196787f,
      69f * (float) Math.E / 547f,
      0.365174145f,
      0.3889052f,
      0.414178461f,
      0.44109413f,
      0.4697589f,
      0.50028646f,
      0.532797933f,
      0.5674221f,
      0.6042964f,
      0.643566966f,
      0.6853896f,
      0.729930043f,
      0.777365f,
      0.8278826f,
      0.881683052f,
      0.9389798f,
      1f
    };

    public void Init(
      IPacket packet,
      int channels,
      int block0Size,
      int block1Size,
      ICodebook[] codebooks)
    {
      int num1 = -1;
      this._partitionClass = new int[(int) packet.ReadBits(5)];
      for (int index = 0; index < this._partitionClass.Length; ++index)
      {
        this._partitionClass[index] = (int) packet.ReadBits(4);
        if (this._partitionClass[index] > num1)
          num1 = this._partitionClass[index];
      }
      int length;
      this._classDimensions = new int[length = num1 + 1];
      this._classSubclasses = new int[length];
      this._classMasterbooks = new ICodebook[length];
      this._classMasterBookIndex = new int[length];
      this._subclassBooks = new ICodebook[length][];
      this._subclassBookIndex = new int[length][];
      for (int index1 = 0; index1 < length; ++index1)
      {
        this._classDimensions[index1] = (int) packet.ReadBits(3) + 1;
        this._classSubclasses[index1] = (int) packet.ReadBits(2);
        if (this._classSubclasses[index1] > 0)
        {
          this._classMasterBookIndex[index1] = (int) packet.ReadBits(8);
          this._classMasterbooks[index1] = codebooks[this._classMasterBookIndex[index1]];
        }
        this._subclassBooks[index1] = new ICodebook[1 << this._classSubclasses[index1]];
        this._subclassBookIndex[index1] = new int[this._subclassBooks[index1].Length];
        for (int index2 = 0; index2 < this._subclassBooks[index1].Length; ++index2)
        {
          int index3 = (int) packet.ReadBits(8) - 1;
          if (index3 >= 0)
            this._subclassBooks[index1][index2] = codebooks[index3];
          this._subclassBookIndex[index1][index2] = index3;
        }
      }
      this._multiplier = (int) packet.ReadBits(2);
      this._range = Floor1._rangeLookup[this._multiplier];
      this._yBits = Floor1._yBitsLookup[this._multiplier];
      ++this._multiplier;
      int count = (int) packet.ReadBits(4);
      List<int> intList = new List<int>();
      intList.Add(0);
      intList.Add(1 << count);
      for (int index4 = 0; index4 < this._partitionClass.Length; ++index4)
      {
        int index5 = this._partitionClass[index4];
        for (int index6 = 0; index6 < this._classDimensions[index5]; ++index6)
          intList.Add((int) packet.ReadBits(count));
      }
      this._xList = intList.ToArray();
      this._lNeigh = new int[intList.Count];
      this._hNeigh = new int[intList.Count];
      this._sortIdx = new int[intList.Count];
      this._sortIdx[0] = 0;
      this._sortIdx[1] = 1;
      for (int index7 = 2; index7 < this._lNeigh.Length; ++index7)
      {
        this._lNeigh[index7] = 0;
        this._hNeigh[index7] = 1;
        this._sortIdx[index7] = index7;
        for (int index8 = 2; index8 < index7; ++index8)
        {
          int x = this._xList[index8];
          if (x < this._xList[index7])
          {
            if (x > this._xList[this._lNeigh[index7]])
              this._lNeigh[index7] = index8;
          }
          else if (x < this._xList[this._hNeigh[index7]])
            this._hNeigh[index7] = index8;
        }
      }
      for (int index9 = 0; index9 < this._sortIdx.Length - 1; ++index9)
      {
        for (int index10 = index9 + 1; index10 < this._sortIdx.Length; ++index10)
        {
          if (this._xList[index9] == this._xList[index10])
            throw new InvalidDataException();
          if (this._xList[this._sortIdx[index9]] > this._xList[this._sortIdx[index10]])
          {
            int num2 = this._sortIdx[index9];
            this._sortIdx[index9] = this._sortIdx[index10];
            this._sortIdx[index10] = num2;
          }
        }
      }
    }

    public IFloorData Unpack(IPacket packet, int blockSize, int channel)
    {
      Floor1.Data data = new Floor1.Data();
      if (packet.ReadBit())
      {
        int index1 = 2;
        data.Posts[0] = (int) packet.ReadBits(this._yBits);
        data.Posts[1] = (int) packet.ReadBits(this._yBits);
        for (int index2 = 0; index2 < this._partitionClass.Length; ++index2)
        {
          int index3 = this._partitionClass[index2];
          int classDimension = this._classDimensions[index3];
          int classSubclass = this._classSubclasses[index3];
          int num1 = (1 << classSubclass) - 1;
          uint num2 = 0;
          if (classSubclass > 0 && (num2 = (uint) this._classMasterbooks[index3].DecodeScalar(packet)) == uint.MaxValue)
          {
            index1 = 0;
            break;
          }
          for (int index4 = 0; index4 < classDimension; ++index4)
          {
            ICodebook codebook = this._subclassBooks[index3][(long) num2 & (long) num1];
            num2 >>= classSubclass;
            if (codebook != null && (data.Posts[index1] = codebook.DecodeScalar(packet)) == -1)
            {
              index1 = 0;
              index2 = this._partitionClass.Length;
              break;
            }
            ++index1;
          }
        }
        data.PostCount = index1;
      }
      return (IFloorData) data;
    }

    public void Apply(IFloorData floorData, int blockSize, float[] residue)
    {
      if (!(floorData is Floor1.Data data))
        throw new ArgumentException("Incorrect packet data!", "packetData");
      int x1 = blockSize / 2;
      if (data.PostCount > 0)
      {
        bool[] flagArray = this.UnwrapPosts(data);
        int x0 = 0;
        int num = data.Posts[0] * this._multiplier;
        for (int index1 = 1; index1 < data.PostCount; ++index1)
        {
          int index2 = this._sortIdx[index1];
          if (flagArray[index2])
          {
            int x = this._xList[index2];
            int y1 = data.Posts[index2] * this._multiplier;
            if (x0 < x1)
              this.RenderLineMulti(x0, num, Math.Min(x, x1), y1, residue);
            x0 = x;
            num = y1;
          }
          if (x0 >= x1)
            break;
        }
        if (x0 >= x1)
          return;
        this.RenderLineMulti(x0, num, x1, num, residue);
      }
      else
        Array.Clear((Array) residue, 0, x1);
    }

    private bool[] UnwrapPosts(Floor1.Data data)
    {
      bool[] flagArray = new bool[64];
      flagArray[0] = true;
      flagArray[1] = true;
      int[] numArray = new int[64];
      numArray[0] = data.Posts[0];
      numArray[1] = data.Posts[1];
      for (int index1 = 2; index1 < data.PostCount; ++index1)
      {
        int index2 = this._lNeigh[index1];
        int index3 = this._hNeigh[index1];
        int num1 = this.RenderPoint(this._xList[index2], numArray[index2], this._xList[index3], numArray[index3], this._xList[index1]);
        int post = data.Posts[index1];
        int num2 = this._range - num1;
        int num3 = num1;
        int num4 = num2 >= num3 ? num3 * 2 : num2 * 2;
        if (post != 0)
        {
          flagArray[index2] = true;
          flagArray[index3] = true;
          flagArray[index1] = true;
          numArray[index1] = post < num4 ? (post % 2 != 1 ? num1 + post / 2 : num1 - (post + 1) / 2) : (num2 <= num3 ? num1 - post + num2 - 1 : post - num3 + num1);
        }
        else
        {
          flagArray[index1] = false;
          numArray[index1] = num1;
        }
      }
      for (int index = 0; index < data.PostCount; ++index)
        data.Posts[index] = numArray[index];
      return flagArray;
    }

    private int RenderPoint(int x0, int y0, int x1, int y1, int X)
    {
      int num1 = y1 - y0;
      int num2 = x1 - x0;
      int num3 = Math.Abs(num1) * (X - x0) / num2;
      return num1 < 0 ? y0 - num3 : y0 + num3;
    }

    private void RenderLineMulti(int x0, int y0, int x1, int y1, float[] v)
    {
      int num1 = y1 - y0;
      int num2 = x1 - x0;
      int num3 = Math.Abs(num1);
      int num4 = 1 - (num1 >> 31 & 1) * 2;
      int num5 = num1 / num2;
      int index1 = x0;
      int index2 = y0;
      int num6 = -num2;
      v[x0] *= Floor1.inverse_dB_table[y0];
      int num7 = num3 - Math.Abs(num5) * num2;
      while (++index1 < x1)
      {
        index2 += num5;
        num6 += num7;
        if (num6 >= 0)
        {
          num6 -= num2;
          index2 += num4;
        }
        v[index1] *= Floor1.inverse_dB_table[index2];
      }
    }

    private class Data : IFloorData
    {
      internal int[] Posts = new int[64];
      internal int PostCount;

      public bool ExecuteChannel => (this.ForceEnergy || this.PostCount > 0) && !this.ForceNoEnergy;

      public bool ForceEnergy { get; set; }

      public bool ForceNoEnergy { get; set; }
    }
  }
}
