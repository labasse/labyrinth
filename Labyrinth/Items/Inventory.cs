using System;
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
        /// True if the inventory has at least one item, false otherwise.
        /// </summary>
        public bool HasItems => _items.Count > 0;

        /// <summary>
        /// Gets the types of the items in the inventory.
        /// </summary>
        public IEnumerable<Type> ItemTypes => _items.Select(i => i.GetType());

        /// <summary>
        /// Places an item in the inventory, removing it from another one.
        /// </summary>
        /// <param name="from">
        /// The inventory from which the item is taken. The item is removed from this inventory.
        /// </param>
        /// <param name="nth">
        /// Index (0-based) of the item to take in the source inventory. Default is 0 (first item).
        /// </param>
        public void MoveItemFrom(Inventory from, int nth = 0)
        {
            if (!from.HasItems)
            {
                throw new InvalidOperationException("No item to take from the source inventory.");
            }

            if (nth < 0 || nth >= from._items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(nth));
            }

            var itemToMove = from._items[nth];
            from._items.RemoveAt(nth);
            _items.Add(itemToMove);
        }

        protected readonly List<ICollectable> _items =
            item is null
                ? new List<ICollectable>()
                : new List<ICollectable> { item };
    }
}
