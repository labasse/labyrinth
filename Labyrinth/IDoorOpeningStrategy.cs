using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth
{
    /// <summary>
    /// Strategy used to try opening a door using items from an inventory.
    /// </summary>
    public interface IDoorOpeningStrategy
    {
        /// <summary>
        /// Tries to open the specified door using items contained in <paramref name="bag"/>.
        /// </summary>
        /// <param name="door">The door to open.</param>
        /// <param name="bag">The explorer's inventory.</param>
        void TryOpen(Door door, MyInventory bag);
    }
}
