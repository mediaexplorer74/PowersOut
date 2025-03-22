
// Type: BlueJay.Component.System.Layers
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace BlueJay.Component.System
{
  internal class Layers : ILayers, IEnumerable<ILayer>, IEnumerable
  {
    private readonly IServiceProvider _provider;
    private List<ILayer> _collection;
    private int _size;

    public int Count => this._size;

    public ILayer? this[string id]
    {
      get
      {
        return Enumerable
        .FirstOrDefault<ILayer>((IEnumerable<ILayer>) this._collection, (Func<ILayer, bool>) (x => string.Equals(x.Id, id)));
      }
    }

    public Layers(IServiceProvider provider)
    {
      this._provider = provider;
      this._collection = new List<ILayer>();
    }

    public void Add(IEntity entity, string layer = "", int weight = 0)
    {
      ILayer instance = this[layer];
      if (instance == null)
      {
        instance = (ILayer) ActivatorUtilities.CreateInstance<Layer>(this._provider, (object) layer, (object) weight);
        this.Add(instance);
      }
      instance.Add(entity);
    }

    public void Add(string layer, int weight = 0)
    {
      if (this.Contains(layer))
        return;
      this.Add((ILayer) ActivatorUtilities.CreateInstance<Layer>(this._provider, (object) layer, (object) weight));
    }

    public void Remove(IEntity entity)
    {
      if (!this.Contains(entity.Layer))
        return;
      this[entity.Layer]?.Remove(entity);
    }

    public void Clear()
    {
      for (int index = 0; index < this._size; ++index)
        this._collection[index]?.Clear();
    }

    public bool Contains(string layer)
    {
      return Enumerable.Any<ILayer>((IEnumerable<ILayer>) this._collection, (Func<ILayer, bool>) (x => string.Equals(x?.Id, layer)));
    }

    public IEnumerator<ILayer> GetEnumerator()
    {
      return (IEnumerator<ILayer>) this._collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._collection.GetEnumerator();

    private void Add(ILayer item) => this._collection.Add(item);
  }
}
