using System.Diagnostics.CodeAnalysis;

namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory of collectable items for rooms and players.
    /// </summary>
    /// <param name="item">Optional initial item in the inventory.</param>
    public abstract class Inventory(ICollectable? item = null)
    {
        protected List<ICollectable> _items = item != null ? [item] : [];

        /// <summary>
        /// True if the inventory has any items, false otherwise.
        /// </summary>
        public bool HasItems => _items.Count > 0;

        /// <summary>
        /// Gets the types of all items in the inventory.
        /// </summary>
        public IEnumerable<Type> ItemTypes => _items.Select(i => i.GetType());

        /// <summary>
        /// Places an item in the inventory, removing it from another one.
        /// </summary>
        /// <param name="from">The inventory from which the item is taken.</param>
        /// <param name="nth">Index of the item to take from the source inventory (default 0).</param>
        /// <exception cref="InvalidOperationException">Thrown if the source inventory doesn't have enough items or index is invalid.</exception>
        public void MoveItemFrom(Inventory from, int nth = 0)
        {
            if (nth < 0 || nth >= from._items.Count)
            {
                throw new InvalidOperationException($"Invalid item index {nth}. Source inventory has {from._items.Count} items.");
            }

            var itemToMove = from._items[nth];
            from._items.RemoveAt(nth);
            _items.Add(itemToMove);
        }

        /// <summary>
        /// Swaps all items between inventories.
        /// </summary>
        /// <param name="from">The inventory to swap items with.</param>
        public void SwapItems(Inventory from)
        {
            (_items, from._items) = (from._items, _items);
        }
    }
}
