
// Type: GameManager.Addon.DirectionAddon
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using System.Runtime.CompilerServices;

#nullable enable
namespace GameManager.Addon
{
    public struct DirectionAddon : IAddon
    {
        public Direction Direction { get; set; }

        public DirectionAddon(Direction direction)
        {
            Direction = direction;
        }

        public override string ToString()
        {
            return $"Direction: {Direction}";
        }
    }
}
