using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory of collectable items for rooms and players.
    /// </summary>
    /// <param name="Items">Optional initial Items in the inventory.</param>
    public abstract class Inventory(ICollectable? Items = null)
    {
        /// <summary>
        /// True if the inventory contains one or more items, false otherwise.
        /// </summary>
        [MemberNotNullWhen(true, nameof(_items))]
        public bool HasItems => _items.Count > 0;

        /// <summary>
        /// Gets the types of the items in the inventory.
        /// </summary>
        public IEnumerable<Type> ItemTypes => _items.Select(i => i.GetType());

        /// <summary>
        /// Moves the nth Items (default: first) from another inventory to this one.
        /// </summary>
        /// <param name="from">The inventory from which the Items is taken. The Items is removed from that inventory.</param>
        /// <param name="nth">Index of the Items to take (0-based).</param>
        /// <exception cref="InvalidOperationException">Thrown if there is no Items to move.</exception>
        [MemberNotNull(nameof(_items))]
        public void MoveItemFrom(Inventory from, int nth = 0)
        {
            if (!from.HasItems)
            {
                throw new InvalidOperationException("No Items to take from the source inventory.");
            }

            if (nth < 0 || nth >= from._items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(nth), "Invalid Items index.");
            }

            var Items = from._items[nth];
            _items.Add(Items);
            from._items.RemoveAt(nth);
        }

        /// <summary>
        /// Swaps all items between inventories.
        /// </summary>
        /// <param name="from">The inventory to swap items with.</param>
        public void SwapItems(Inventory from)
        {
            var tmp = _items;
            _items = from._items;
            from._items = tmp;
        }

        /// <summary>
        /// Internal list of collectable items.
        /// </summary>
        protected List<ICollectable> _items = Items != null ? new List<ICollectable> { Items } : new List<ICollectable>();
    }
}
