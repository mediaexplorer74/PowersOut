
// Type: GameManager.Views.TitleView
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay;
using BlueJay.Common.Systems;
using BlueJay.Views;
using Microsoft.Xna.Framework;
using GameManager.Systems;
using System;

#nullable enable
namespace GameManager.Views
{
  public class TitleView : View
  {
    protected override void ConfigureProvider(IServiceProvider serviceProvider)
    {
      serviceProvider.AddSystem<ClearSystem>((object) Color.Black);
      serviceProvider.AddSystem<TitleSystem>();
    }
  }
}
