
// Type: BlueJay.Component.System.Query
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace BlueJay.Component.System
{
  internal class Query : IEnumerable<IEntity>, IEnumerable, IQuery
  {
    private readonly ILayers _layers;
    private readonly AddonKey _key;
    private readonly List<string>? _filterOnLayers;
    private readonly List<string>? _layersToExclude;

    public Query(
      ILayers layers,
      AddonKey key,
      List<string>? filterOnLayers = null,
      List<string>? layersToExclude = null)
    {
      this._layers = layers;
      this._key = key;
      this._filterOnLayers = filterOnLayers;
      this._layersToExclude = layersToExclude;
    }

    public IEnumerator<IEntity> GetEnumerator()
    {
      return (IEnumerator<IEntity>) new QueryEnumerator(this._layers, this._key, this._filterOnLayers, this._layersToExclude);
    }

    public IQuery WhereLayer(params string[] layers)
    {
      return (IQuery) new Query(this._layers, this._key, Enumerable.ToList<string>(Enumerable.Concat<string>((IEnumerable<string>) Enumerable.ToList<string>((IEnumerable<string>) layers), (IEnumerable<string>) (this._filterOnLayers ?? new List<string>()))), this._layersToExclude);
    }

    public IQuery ExcludeLayer(params string[] layers)
    {
      return (IQuery) new Query(this._layers, this._key, this._filterOnLayers, Enumerable.ToList<string>(Enumerable.Concat<string>((IEnumerable<string>) Enumerable.ToList<string>((IEnumerable<string>) layers), (IEnumerable<string>) (this._layersToExclude ?? new List<string>()))));
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
