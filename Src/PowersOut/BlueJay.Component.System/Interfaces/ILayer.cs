
// Type: BlueJay.Component.System.Interfaces.ILayer
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System.Interfaces
{
  public interface ILayer : IEnumerable<IEntity>, IEnumerable
  {
    string Id { get; }

    int Weight { get; }

    int Count { get; }

    IEntity? this[int id] { get; }

    void Add(IEntity item);

    void Remove(IEntity item);

    void Clear();
  }
}
