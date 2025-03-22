
// Type: BlueJay.Component.System.Query`2
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Interfaces;
using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System
{
  internal class Query<A1, A2>(ILayers layers) : 
    Query(layers, KeyHelper.Create<A1, A2>()),
    IQuery<A1, A2>,
    IQuery,
    IEnumerable<IEntity>,
    IEnumerable
    where A1 : struct, IAddon
    where A2 : struct, IAddon
  {
  }
}
