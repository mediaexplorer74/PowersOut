
// Type: NVorbis.Ogg.PageReaderBase
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts.Ogg;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NVorbis.Ogg
{
  internal abstract class PageReaderBase : IPageReader, IDisposable
  {
    private readonly ICrc _crc = PageReaderBase.CreateCrc();
    private readonly HashSet<int> _ignoredSerials = new HashSet<int>();
    private readonly byte[] _headerBuf = new byte[305];
    private byte[] _overflowBuf;
    private int _overflowBufIndex;
    private Stream _stream;
    private bool _closeOnDispose;

    internal static Func<ICrc> CreateCrc { get; set; } = (Func<ICrc>) (() => (ICrc) new Crc());

    protected PageReaderBase(Stream stream, bool closeOnDispose)
    {
      this._stream = stream;
      this._closeOnDispose = closeOnDispose;
    }

    protected long StreamPosition
    {
      get => (this._stream ?? throw new ObjectDisposedException(nameof (PageReaderBase))).Position;
    }

    public long ContainerBits { get; private set; }

    public long WasteBits { get; private set; }

    private bool VerifyPage(
      byte[] headerBuf,
      int index,
      int cnt,
      out byte[] pageBuf,
      out int bytesRead)
    {
      byte num = headerBuf[index + 26];
      if (cnt - index < index + 27 + (int) num)
      {
        pageBuf = (byte[]) null;
        bytesRead = 0;
        return false;
      }
      int count = 0;
      for (int index1 = 0; index1 < (int) num; ++index1)
        count += (int) headerBuf[index + index1 + 27];
      pageBuf = new byte[count + (int) num + 27];
      Buffer.BlockCopy((Array) headerBuf, index, (Array) pageBuf, 0, (int) num + 27);
      bytesRead = this.EnsureRead(pageBuf, (int) num + 27, count);
      if (bytesRead != count)
        return false;
      int length = pageBuf.Length;
      this._crc.Reset();
      int index2;
      for (index2 = 0; index2 < 22; ++index2)
        this._crc.Update((int) pageBuf[index2]);
      this._crc.Update(0);
      this._crc.Update(0);
      this._crc.Update(0);
      this._crc.Update(0);
      for (int index3 = index2 + 4; index3 < length; ++index3)
        this._crc.Update((int) pageBuf[index3]);
      return this._crc.Test(BitConverter.ToUInt32(pageBuf, 22));
    }

    private bool AddPage(byte[] pageBuf, bool isResync)
    {
      int int32 = BitConverter.ToInt32(pageBuf, 14);
      if (!this._ignoredSerials.Contains(int32))
      {
        if (this.AddPage(int32, pageBuf, isResync))
        {
          this.ContainerBits += (long) (8 * (27 + (int) pageBuf[26]));
          return true;
        }
        this._ignoredSerials.Add(int32);
      }
      return false;
    }

    private void EnqueueData(byte[] buf, int count)
    {
      if (this._overflowBuf != null)
      {
        byte[] numArray = new byte[this._overflowBuf.Length - this._overflowBufIndex + count];
        Buffer.BlockCopy((Array) this._overflowBuf, this._overflowBufIndex, (Array) numArray, 0, numArray.Length - count);
        int num = buf.Length - count;
        Buffer.BlockCopy((Array) buf, num, (Array) numArray, numArray.Length - count, count);
        this._overflowBufIndex = 0;
      }
      else
      {
        this._overflowBuf = buf;
        this._overflowBufIndex = buf.Length - count;
      }
    }

    private void ClearEnqueuedData(int count)
    {
      if (this._overflowBuf == null || (this._overflowBufIndex += count) < this._overflowBuf.Length)
        return;
      this._overflowBuf = (byte[]) null;
    }

    private int FillHeader(byte[] buf, int index, int count, int maxTries = 10)
    {
      int num = 0;
      if (this._overflowBuf != null)
      {
        num = Math.Min(this._overflowBuf.Length - this._overflowBufIndex, count);
        Buffer.BlockCopy((Array) this._overflowBuf, this._overflowBufIndex, (Array) buf, index, num);
        index += num;
        count -= num;
        if ((this._overflowBufIndex += num) == this._overflowBuf.Length)
          this._overflowBuf = (byte[]) null;
      }
      if (count > 0)
        num += this.EnsureRead(buf, index, count, maxTries);
      return num;
    }

    private bool VerifyHeader(byte[] buffer, int index, ref int cnt, bool isFromReadNextPage)
    {
      if (buffer[index] == (byte) 79 && buffer[index + 1] == (byte) 103 && buffer[index + 2] == (byte) 103 && buffer[index + 3] == (byte) 83)
      {
        if (cnt < 27)
        {
          if (isFromReadNextPage)
            cnt += this.FillHeader(buffer, 27 - cnt + index, 27 - cnt);
          else
            cnt += this.EnsureRead(buffer, 27 - cnt + index, 27 - cnt);
        }
        if (cnt >= 27)
        {
          byte count = buffer[index + 26];
          if (isFromReadNextPage)
            cnt += this.FillHeader(buffer, index + 27, (int) count);
          else
            cnt += this.EnsureRead(buffer, index + 27, (int) count);
          if (cnt == index + 27 + (int) count)
            return true;
        }
      }
      return false;
    }

    protected int EnsureRead(byte[] buf, int index, int count, int maxTries = 10)
    {
      int num1 = 0;
      int num2 = 0;
      do
      {
        int num3 = this._stream.Read(buf, index + num1, count - num1);
        if (num3 != 0 || ++num2 != maxTries)
          num1 += num3;
        else
          break;
      }
      while (num1 < count);
      return num1;
    }

    protected bool VerifyHeader(byte[] buffer, int index, ref int cnt)
    {
      return this.VerifyHeader(buffer, index, ref cnt, false);
    }

    protected long SeekStream(long offset)
    {
      if (!this.CheckLock())
        throw new InvalidOperationException("Must be locked prior to reading!");
      return this._stream.Seek(offset, (SeekOrigin) 0);
    }

    protected virtual void PrepareStreamForNextPage()
    {
    }

    protected virtual void SaveNextPageSearch()
    {
    }

    protected abstract bool AddPage(int streamSerial, byte[] pageBuf, bool isResync);

    protected abstract void SetEndOfStreams();

    public virtual void Lock()
    {
    }

    protected virtual bool CheckLock() => true;

    public virtual bool Release() => false;

    public bool ReadNextPage()
    {
      if (!this.CheckLock())
        throw new InvalidOperationException("Must be locked prior to reading!");
      bool isResync = false;
      int index1 = 0;
      this.PrepareStreamForNextPage();
      int num;
      while ((num = this.FillHeader(this._headerBuf, index1, 27 - index1)) > 0)
      {
        int cnt = num + index1;
        for (int index2 = 0; index2 < cnt - 4; ++index2)
        {
          if (this.VerifyHeader(this._headerBuf, index2, ref cnt, true))
          {
            byte[] pageBuf;
            int bytesRead;
            if (this.VerifyPage(this._headerBuf, index2, cnt, out pageBuf, out bytesRead))
            {
              this.ClearEnqueuedData(bytesRead);
              this.SaveNextPageSearch();
              if (this.AddPage(pageBuf, isResync))
                return true;
              this.WasteBits += (long) (pageBuf.Length * 8);
              index1 = 0;
              cnt = 0;
              break;
            }
            if (pageBuf != null)
              this.EnqueueData(pageBuf, bytesRead);
          }
          this.WasteBits += 8L;
          isResync = true;
        }
        if (cnt >= 3)
        {
          this._headerBuf[0] = this._headerBuf[cnt - 3];
          this._headerBuf[1] = this._headerBuf[cnt - 2];
          this._headerBuf[2] = this._headerBuf[cnt - 1];
          index1 = 3;
        }
      }
      if (num == 0)
        this.SetEndOfStreams();
      return false;
    }

    public abstract bool ReadPageAt(long offset);

    public void Dispose()
    {
      this.SetEndOfStreams();
      if (this._closeOnDispose)
        this._stream?.Dispose();
      this._stream = (Stream) null;
    }
  }
}
