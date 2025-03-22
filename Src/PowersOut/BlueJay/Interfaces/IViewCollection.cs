
// Type: BlueJay.Interfaces.IViewCollection
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Modded by [M]edia[E]xplorer

#nullable enable
namespace BlueJay.Interfaces
{
  public interface IViewCollection
  {
    IView? Current { get; }

    T SetCurrent<T>() where T : IView;
  }
}
