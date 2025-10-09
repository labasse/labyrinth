using System.Diagnostics.CodeAnalysis;

namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory of collectable items for rooms and players.
    /// </summary>
    /// <param name="item">Optional initial item in the inventory.</param>
    public abstract class Inventory(ICollectable? item = null)
    {
        /// <summary>
        /// True if the room has an item, false otherwise.
        /// </summary>
        [MemberNotNullWhen(true, nameof(Item))]
        public bool HasItem => Item != null;

        /// <summary>
        /// Gets the type of the item in the room.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the room has no item (check with <see cref="HasItem"/>).</exception>
        public Type ItemType => Item?.GetType() ?? throw new InvalidOperationException("No item in the room");

        /// <summary>
        /// Places an item in the inventory, removing it from another one.
        /// </summary>
        /// <param name="from">The inventory from which the item is taken. The item is removed from this inventory.</param>
        /// <exception cref="InvalidOperationException">Thrown if the room already contains an item (check with <see cref="HasItem"/>).</exception>
        [MemberNotNull(nameof(Item))]
        public void MoveItemFrom(Inventory from)
        {
            if (HasItem)
            {
                throw new InvalidOperationException("Room already has an item.");
            }
            if (!from.HasItem)
            {
                throw new InvalidOperationException("No item to take from the source inventory");
            }
            Item = from.Item;
            from.Item = null;
        }

        protected ICollectable? Item = item;
    }
}
