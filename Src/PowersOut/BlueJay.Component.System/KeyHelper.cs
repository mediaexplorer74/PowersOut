
// Type: BlueJay.Component.System.KeyHelper
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using System;
using System.Collections.Concurrent;

#nullable enable
namespace BlueJay.Component.System
{
  public static class KeyHelper
  {
    private static AddonKey _nextKey = AddonKey.One;
    private static ConcurrentDictionary<Type, AddonKey> _cache = new ConcurrentDictionary<Type, AddonKey>();

    public static AddonKey Create<T1>() where T1 : IAddon => KeyHelper.Create(typeof (T1));

    public static AddonKey Create<T1, T2>()
      where T1 : IAddon
      where T2 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2));
    }

    public static AddonKey Create<T1, T2, T3>()
      where T1 : IAddon
      where T2 : IAddon
      where T3 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2), typeof (T3));
    }

    public static AddonKey Create<T1, T2, T3, T4>()
      where T1 : IAddon
      where T2 : IAddon
      where T3 : IAddon
      where T4 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2), typeof (T3), typeof (T4));
    }

    public static AddonKey Create<T1, T2, T3, T4, T5>()
      where T1 : IAddon
      where T2 : IAddon
      where T3 : IAddon
      where T4 : IAddon
      where T5 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2), typeof (T3), typeof (T4), typeof (T5));
    }

    public static AddonKey Create<T1, T2, T3, T4, T5, T6>()
      where T1 : IAddon
      where T2 : IAddon
      where T3 : IAddon
      where T4 : IAddon
      where T5 : IAddon
      where T6 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2), typeof (T3), typeof (T4), typeof (T5), typeof (T6));
    }

    public static AddonKey Create<T1, T2, T3, T4, T5, T6, T7>()
      where T1 : IAddon
      where T2 : IAddon
      where T3 : IAddon
      where T4 : IAddon
      where T5 : IAddon
      where T6 : IAddon
      where T7 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2), typeof (T3), typeof (T4), typeof (T5), typeof (T6), typeof (T7));
    }

    public static AddonKey Create<T1, T2, T3, T4, T5, T6, T7, T8>()
      where T1 : IAddon
      where T2 : IAddon
      where T3 : IAddon
      where T4 : IAddon
      where T5 : IAddon
      where T6 : IAddon
      where T7 : IAddon
      where T8 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2), typeof (T3), typeof (T4), typeof (T5), typeof (T6), typeof (T7), typeof (T8));
    }

    public static AddonKey Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
      where T1 : IAddon
      where T2 : IAddon
      where T3 : IAddon
      where T4 : IAddon
      where T5 : IAddon
      where T6 : IAddon
      where T7 : IAddon
      where T8 : IAddon
      where T9 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2), typeof (T3), typeof (T4), typeof (T5), typeof (T6), typeof (T7), typeof (T8), typeof (T9));
    }

    public static AddonKey Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
      where T1 : IAddon
      where T2 : IAddon
      where T3 : IAddon
      where T4 : IAddon
      where T5 : IAddon
      where T6 : IAddon
      where T7 : IAddon
      where T8 : IAddon
      where T9 : IAddon
      where T10 : IAddon
    {
      return KeyHelper.Create(typeof (T1), typeof (T2), typeof (T3), typeof (T4), typeof (T5), typeof (T6), typeof (T7), typeof (T8), typeof (T9), typeof (T10));
    }

    public static AddonKey Create(params Type[] types)
    {
      AddonKey none = AddonKey.None;
      for (int index = 0; index < types.Length; ++index)
      {
        if (typeof (IAddon).IsAssignableFrom(types[index]))
          none |= KeyHelper.GetIdentifier(types[index]);
      }
      return none;
    }

    private static AddonKey GetIdentifier(Type key)
    {
      if (!KeyHelper._cache.ContainsKey(key))
        KeyHelper._cache[key] = KeyHelper.NextKey();
      return KeyHelper._cache[key];
    }

    private static AddonKey NextKey()
    {
      AddonKey nextKey = KeyHelper._nextKey;
      KeyHelper._nextKey = KeyHelper._nextKey.IncrementKey();
      return nextKey;
    }

    internal static void SetNext(AddonKey key)
    {
      KeyHelper._nextKey = key;
      KeyHelper._cache.Clear();
    }
  }
}
