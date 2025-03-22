
// Type: NVorbis.NewStreamEventArgs
// Assembly: NVorbis, Version=0.10.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 29622A92-8C01-4F05-9B21-EB74E2D703B8
// Assembly location: NVorbis.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using NVorbis.Contracts;
using System;

#nullable disable
namespace NVorbis
{
  [Serializable]
  public class NewStreamEventArgs : EventArgs
  {
    public NewStreamEventArgs(IStreamDecoder streamDecoder)
    {
      this.StreamDecoder = streamDecoder ?? throw new ArgumentNullException(nameof (streamDecoder));
    }

    public IStreamDecoder StreamDecoder { get; }

    public bool IgnoreStream { get; set; }
  }
}
