
// Type: BlueJay.Component.System.Events.RemoveAddonEvent
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;

#nullable enable
namespace BlueJay.Component.System.Events
{
  public sealed class RemoveAddonEvent
  {
    public IAddon Addon { get; set; }

    public RemoveAddonEvent(IAddon addon) => this.Addon = addon;
  }
}
