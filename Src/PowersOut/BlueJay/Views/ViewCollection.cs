
// Type: BlueJay.Views.ViewCollection
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Assembly location: BlueJay.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace BlueJay.Views
{
  internal class ViewCollection : IViewCollection
  {
    private readonly IServiceProvider _provider;
    private List<IView> _collection;
    private IView? _current;

    public IView? Current
    {
      get => this._current;
      private set
      {
        this._current?.Leave();
        this._current = value;
        this._current?.Enter();
      }
    }

    public ViewCollection(IServiceProvider provider)
    {
      this._provider = provider;
      this._collection = new List<IView>();
      this._current = (IView) null;
    }

    public T SetCurrent<T>() where T : IView
    {
      IView view = Enumerable.FirstOrDefault<IView>(
          (IEnumerable<IView>) this._collection, (Func<IView, bool>) (x => Type.Equals(x.GetType(), typeof (T))));
      if (view == null)
      {
        view = (IView) ActivatorUtilities.CreateInstance<T>(this._provider);
        view.Initialize(this._provider);
        this._collection.Add(view);
      }
      this.Current = view;
      return (T) view;
    }
  }
}
