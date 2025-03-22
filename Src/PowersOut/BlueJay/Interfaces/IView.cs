
// Type: BlueJay.Interfaces.IView
// Assembly: BlueJay, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9FA4A925-7A7F-4A78-9E85-0E58164C5FDF
// Modded by [M]edia[E]xplorer

using System;

#nullable enable
namespace BlueJay.Interfaces
{
  public interface IView
  {
    void Initialize(IServiceProvider serviceProvider);

    void Enter();

    void Leave();

    void Draw();

    void Update();

    void Activate();

    void Deactivate();

    void Exit();
  }
}
