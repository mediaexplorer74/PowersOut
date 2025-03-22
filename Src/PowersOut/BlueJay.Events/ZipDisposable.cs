
// Type: BlueJay.Events.ZipDisposable
// Assembly: BlueJay.Events, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: CAB4819D-7A1F-43CD-8046-4F016C9F7D65
// Modded by [M]edia[E]xplorer

using System;

#nullable enable
namespace BlueJay.Events
{
  internal class ZipDisposable : IDisposable
  {
    private readonly IDisposable[] _disposables;

    public ZipDisposable(params IDisposable[] disposables) => this._disposables = disposables;

    public void Dispose()
    {
      foreach (IDisposable disposable in this._disposables)
        disposable.Dispose();
    }
  }
}
