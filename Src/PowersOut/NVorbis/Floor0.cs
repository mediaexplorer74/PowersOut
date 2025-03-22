
// Type: NVorbis.Floor0
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
  internal class Floor0 : IFloor
  {
    private int _order;
    private int _rate;
    private int _bark_map_size;
    private int _ampBits;
    private int _ampOfs;
    private int _ampDiv;
    private ICodebook[] _books;
    private int _bookBits;
    private Dictionary<int, float[]> _wMap;
    private Dictionary<int, int[]> _barkMaps;

    public void Init(
      IPacket packet,
      int channels,
      int block0Size,
      int block1Size,
      ICodebook[] codebooks)
    {
      this._order = (int) packet.ReadBits(8);
      this._rate = (int) packet.ReadBits(16);
      this._bark_map_size = (int) packet.ReadBits(16);
      this._ampBits = (int) packet.ReadBits(6);
      this._ampOfs = (int) packet.ReadBits(8);
      this._books = new ICodebook[(int) packet.ReadBits(4) + 1];
      if (this._order < 1 || this._rate < 1 || this._bark_map_size < 1 || this._books.Length == 0)
        throw new InvalidDataException();
      this._ampDiv = (1 << this._ampBits) - 1;
      for (int index1 = 0; index1 < this._books.Length; ++index1)
      {
        int index2 = (int) packet.ReadBits(8);
        if (index2 < 0 || index2 >= codebooks.Length)
          throw new InvalidDataException();
        ICodebook codebook = codebooks[index2];
        if (codebook.MapType == 0 || codebook.Dimensions < 1)
          throw new InvalidDataException();
        this._books[index1] = codebook;
      }
      this._bookBits = Utils.ilog(this._books.Length);
      this._barkMaps = new Dictionary<int, int[]>()
      {
        [block0Size] = this.SynthesizeBarkCurve(block0Size / 2),
        [block1Size] = this.SynthesizeBarkCurve(block1Size / 2)
      };
      this._wMap = new Dictionary<int, float[]>()
      {
        [block0Size] = this.SynthesizeWDelMap(block0Size / 2),
        [block1Size] = this.SynthesizeWDelMap(block1Size / 2)
      };
    }

    private int[] SynthesizeBarkCurve(int n)
    {
      float num = (float) this._bark_map_size / Floor0.toBARK((double) (this._rate / 2));
      int[] numArray = new int[n + 1];
      for (int index = 0; index < n - 1; ++index)
        numArray[index] = Math.Min(this._bark_map_size - 1, (int) Math.Floor((double) Floor0.toBARK((double) this._rate / 2.0 / (double) n * (double) index) * (double) num));
      numArray[n] = -1;
      return numArray;
    }

    private static float toBARK(double lsp)
    {
      return (float) (13.1 * Math.Atan(0.00074 * lsp) + 2.24 * Math.Atan(1.85E-08 * lsp * lsp) + 0.0001 * lsp);
    }

    private float[] SynthesizeWDelMap(int n)
    {
      float num = 3.14159274f / (float) this._bark_map_size;
      float[] numArray = new float[n];
      for (int index = 0; index < n; ++index)
        numArray[index] = 2f * (float) Math.Cos((double) num * (double) index);
      return numArray;
    }

    public IFloorData Unpack(IPacket packet, int blockSize, int channel)
    {
      Floor0.Data data = new Floor0.Data()
      {
        Coeff = new float[this._order + 1]
      };
      data.Amp = (float) packet.ReadBits(this._ampBits);
      if ((double) data.Amp > 0.0)
      {
        Array.Clear((Array) data.Coeff, 0, data.Coeff.Length);
        data.Amp = data.Amp / (float) this._ampDiv * (float) this._ampOfs;
        uint index1 = (uint) packet.ReadBits(this._bookBits);
        if ((long) index1 >= (long) this._books.Length)
        {
          data.Amp = 0.0f;
          return (IFloorData) data;
        }
        ICodebook book = this._books[(int) index1];
        int index2 = 0;
label_9:
        while (index2 < this._order)
        {
          int entry = book.DecodeScalar(packet);
          if (entry == -1)
          {
            data.Amp = 0.0f;
            return (IFloorData) data;
          }
          int dim = 0;
          while (true)
          {
            if (index2 < this._order && dim < book.Dimensions)
            {
              data.Coeff[index2] = book[entry, dim];
              ++dim;
              ++index2;
            }
            else
              goto label_9;
          }
        }
        float num = 0.0f;
        int index3 = 0;
        while (index3 < this._order)
        {
          for (int index4 = 0; index3 < this._order && index4 < book.Dimensions; ++index4)
          {
            data.Coeff[index3] += num;
            ++index3;
          }
          num = data.Coeff[index3 - 1];
        }
      }
      return (IFloorData) data;
    }

    public void Apply(IFloorData floorData, int blockSize, float[] residue)
    {
      if (!(floorData is Floor0.Data data))
        throw new ArgumentException("Incorrect packet data!");
      int num1 = blockSize / 2;
      if ((double) data.Amp > 0.0)
      {
        int[] barkMap = this._barkMaps[blockSize];
        float[] w = this._wMap[blockSize];
        for (int index = 0; index < this._order; ++index)
          data.Coeff[index] = 2f * (float) Math.Cos((double) data.Coeff[index]);
        int index1 = 0;
        while (index1 < num1)
        {
          int index2 = barkMap[index1];
          float num2 = 0.5f;
          float num3 = 0.5f;
          float num4 = w[index2];
          int index3;
          for (index3 = 1; index3 < this._order; index3 += 2)
          {
            num3 *= num4 - data.Coeff[index3 - 1];
            num2 *= num4 - data.Coeff[index3];
          }
          float num5;
          float num6;
          if (index3 == this._order)
          {
            float num7 = num3 * (num4 - data.Coeff[index3 - 1]);
            num5 = num2 * (num2 * (float) (4.0 - (double) num4 * (double) num4));
            num6 = num7 * num7;
          }
          else
          {
            num5 = num2 * (num2 * (2f - num4));
            num6 = num3 * (num3 * (2f + num4));
          }
          float num8 = (float) Math.Exp((double) (data.Amp / (float) Math.Sqrt((double) num5 + (double) num6) - (float) this._ampOfs) * 0.1151292473077774);
          residue[index1] *= num8;
          while (barkMap[++index1] == index2)
            residue[index1] *= num8;
        }
      }
      else
        Array.Clear((Array) residue, 0, num1);
    }

    private class Data : IFloorData
    {
      internal float[] Coeff;
      internal float Amp;

      public bool ExecuteChannel
      {
        get => (this.ForceEnergy || (double) this.Amp > 0.0) && !this.ForceNoEnergy;
      }

      public bool ForceEnergy { get; set; }

      public bool ForceNoEnergy { get; set; }
    }
  }
}
