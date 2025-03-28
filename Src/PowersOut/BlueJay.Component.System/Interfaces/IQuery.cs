﻿
// Type: BlueJay.Component.System.Interfaces.IQuery
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System.Interfaces
{
  public interface IQuery : IEnumerable<IEntity>, IEnumerable
  {
    IQuery WhereLayer(params string[] layers);

    IQuery ExcludeLayer(params string[] layers);
  }
}
