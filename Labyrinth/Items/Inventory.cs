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
        public bool HasItems => _items.Any();

        /// <summary>
        /// Gets the type of the item in the room.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the room has no item (check with <see cref="HasItems"/>).</exception>
        public IEnumerable<Type> ItemTypes => HasItems ? _items.Select(i => i.GetType()) : throw new InvalidOperationException("No items in the room");

        /// <summary>
        /// Places an item in the inventory, removing it from another one.
        /// </summary>
        /// <param name="from">The inventory from which the item is taken. The item is removed from this inventory.</param>
        /// <exception cref="InvalidOperationException">Thrown if the room already contains an item (check with <see cref="HasItems"/>).</exception>
        public void MoveItemFrom(Inventory from, int nth = 0)
        {
            if (!from.HasItems)
                throw new InvalidOperationException("No items to take from the source inventory");

            if (nth < 0 || nth >= from._items.Count)
                throw new InvalidOperationException($"Source inventory does not contain item #{nth}");

            var item = from._items.ElementAt(nth);
            from._items.RemoveAt(nth);
            _items.Add(item);
        }

        /// <summary>
        /// Swaps items between inventories (if any)
        /// </summary>
        /// <param name="from">The inventory to swap item from</param>
        protected readonly List<ICollectable> _items = item is null ? new List<ICollectable>() : new List<ICollectable> { item };
    }
}
