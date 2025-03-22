
// Type: BlueJay.Component.System.Query`9
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Interfaces;
using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System
{
  internal class Query<A1, A2, A3, A4, A5, A6, A7, A8, A9>(ILayers layers) : 
    Query(layers, KeyHelper.Create<A1, A2, A3, A4, A5, A6, A7, A8, A9>()),
    IQuery<A1, A2, A3, A4, A5, A6, A7, A8, A9>,
    IQuery,
    IEnumerable<IEntity>,
    IEnumerable
    where A1 : struct, IAddon
    where A2 : struct, IAddon
    where A3 : struct, IAddon
    where A4 : struct, IAddon
    where A5 : struct, IAddon
    where A6 : struct, IAddon
    where A7 : struct, IAddon
    where A8 : struct, IAddon
    where A9 : struct, IAddon
  {
  }
}
