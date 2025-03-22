
// Type: NVorbis.Mdct
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;
using System.Collections.Generic;

#nullable disable
namespace NVorbis
{
  internal class Mdct : IMdct
  {
    private const float M_PI = 3.14159274f;
    private Dictionary<int, Mdct.MdctImpl> _setupCache = new Dictionary<int, Mdct.MdctImpl>();

    public void Reverse(float[] samples, int sampleCount)
    {
      Mdct.MdctImpl mdctImpl;
      if (!this._setupCache.TryGetValue(sampleCount, out mdctImpl))
      {
        mdctImpl = new Mdct.MdctImpl(sampleCount);
        this._setupCache[sampleCount] = mdctImpl;
      }
      mdctImpl.CalcReverse(samples);
    }

    private class MdctImpl
    {
      private readonly int _n;
      private readonly int _n2;
      private readonly int _n4;
      private readonly int _n8;
      private readonly int _ld;
      private readonly float[] _a;
      private readonly float[] _b;
      private readonly float[] _c;
      private readonly ushort[] _bitrev;

      public MdctImpl(int n)
      {
        this._n = n;
        this._n2 = n >> 1;
        this._n4 = this._n2 >> 1;
        this._n8 = this._n4 >> 1;
        this._ld = Utils.ilog(n) - 1;
        this._a = new float[this._n2];
        this._b = new float[this._n2];
        this._c = new float[this._n4];
        int index1;
        int num1 = index1 = 0;
        while (num1 < this._n4)
        {
          this._a[index1] = (float) Math.Cos((double) (4 * num1) * 3.1415927410125732 / (double) n);
          this._a[index1 + 1] = (float) -Math.Sin((double) (4 * num1) * 3.1415927410125732 / (double) n);
          this._b[index1] = (float) Math.Cos((double) (index1 + 1) * 3.1415927410125732 / (double) n / 2.0) * 0.5f;
          this._b[index1 + 1] = (float) Math.Sin((double) (index1 + 1) * 3.1415927410125732 / (double) n / 2.0) * 0.5f;
          ++num1;
          index1 += 2;
        }
        int index2;
        int num2 = index2 = 0;
        while (num2 < this._n8)
        {
          this._c[index2] = (float) Math.Cos((double) (2 * (index2 + 1)) * 3.1415927410125732 / (double) n);
          this._c[index2 + 1] = (float) -Math.Sin((double) (2 * (index2 + 1)) * 3.1415927410125732 / (double) n);
          ++num2;
          index2 += 2;
        }
        this._bitrev = new ushort[this._n8];
        for (int n1 = 0; n1 < this._n8; ++n1)
          this._bitrev[n1] = (ushort) (Utils.BitReverse((uint) n1, this._ld - 3) << 2);
      }

      internal void CalcReverse(float[] buffer)
      {
        float[] numArray1 = new float[this._n2];
        int index1 = this._n2 - 2;
        int index2 = 0;
        int index3 = 0;
        for (int n2 = this._n2; index3 != n2; index3 += 4)
        {
          numArray1[index1 + 1] = (float) ((double) buffer[index3] * (double) this._a[index2] - (double) buffer[index3 + 2] * (double) this._a[index2 + 1]);
          numArray1[index1] = (float) ((double) buffer[index3] * (double) this._a[index2 + 1] + (double) buffer[index3 + 2] * (double) this._a[index2]);
          index1 -= 2;
          index2 += 2;
        }
        int index4 = this._n2 - 3;
        while (index1 >= 0)
        {
          numArray1[index1 + 1] = (float) (-(double) buffer[index4 + 2] * (double) this._a[index2] - -(double) buffer[index4] * (double) this._a[index2 + 1]);
          numArray1[index1] = (float) (-(double) buffer[index4 + 2] * (double) this._a[index2 + 1] + -(double) buffer[index4] * (double) this._a[index2]);
          index1 -= 2;
          index2 += 2;
          index4 -= 4;
        }
        float[] e1 = buffer;
        float[] numArray2 = numArray1;
        int index5 = this._n2 - 8;
        int n4_1 = this._n4;
        int index6 = 0;
        int n4_2 = this._n4;
        int index7 = 0;
        while (index5 >= 0)
        {
          float num1 = numArray2[n4_1 + 1] - numArray2[index6 + 1];
          float num2 = numArray2[n4_1] - numArray2[index6];
          e1[n4_2 + 1] = numArray2[n4_1 + 1] + numArray2[index6 + 1];
          e1[n4_2] = numArray2[n4_1] + numArray2[index6];
          e1[index7 + 1] = (float) ((double) num1 * (double) this._a[index5 + 4] - (double) num2 * (double) this._a[index5 + 5]);
          e1[index7] = (float) ((double) num2 * (double) this._a[index5 + 4] + (double) num1 * (double) this._a[index5 + 5]);
          float num3 = numArray2[n4_1 + 3] - numArray2[index6 + 3];
          float num4 = numArray2[n4_1 + 2] - numArray2[index6 + 2];
          e1[n4_2 + 3] = numArray2[n4_1 + 3] + numArray2[index6 + 3];
          e1[n4_2 + 2] = numArray2[n4_1 + 2] + numArray2[index6 + 2];
          e1[index7 + 3] = (float) ((double) num3 * (double) this._a[index5] - (double) num4 * (double) this._a[index5 + 1]);
          e1[index7 + 2] = (float) ((double) num4 * (double) this._a[index5] + (double) num3 * (double) this._a[index5 + 1]);
          index5 -= 8;
          n4_2 += 4;
          index7 += 4;
          n4_1 += 4;
          index6 += 4;
        }
        int n1 = this._n >> 4;
        float[] e2 = e1;
        int num5 = this._n2 - 1;
        int n4_3 = this._n4;
        int i_off1 = num5 - 0;
        int k_off1 = -this._n8;
        this.step3_iter0_loop(n1, e2, i_off1, k_off1);
        this.step3_iter0_loop(this._n >> 4, e1, this._n2 - 1 - this._n4, -this._n8);
        int lim = this._n >> 5;
        float[] e3 = e1;
        int num6 = this._n2 - 1;
        int n8 = this._n8;
        int d0 = num6 - 0;
        int k_off2 = -(this._n >> 4);
        this.step3_inner_r_loop(lim, e3, d0, k_off2, 16);
        this.step3_inner_r_loop(this._n >> 5, e1, this._n2 - 1 - this._n8, -(this._n >> 4), 16);
        this.step3_inner_r_loop(this._n >> 5, e1, this._n2 - 1 - this._n8 * 2, -(this._n >> 4), 16);
        this.step3_inner_r_loop(this._n >> 5, e1, this._n2 - 1 - this._n8 * 3, -(this._n >> 4), 16);
        int num7;
        for (num7 = 2; num7 < this._ld - 3 >> 1; ++num7)
        {
          int num8 = this._n >> num7 + 2;
          int num9 = num8 >> 1;
          int num10 = 1 << num7 + 1;
          for (int index8 = 0; index8 < num10; ++index8)
            this.step3_inner_r_loop(this._n >> num7 + 4, e1, this._n2 - 1 - num8 * index8, -num9, 1 << num7 + 3);
        }
        for (; num7 < this._ld - 6; ++num7)
        {
          int k0 = this._n >> num7 + 2;
          int a_off = 1 << num7 + 3;
          int num11 = k0 >> 1;
          int num12 = this._n >> num7 + 6;
          int n2 = 1 << num7 + 1;
          int i_off2 = this._n2 - 1;
          int a = 0;
          for (int index9 = num12; index9 > 0; --index9)
          {
            this.step3_inner_s_loop(n2, e1, i_off2, -num11, a, a_off, k0);
            a += a_off * 4;
            i_off2 -= 8;
          }
        }
        this.step3_inner_s_loop_ld654(this._n >> 5, e1, this._n2 - 1, this._n);
        int index10 = 0;
        int index11 = this._n4 - 4;
        int index12 = this._n2 - 4;
        while (index11 >= 0)
        {
          int index13 = (int) this._bitrev[index10];
          numArray2[index12 + 3] = e1[index13];
          numArray2[index12 + 2] = e1[index13 + 1];
          numArray2[index11 + 3] = e1[index13 + 2];
          numArray2[index11 + 2] = e1[index13 + 3];
          int index14 = (int) this._bitrev[index10 + 1];
          numArray2[index12 + 1] = e1[index14];
          numArray2[index12] = e1[index14 + 1];
          numArray2[index11 + 1] = e1[index14 + 2];
          numArray2[index11] = e1[index14 + 3];
          index11 -= 4;
          index12 -= 4;
          index10 += 2;
        }
        int index15 = 0;
        int index16 = 0;
        for (int index17 = this._n2 - 4; index16 < index17; index17 -= 4)
        {
          float num13 = numArray2[index16] - numArray2[index17 + 2];
          float num14 = numArray2[index16 + 1] + numArray2[index17 + 3];
          float num15 = (float) ((double) this._c[index15 + 1] * (double) num13 + (double) this._c[index15] * (double) num14);
          float num16 = (float) ((double) this._c[index15 + 1] * (double) num14 - (double) this._c[index15] * (double) num13);
          float num17 = numArray2[index16] + numArray2[index17 + 2];
          float num18 = numArray2[index16 + 1] - numArray2[index17 + 3];
          numArray2[index16] = num17 + num15;
          numArray2[index16 + 1] = num18 + num16;
          numArray2[index17 + 2] = num17 - num15;
          numArray2[index17 + 3] = num16 - num18;
          float num19 = numArray2[index16 + 2] - numArray2[index17];
          float num20 = numArray2[index16 + 3] + numArray2[index17 + 1];
          float num21 = (float) ((double) this._c[index15 + 3] * (double) num19 + (double) this._c[index15 + 2] * (double) num20);
          float num22 = (float) ((double) this._c[index15 + 3] * (double) num20 - (double) this._c[index15 + 2] * (double) num19);
          float num23 = numArray2[index16 + 2] + numArray2[index17];
          float num24 = numArray2[index16 + 3] - numArray2[index17 + 1];
          numArray2[index16 + 2] = num23 + num21;
          numArray2[index16 + 3] = num24 + num22;
          numArray2[index17] = num23 - num21;
          numArray2[index17 + 1] = num22 - num24;
          index15 += 4;
          index16 += 4;
        }
        int index18 = this._n2 - 8;
        int index19 = this._n2 - 8;
        int index20 = 0;
        int index21 = this._n2 - 4;
        int n2_1 = this._n2;
        int index22 = this._n - 4;
        while (index19 >= 0)
        {
          float num25 = (float) ((double) numArray1[index19 + 6] * (double) this._b[index18 + 7] - (double) numArray1[index19 + 7] * (double) this._b[index18 + 6]);
          float num26 = (float) (-(double) numArray1[index19 + 6] * (double) this._b[index18 + 6] - (double) numArray1[index19 + 7] * (double) this._b[index18 + 7]);
          buffer[index20] = num25;
          buffer[index21 + 3] = -num25;
          buffer[n2_1] = num26;
          buffer[index22 + 3] = num26;
          float num27 = (float) ((double) numArray1[index19 + 4] * (double) this._b[index18 + 5] - (double) numArray1[index19 + 5] * (double) this._b[index18 + 4]);
          float num28 = (float) (-(double) numArray1[index19 + 4] * (double) this._b[index18 + 4] - (double) numArray1[index19 + 5] * (double) this._b[index18 + 5]);
          buffer[index20 + 1] = num27;
          buffer[index21 + 2] = -num27;
          buffer[n2_1 + 1] = num28;
          buffer[index22 + 2] = num28;
          float num29 = (float) ((double) numArray1[index19 + 2] * (double) this._b[index18 + 3] - (double) numArray1[index19 + 3] * (double) this._b[index18 + 2]);
          float num30 = (float) (-(double) numArray1[index19 + 2] * (double) this._b[index18 + 2] - (double) numArray1[index19 + 3] * (double) this._b[index18 + 3]);
          buffer[index20 + 2] = num29;
          buffer[index21 + 1] = -num29;
          buffer[n2_1 + 2] = num30;
          buffer[index22 + 1] = num30;
          float num31 = (float) ((double) numArray1[index19] * (double) this._b[index18 + 1] - (double) numArray1[index19 + 1] * (double) this._b[index18]);
          float num32 = (float) (-(double) numArray1[index19] * (double) this._b[index18] - (double) numArray1[index19 + 1] * (double) this._b[index18 + 1]);
          buffer[index20 + 3] = num31;
          buffer[index21] = -num31;
          buffer[n2_1 + 3] = num32;
          buffer[index22] = num32;
          index18 -= 8;
          index19 -= 8;
          index20 += 4;
          n2_1 += 4;
          index21 -= 4;
          index22 -= 4;
        }
      }

      private void step3_iter0_loop(int n, float[] e, int i_off, int k_off)
      {
        int index1 = i_off;
        int index2 = index1 + k_off;
        int index3 = 0;
        for (int index4 = n >> 2; index4 > 0; --index4)
        {
          float num1 = e[index1] - e[index2];
          float num2 = e[index1 - 1] - e[index2 - 1];
          e[index1] += e[index2];
          e[index1 - 1] += e[index2 - 1];
          e[index2] = (float) ((double) num1 * (double) this._a[index3] - (double) num2 * (double) this._a[index3 + 1]);
          e[index2 - 1] = (float) ((double) num2 * (double) this._a[index3] + (double) num1 * (double) this._a[index3 + 1]);
          int index5 = index3 + 8;
          float num3 = e[index1 - 2] - e[index2 - 2];
          float num4 = e[index1 - 3] - e[index2 - 3];
          e[index1 - 2] += e[index2 - 2];
          e[index1 - 3] += e[index2 - 3];
          e[index2 - 2] = (float) ((double) num3 * (double) this._a[index5] - (double) num4 * (double) this._a[index5 + 1]);
          e[index2 - 3] = (float) ((double) num4 * (double) this._a[index5] + (double) num3 * (double) this._a[index5 + 1]);
          int index6 = index5 + 8;
          float num5 = e[index1 - 4] - e[index2 - 4];
          float num6 = e[index1 - 5] - e[index2 - 5];
          e[index1 - 4] += e[index2 - 4];
          e[index1 - 5] += e[index2 - 5];
          e[index2 - 4] = (float) ((double) num5 * (double) this._a[index6] - (double) num6 * (double) this._a[index6 + 1]);
          e[index2 - 5] = (float) ((double) num6 * (double) this._a[index6] + (double) num5 * (double) this._a[index6 + 1]);
          int index7 = index6 + 8;
          float num7 = e[index1 - 6] - e[index2 - 6];
          float num8 = e[index1 - 7] - e[index2 - 7];
          e[index1 - 6] += e[index2 - 6];
          e[index1 - 7] += e[index2 - 7];
          e[index2 - 6] = (float) ((double) num7 * (double) this._a[index7] - (double) num8 * (double) this._a[index7 + 1]);
          e[index2 - 7] = (float) ((double) num8 * (double) this._a[index7] + (double) num7 * (double) this._a[index7 + 1]);
          index3 = index7 + 8;
          index1 -= 8;
          index2 -= 8;
        }
      }

      private void step3_inner_r_loop(int lim, float[] e, int d0, int k_off, int k1)
      {
        int index1 = d0;
        int index2 = index1 + k_off;
        int index3 = 0;
        for (int index4 = lim >> 2; index4 > 0; --index4)
        {
          float num1 = e[index1] - e[index2];
          float num2 = e[index1 - 1] - e[index2 - 1];
          e[index1] += e[index2];
          e[index1 - 1] += e[index2 - 1];
          e[index2] = (float) ((double) num1 * (double) this._a[index3] - (double) num2 * (double) this._a[index3 + 1]);
          e[index2 - 1] = (float) ((double) num2 * (double) this._a[index3] + (double) num1 * (double) this._a[index3 + 1]);
          int index5 = index3 + k1;
          float num3 = e[index1 - 2] - e[index2 - 2];
          float num4 = e[index1 - 3] - e[index2 - 3];
          e[index1 - 2] += e[index2 - 2];
          e[index1 - 3] += e[index2 - 3];
          e[index2 - 2] = (float) ((double) num3 * (double) this._a[index5] - (double) num4 * (double) this._a[index5 + 1]);
          e[index2 - 3] = (float) ((double) num4 * (double) this._a[index5] + (double) num3 * (double) this._a[index5 + 1]);
          int index6 = index5 + k1;
          float num5 = e[index1 - 4] - e[index2 - 4];
          float num6 = e[index1 - 5] - e[index2 - 5];
          e[index1 - 4] += e[index2 - 4];
          e[index1 - 5] += e[index2 - 5];
          e[index2 - 4] = (float) ((double) num5 * (double) this._a[index6] - (double) num6 * (double) this._a[index6 + 1]);
          e[index2 - 5] = (float) ((double) num6 * (double) this._a[index6] + (double) num5 * (double) this._a[index6 + 1]);
          int index7 = index6 + k1;
          float num7 = e[index1 - 6] - e[index2 - 6];
          float num8 = e[index1 - 7] - e[index2 - 7];
          e[index1 - 6] += e[index2 - 6];
          e[index1 - 7] += e[index2 - 7];
          e[index2 - 6] = (float) ((double) num7 * (double) this._a[index7] - (double) num8 * (double) this._a[index7 + 1]);
          e[index2 - 7] = (float) ((double) num8 * (double) this._a[index7] + (double) num7 * (double) this._a[index7 + 1]);
          index3 = index7 + k1;
          index1 -= 8;
          index2 -= 8;
        }
      }

      private void step3_inner_s_loop(
        int n,
        float[] e,
        int i_off,
        int k_off,
        int a,
        int a_off,
        int k0)
      {
        float num1 = this._a[a];
        float num2 = this._a[a + 1];
        float num3 = this._a[a + a_off];
        float num4 = this._a[a + a_off + 1];
        float num5 = this._a[a + a_off * 2];
        float num6 = this._a[a + a_off * 2 + 1];
        float num7 = this._a[a + a_off * 3];
        float num8 = this._a[a + a_off * 3 + 1];
        int index1 = i_off;
        int index2 = index1 + k_off;
        for (int index3 = n; index3 > 0; --index3)
        {
          float num9 = e[index1] - e[index2];
          float num10 = e[index1 - 1] - e[index2 - 1];
          e[index1] += e[index2];
          e[index1 - 1] += e[index2 - 1];
          e[index2] = (float) ((double) num9 * (double) num1 - (double) num10 * (double) num2);
          e[index2 - 1] = (float) ((double) num10 * (double) num1 + (double) num9 * (double) num2);
          float num11 = e[index1 - 2] - e[index2 - 2];
          float num12 = e[index1 - 3] - e[index2 - 3];
          e[index1 - 2] += e[index2 - 2];
          e[index1 - 3] += e[index2 - 3];
          e[index2 - 2] = (float) ((double) num11 * (double) num3 - (double) num12 * (double) num4);
          e[index2 - 3] = (float) ((double) num12 * (double) num3 + (double) num11 * (double) num4);
          float num13 = e[index1 - 4] - e[index2 - 4];
          float num14 = e[index1 - 5] - e[index2 - 5];
          e[index1 - 4] += e[index2 - 4];
          e[index1 - 5] += e[index2 - 5];
          e[index2 - 4] = (float) ((double) num13 * (double) num5 - (double) num14 * (double) num6);
          e[index2 - 5] = (float) ((double) num14 * (double) num5 + (double) num13 * (double) num6);
          float num15 = e[index1 - 6] - e[index2 - 6];
          float num16 = e[index1 - 7] - e[index2 - 7];
          e[index1 - 6] += e[index2 - 6];
          e[index1 - 7] += e[index2 - 7];
          e[index2 - 6] = (float) ((double) num15 * (double) num7 - (double) num16 * (double) num8);
          e[index2 - 7] = (float) ((double) num16 * (double) num7 + (double) num15 * (double) num8);
          index1 -= k0;
          index2 -= k0;
        }
      }

      private void step3_inner_s_loop_ld654(int n, float[] e, int i_off, int base_n)
      {
        float num1 = this._a[base_n >> 3];
        int z = i_off;
        for (int index = z - 16 * n; z > index; z -= 16)
        {
          float num2 = e[z] - e[z - 8];
          float num3 = e[z - 1] - e[z - 9];
          e[z] += e[z - 8];
          e[z - 1] += e[z - 9];
          e[z - 8] = num2;
          e[z - 9] = num3;
          float num4 = e[z - 2] - e[z - 10];
          float num5 = e[z - 3] - e[z - 11];
          e[z - 2] += e[z - 10];
          e[z - 3] += e[z - 11];
          e[z - 10] = (num4 + num5) * num1;
          e[z - 11] = (num5 - num4) * num1;
          float num6 = e[z - 12] - e[z - 4];
          float num7 = e[z - 5] - e[z - 13];
          e[z - 4] += e[z - 12];
          e[z - 5] += e[z - 13];
          e[z - 12] = num7;
          e[z - 13] = num6;
          float num8 = e[z - 14] - e[z - 6];
          float num9 = e[z - 7] - e[z - 15];
          e[z - 6] += e[z - 14];
          e[z - 7] += e[z - 15];
          e[z - 14] = (num8 + num9) * num1;
          e[z - 15] = (num8 - num9) * num1;
          this.iter_54(e, z);
          this.iter_54(e, z - 8);
        }
      }

      private void iter_54(float[] e, int z)
      {
        float num1 = e[z] - e[z - 4];
        float num2 = e[z] + e[z - 4];
        float num3 = e[z - 2] + e[z - 6];
        float num4 = e[z - 2] - e[z - 6];
        e[z] = num2 + num3;
        e[z - 2] = num2 - num3;
        float num5 = e[z - 3] - e[z - 7];
        e[z - 4] = num1 + num5;
        e[z - 6] = num1 - num5;
        float num6 = e[z - 1] - e[z - 5];
        float num7 = e[z - 1] + e[z - 5];
        float num8 = e[z - 3] + e[z - 7];
        e[z - 1] = num7 + num8;
        e[z - 3] = num7 - num8;
        e[z - 5] = num6 - num4;
        e[z - 7] = num6 + num4;
      }
    }
  }
}
