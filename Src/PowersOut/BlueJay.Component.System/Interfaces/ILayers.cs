
// Type: BlueJay.Component.System.Interfaces.ILayers
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System.Interfaces
{
  internal interface ILayers : IEnumerable<ILayer>, IEnumerable
  {
    void Add(IEntity entity, string layer = "", int weight = 0);

    void Add(string layer, int weight = 0);

    void Remove(IEntity entity);

    bool Contains(string layer);

    ILayer? this[string id] { get; }

    void Clear();
  }
}
