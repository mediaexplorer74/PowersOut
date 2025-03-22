
// Type: BlueJay.Component.System.Layer
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace BlueJay.Component.System
{
  internal class Layer : ILayer, IEnumerable<IEntity>, IEnumerable
  {
    private List<IEntity> _collection;

    public string Id { get; private set; }

    public int Weight { get; private set; }

    public int Count => this._collection.Count;

    public IEntity this[int index] => this._collection[index];

    public Layer(string id, int weight)
    {
      this.Id = id;
      this.Weight = weight;
      this._collection = new List<IEntity>();
    }

    public void Add(IEntity item)
    {
      this._collection.Add(item);
      this.Sort();
    }

    public void Remove(IEntity item) => this._collection.Remove(item);

    public void Clear() => this._collection.Clear();

    public IEnumerator<IEntity> GetEnumerator()
    {
      return (IEnumerator<IEntity>) this._collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._collection.GetEnumerator();

    private void Sort()
    {
      this._collection = Enumerable.ToList<IEntity>((IEnumerable<IEntity>) Enumerable.OrderBy<IEntity, int>((IEnumerable<IEntity>) this._collection, (Func<IEntity, int>) (x => x.Weight)));
    }
  }
}
