
// Type: BlueJay.Common.EnumHelper
// Assembly: BlueJay.Common, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: F91EC822-8C3A-44B6-A30D-08602B749F54
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;

#nullable enable
namespace BlueJay.Common
{
  internal static class EnumHelper
  {
    internal static Dictionary<T, V> GenerateEnumDictionary<T, V>(V defaultValue) where T : notnull
    {
      Dictionary<T, V> enumDictionary = new Dictionary<T, V>();
      foreach (object obj in Enum.GetValues(typeof (T)))
        enumDictionary.Add((T) obj, defaultValue);
      return enumDictionary;
    }
  }
}
