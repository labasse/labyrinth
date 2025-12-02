using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory of collectable items for rooms and players.
    /// </summary>
    /// <param name="item">Optional initial item in the inventory.</param>
    public abstract class Inventory(ICollectable? item = null)
    {
        /// <summary>
        /// True if the room has one or more items, false otherwise.
        /// </summary>
        [MemberNotNullWhen(true, nameof(_items))]
        public bool HasItems => _items.Count > 0;

        /// <summary>
        /// Gets the types of the items in the room.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the room has no items (check with <see cref="HasItems"/>).</exception>
        public IEnumerable<Type> ItemTypes =>
            HasItems ? _items.Select(i => i.GetType()) 
                     : throw new InvalidOperationException("No items in the room");

        /// <summary>
        /// Places an item in the inventory, removing it from another one.
        /// </summary>
        /// <param name="from">The inventory from which the item is taken. The item is removed from this inventory.</param>
        /// <param name="nth">Index of the item to take from the source inventory (default: 0).</param>
        /// <exception cref="InvalidOperationException">Thrown if the source has no items.</exception>
        public void MoveItemFrom(Inventory from, int nth = 0)
        {
            if (!from.HasItems)
            {
                throw new InvalidOperationException("No items to take from the source inventory");
            }
            if (nth < 0 || nth >= from._items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(nth));
            }

            _items.Add(from._items[nth]);
            from._items.RemoveAt(nth);
        }

        /// <summary>
        /// Swaps all items between inventories.
        /// </summary>
        /// <param name="from">The inventory to swap items with.</param>
        

        /// <summary>
        /// Internal list of items contained in the inventory.
        /// </summary>
        protected List<ICollectable> _items = item != null ? new List<ICollectable> { item } : new List<ICollectable>();
    }
}
