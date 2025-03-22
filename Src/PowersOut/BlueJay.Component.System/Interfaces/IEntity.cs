
// Type: BlueJay.Component.System.Interfaces.IEntity
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Component.System.Interfaces
{
  public interface IEntity : IDisposable
  {
    long Id { get; set; }

    bool Active { get; set; }

    string Layer { get; set; }

    int Weight { get; set; }

    bool Add<T>(T addon) where T : struct, IAddon;

    bool Remove<T>(T addon) where T : struct, IAddon;

    bool Remove<T>() where T : struct, IAddon;

    bool Update<T>(T addon) where T : struct, IAddon;

    bool Upsert<T>(T addon) where T : struct, IAddon;

    T GetAddon<T>() where T : struct, IAddon;

    bool TryGetAddon<T>(out T addon) where T : struct, IAddon;

    IEnumerable<IAddon> GetAddons(AddonKey key);

    bool MatchKey(AddonKey key);

    bool Contains<A1>() where A1 : struct, IAddon;

    bool Contains<A1, A2>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon;

    bool Contains<A1, A2, A3>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon;

    bool Contains<A1, A2, A3, A4>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon;

    bool Contains<A1, A2, A3, A4, A5>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon;

    bool Contains<A1, A2, A3, A4, A5, A6>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon;

    bool Contains<A1, A2, A3, A4, A5, A6, A7>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon
      where A7 : struct, IAddon;

    bool Contains<A1, A2, A3, A4, A5, A6, A7, A8>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon
      where A7 : struct, IAddon
      where A8 : struct, IAddon;

    bool Contains<A1, A2, A3, A4, A5, A6, A7, A8, A9>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon
      where A7 : struct, IAddon
      where A8 : struct, IAddon
      where A9 : struct, IAddon;

    bool Contains<A1, A2, A3, A4, A5, A6, A7, A8, A9, A10>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon
      where A7 : struct, IAddon
      where A8 : struct, IAddon
      where A9 : struct, IAddon
      where A10 : struct, IAddon;
  }
}
