
// Type: BlueJay.Component.System.AddonKey
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace BlueJay.Component.System
{
  public struct AddonKey : IEquatable<AddonKey>
  {
    private static ulong _lastBit = 1073741824;
    private readonly ulong[] _key;

    private AddonKey(params ulong[] key) => this._key = key;

    public static AddonKey operator &(AddonKey left, AddonKey right)
    {
      ulong[] numArray = new ulong[Math.Max(left._key.Length, right._key.Length)];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = (ulong) ((left._key.Length > index ? (long) left._key[index] : 0L) & (right._key.Length > index ? (long) right._key[index] : 0L));
      return new AddonKey(numArray);
    }

    public static AddonKey operator |(AddonKey left, AddonKey right)
    {
      ulong[] numArray = new ulong[Math.Max(left._key.Length, right._key.Length)];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = (ulong) ((left._key.Length > index ? (long) left._key[index] : 0L) | (right._key.Length > index ? (long) right._key[index] : 0L));
      return new AddonKey(numArray);
    }

    public static bool operator ==(AddonKey left, AddonKey right)
    {
      if (left.IsNone() && right.IsNone())
        return true;
      if (left._key.Length != right._key.Length)
        return false;
      for (int index = 0; index < left._key.Length; ++index)
      {
        if ((long) left._key[index] != (long) right._key[index])
          return false;
      }
      return true;
    }

    public bool IsNone()
    {
      for (int index = 0; index < this._key.Length; ++index)
      {
        if (this._key[index] != 0UL)
          return false;
      }
      return true;
    }

    public static bool operator !=(AddonKey left, AddonKey right) => !(left == right);

    public bool Equals(AddonKey other) => this == other;

    public override bool Equals(object? obj)
    {
        return obj is AddonKey other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        int hashCode = 0;
        for (int index = 0; index < this._key.Length; ++index)
            hashCode = CombineHashCodes(this._key[index], hashCode);
        return hashCode;
    }

    private static int CombineHashCodes(ulong keyPart, int currentHash)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + keyPart.GetHashCode();
            hash = hash * 23 + currentHash;
            return hash;
        }
    }

    internal AddonKey IncrementKey()
    {
      ulong[] numArray = new ulong[this._key.Length];
      Array.Copy((Array) this._key, (Array) numArray, this._key.Length);
      if ((long) numArray[numArray.Length - 1] == (long) AddonKey._lastBit)
      {
        numArray[numArray.Length - 1] = 0UL;
        Array.Resize<ulong>(ref numArray, this._key.Length + 1);
        numArray[numArray.Length - 1] = 1UL;
        return new AddonKey(numArray);
      }
      numArray[numArray.Length - 1] <<= 1;
      return new AddonKey(numArray);
    }

    public static implicit operator AddonKey(ulong val)
    {
      return new AddonKey(new ulong[1]{ val });
    }

    public static implicit operator AddonKey(uint val)
    {
      return new AddonKey(new ulong[1]{ (ulong) val });
    }

    public static AddonKey One
    {
      get => new AddonKey(new ulong[1]{ 1UL });
    }

    public static AddonKey None => new AddonKey(new ulong[1]);
  }
}
