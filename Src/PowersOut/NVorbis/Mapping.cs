
// Type: NVorbis.Mapping
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.IO;

#nullable disable
namespace NVorbis
{
  internal class Mapping : IMapping
  {
    private IMdct _mdct;
    private int[] _couplingAngle;
    private int[] _couplingMangitude;
    private IFloor[] _submapFloor;
    private IResidue[] _submapResidue;
    private IFloor[] _channelFloor;
    private IResidue[] _channelResidue;

    public void Init(
      IPacket packet,
      int channels,
      IFloor[] floors,
      IResidue[] residues,
      IMdct mdct)
    {
      int length1 = 1;
      if (packet.ReadBit())
        length1 += (int) packet.ReadBits(4);
      int length2 = 0;
      if (packet.ReadBit())
        length2 = (int) packet.ReadBits(8) + 1;
      int count = Utils.ilog(channels - 1);
      this._couplingAngle = new int[length2];
      this._couplingMangitude = new int[length2];
      for (int index = 0; index < length2; ++index)
      {
        int num1 = (int) packet.ReadBits(count);
        int num2 = (int) packet.ReadBits(count);
        if (num1 == num2 || num1 > channels - 1 || num2 > channels - 1)
          throw new InvalidDataException("Invalid magnitude or angle in mapping header!");
        this._couplingAngle[index] = num2;
        this._couplingMangitude[index] = num1;
      }
      if (packet.ReadBits(2) != 0UL)
        throw new InvalidDataException("Reserved bits not 0 in mapping header.");
      int[] numArray = new int[channels];
      if (length1 > 1)
      {
        for (int index = 0; index < channels; ++index)
        {
          numArray[index] = (int) packet.ReadBits(4);
          if (numArray[index] > length1)
            throw new InvalidDataException("Invalid channel mux submap index in mapping header!");
        }
      }
      this._submapFloor = new IFloor[length1];
      this._submapResidue = new IResidue[length1];
      for (int index1 = 0; index1 < length1; ++index1)
      {
        packet.SkipBits(8);
        int index2 = (int) packet.ReadBits(8);
        if (index2 >= floors.Length)
          throw new InvalidDataException("Invalid floor number in mapping header!");
        int index3 = (int) packet.ReadBits(8);
        if (index3 >= residues.Length)
          throw new InvalidDataException("Invalid residue number in mapping header!");
        this._submapFloor[index1] = floors[index2];
        this._submapResidue[index1] = residues[index3];
      }
      this._channelFloor = new IFloor[channels];
      this._channelResidue = new IResidue[channels];
      for (int index = 0; index < channels; ++index)
      {
        this._channelFloor[index] = this._submapFloor[numArray[index]];
        this._channelResidue[index] = this._submapResidue[numArray[index]];
      }
      this._mdct = mdct;
    }

    public void DecodePacket(IPacket packet, int blockSize, int channels, float[][] buffer)
    {
      int num1 = blockSize >> 1;
      IFloorData[] floorDataArray = new IFloorData[this._channelFloor.Length];
      bool[] doNotDecodeChannel = new bool[this._channelFloor.Length];
      for (int channel = 0; channel < this._channelFloor.Length; ++channel)
      {
        floorDataArray[channel] = this._channelFloor[channel].Unpack(packet, blockSize, channel);
        doNotDecodeChannel[channel] = !floorDataArray[channel].ExecuteChannel;
        Array.Clear((Array) buffer[channel], 0, num1);
      }
      for (int index = 0; index < this._couplingAngle.Length; ++index)
      {
        if (floorDataArray[this._couplingAngle[index]].ExecuteChannel || floorDataArray[this._couplingMangitude[index]].ExecuteChannel)
        {
          floorDataArray[this._couplingAngle[index]].ForceEnergy = true;
          floorDataArray[this._couplingMangitude[index]].ForceEnergy = true;
        }
      }
      for (int index1 = 0; index1 < this._submapFloor.Length; ++index1)
      {
        for (int index2 = 0; index2 < this._channelFloor.Length; ++index2)
        {
          if (this._submapFloor[index1] != this._channelFloor[index2] || this._submapResidue[index1] != this._channelResidue[index2])
            floorDataArray[index2].ForceNoEnergy = true;
        }
        this._submapResidue[index1].Decode(packet, doNotDecodeChannel, blockSize, buffer);
      }
      for (int index3 = this._couplingAngle.Length - 1; index3 >= 0; --index3)
      {
        if (floorDataArray[this._couplingAngle[index3]].ExecuteChannel || floorDataArray[this._couplingMangitude[index3]].ExecuteChannel)
        {
          float[] numArray1 = buffer[this._couplingMangitude[index3]];
          float[] numArray2 = buffer[this._couplingAngle[index3]];
          for (int index4 = 0; index4 < num1; ++index4)
          {
            float num2 = numArray1[index4];
            float num3 = numArray2[index4];
            float num4;
            float num5;
            if ((double) num2 > 0.0)
            {
              if ((double) num3 > 0.0)
              {
                num4 = num2;
                num5 = num2 - num3;
              }
              else
              {
                num5 = num2;
                num4 = num2 + num3;
              }
            }
            else if ((double) num3 > 0.0)
            {
              num4 = num2;
              num5 = num2 + num3;
            }
            else
            {
              num5 = num2;
              num4 = num2 - num3;
            }
            numArray1[index4] = num4;
            numArray2[index4] = num5;
          }
        }
      }
      for (int index = 0; index < this._channelFloor.Length; ++index)
      {
        if (floorDataArray[index].ExecuteChannel)
        {
          this._channelFloor[index].Apply(floorDataArray[index], blockSize, buffer[index]);
          this._mdct.Reverse(buffer[index], blockSize);
        }
        else
          Array.Clear((Array) buffer[index], num1, num1);
      }
    }
  }
}
