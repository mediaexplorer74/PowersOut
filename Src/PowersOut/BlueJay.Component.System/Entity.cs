
// Type: BlueJay.Component.System.Entity
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Events;
using BlueJay.Component.System.Interfaces;
using BlueJay.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace BlueJay.Component.System
{
  public class Entity : IEntity, IDisposable
  {
    private readonly IEventQueue _eventQueue;
    private IAddon[] _addons;
    private AddonKey[] _addonKeys;
    private AddonKey _addonsId;

    public long Id { get; set; }

    public bool Active { get; set; }

    public string Layer { get; set; }

    public int Weight { get; set; }

    public Entity(IEventQueue eventQueue)
    {
      this._eventQueue = eventQueue;
      this._addonsId = AddonKey.None;
      this._addons = Array.Empty<IAddon>();
      this._addonKeys = Array.Empty<AddonKey>();
      this.Active = true;
      this.Layer = string.Empty;
    }

    public bool Add<T>(T addon) where T : struct, IAddon
    {
      if (!((KeyHelper.Create<T>() & this._addonsId) == AddonKey.None))
        return false;
      Array.Resize<IAddon>(ref this._addons, this._addons.Length + 1);
      Array.Resize<AddonKey>(ref this._addonKeys, this._addonKeys.Length + 1);
      this._addons[this._addons.Length - 1] = (IAddon) addon;
      this._addonKeys[this._addons.Length - 1] = KeyHelper.Create(addon.GetType());
      this._addonsId |= this._addonKeys[this._addons.Length - 1];
      this._eventQueue.DispatchEvent<AddAddonEvent>(new AddAddonEvent((IAddon) addon), (object) this);
      return true;
    }

    public bool Remove<T>(T addon) where T : struct, IAddon => this.Remove<T>();

    public bool Remove<T>() where T : struct, IAddon
    {
      int index1 = Array.IndexOf<AddonKey>(this._addonKeys, KeyHelper.Create<T>());
      if (index1 == -1)
        return false;
      IAddon addon = this._addons[index1];
      for (int index2 = index1 + 1; index2 < this._addons.Length; ++index2)
      {
        this._addons[index2 - 1] = this._addons[index2];
        this._addonKeys[index2 - 1] = this._addonKeys[index2];
      }
      Array.Resize<IAddon>(ref this._addons, this._addons.Length - 1);
      Array.Resize<AddonKey>(ref this._addonKeys, this._addonKeys.Length - 1);
      this._addonsId = KeyHelper.Create(Enumerable.ToArray<Type>(Enumerable.Select<IAddon, Type>((IEnumerable<IAddon>) this._addons, (Func<IAddon, Type>) (x => x.GetType()))));
      this._eventQueue.DispatchEvent<RemoveAddonEvent>(new RemoveAddonEvent(addon), (object) this);
      return true;
    }

    public bool Update<T>(T addon) where T : struct, IAddon
    {
      int index = Array.IndexOf<AddonKey>(this._addonKeys, KeyHelper.Create<T>());
      if (index == -1)
        return false;
      this._addons[index] = (IAddon) addon;
      this._eventQueue.DispatchEvent<UpdateAddonEvent>(new UpdateAddonEvent((IAddon) addon), (object) this);
      return true;
    }

    public bool Upsert<T>(T addon) where T : struct, IAddon
    {
      if (!this.Update<T>(addon))
        this.Add<T>(addon);
      return true;
    }

    public bool MatchKey(AddonKey key) => (this._addonsId & key) == key;

    public IEnumerable<IAddon> GetAddons(AddonKey key)
    {
      IAddon[] addonArray = this._addons;
      for (int index = 0; index < addonArray.Length; ++index)
      {
        IAddon addon = addonArray[index];
        AddonKey addonKey = KeyHelper.Create(addon.GetType());
        if ((key & addonKey) == addonKey)
          yield return addon;
      }
      addonArray = (IAddon[]) null;
    }

    public T GetAddon<T>() where T : struct, IAddon
    {
      AddonKey addonKey = KeyHelper.Create<T>();
      for (int index = 0; index < this._addons.Length; ++index)
      {
        if (KeyHelper.Create(this._addons[index].GetType()) == addonKey)
          return (T) this._addons[index];
      }
      return default (T);
    }

    public bool TryGetAddon<T>(out T addon) where T : struct, IAddon
    {
      addon = default (T);
      if (!this.Contains<T>())
        return false;
      addon = this.GetAddon<T>();
      return true;
    }

    public virtual void Dispose()
    {
    }

    public bool Contains<A1>() where A1 : struct, IAddon => this.MatchKey(KeyHelper.Create<A1>());

    public bool Contains<A1, A2>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
    {
      return this.MatchKey(KeyHelper.Create<A1, A2>());
    }

    public bool Contains<A1, A2, A3>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
    {
      return this.MatchKey(KeyHelper.Create<A1, A2, A3>());
    }

    public bool Contains<A1, A2, A3, A4>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
    {
      return this.MatchKey(KeyHelper.Create<A1, A2, A3, A4>());
    }

    public bool Contains<A1, A2, A3, A4, A5>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
    {
      return this.MatchKey(KeyHelper.Create<A1, A2, A3, A4, A5>());
    }

    public bool Contains<A1, A2, A3, A4, A5, A6>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon
    {
      return this.MatchKey(KeyHelper.Create<A1, A2, A3, A4, A5, A6>());
    }

    public bool Contains<A1, A2, A3, A4, A5, A6, A7>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon
      where A7 : struct, IAddon
    {
      return this.MatchKey(KeyHelper.Create<A1, A2, A3, A4, A5, A6, A7>());
    }

    public bool Contains<A1, A2, A3, A4, A5, A6, A7, A8>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon
      where A7 : struct, IAddon
      where A8 : struct, IAddon
    {
      return this.MatchKey(KeyHelper.Create<A1, A2, A3, A4, A5, A6, A7, A8>());
    }

    public bool Contains<A1, A2, A3, A4, A5, A6, A7, A8, A9>()
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
      return this.MatchKey(KeyHelper.Create<A1, A2, A3, A4, A5, A6, A7, A8, A9>());
    }

    public bool Contains<A1, A2, A3, A4, A5, A6, A7, A8, A9, A10>()
      where A1 : struct, IAddon
      where A2 : struct, IAddon
      where A3 : struct, IAddon
      where A4 : struct, IAddon
      where A5 : struct, IAddon
      where A6 : struct, IAddon
      where A7 : struct, IAddon
      where A8 : struct, IAddon
      where A9 : struct, IAddon
      where A10 : struct, IAddon
    {
      return this.MatchKey(KeyHelper.Create<A1, A2, A3, A4, A5, A6, A7, A8, A9, A10>());
    }
  }
}
