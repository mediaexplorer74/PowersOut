
// Type: BlueJay.Component.System.Interfaces.IQuery`5
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System.Interfaces
{
  public interface IQuery<A1, A2, A3, A4, A5> : IQuery, IEnumerable<IEntity>, IEnumerable
    where A1 : struct, IAddon
    where A2 : struct, IAddon
    where A3 : struct, IAddon
    where A4 : struct, IAddon
    where A5 : struct, IAddon
  {
  }
}
