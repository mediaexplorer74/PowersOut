
// Type: BlueJay.Component.System.QueryEnumerator
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
  internal class QueryEnumerator : IEnumerator<IEntity>, IEnumerator, IDisposable
  {
    private readonly ILayers _layers;
    private readonly AddonKey _key;
    private List<string> _currentLayers;
    private int _index;
    private int _layerIndex;

    public QueryEnumerator(
      ILayers layers,
      AddonKey key,
      List<string>? filterOnLayers,
      List<string>? layersToExclude)
    {
      this._layers = layers;
      this._key = key;
      this._index = -1;
      this._layerIndex = 0;
      this._currentLayers = Enumerable.ToList<string>(Enumerable.Select<ILayer, string>((IEnumerable<ILayer>) Enumerable.OrderBy<ILayer, int>(Enumerable.Where<ILayer>(Enumerable.Where<ILayer>((IEnumerable<ILayer>) layers, (Func<ILayer, bool>) (x => filterOnLayers == null || filterOnLayers.Count == 0 || filterOnLayers.Contains(x.Id))), (Func<ILayer, bool>) (x => layersToExclude == null || !layersToExclude.Contains(x.Id))), (Func<ILayer, int>) (x => x.Weight)), (Func<ILayer, string>) (x => x.Id)));
    }

    public bool MoveNext()
    {
      if (this._currentLayers.Count == 0)
        return false;
      ILayer layer;
      do
      {
        ++this._index;
        if (this._layers[this._currentLayers[this._layerIndex]].Count <= this._index)
        {
          this._index = 0;
          ++this._layerIndex;
        }
        if (this._currentLayers.Count <= this._layerIndex)
          return false;
        layer = this._layers[this._currentLayers[this._layerIndex]];
      }
      while (layer.Count <= 0 || !layer[this._index].MatchKey(this._key));
      return true;
    }

    public void Reset()
    {
      this._currentLayers = Enumerable.ToList<string>(Enumerable.Select<ILayer, string>((IEnumerable<ILayer>) Enumerable.OrderBy<ILayer, int>((IEnumerable<ILayer>) this._layers, (Func<ILayer, int>) (x => x.Weight)), (Func<ILayer, string>) (x => x.Id)));
      this._layerIndex = 0;
      this._index = -1;
    }

    public IEntity Current => this._layers[this._currentLayers[this._layerIndex]][this._index];

    object IEnumerator.Current => (object) this.Current;

    public void Dispose()
    {
    }
  }
}
