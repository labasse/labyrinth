using System.Diagnostics.CodeAnalysis;

namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory of collectable items for rooms and players.
    /// </summary>
    /// <param name="item">Optional initial items in the inventory.</param>
    public abstract class Inventory(IEnumerable<ICollectable>? items)
    {
        /// <summary>
        /// True if the room has an item, false otherwise.
        /// </summary>
        [MemberNotNullWhen(true, nameof(_items))]
        public bool HasItems => _items != null && _items.Any();

        /// <summary>
        /// Gets the type of the item in the room.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the room has no item (check with <see cref="HasItem"/>).</exception>
        public IEnumerable<Type> ItemTypes => !HasItems
                    ? throw new InvalidOperationException("No item in the room")
                    : _items.Select(x => x.GetType());
        

        /// <summary>
        /// Places an item in the inventory, removing it from another one.
        /// </summary>
        /// <param name="from">The inventory from which the item is taken. The item is removed from this inventory.</param>
        /// <exception cref="InvalidOperationException">Thrown if the room doesn't have any item (check with <see cref="HasItems"/>).</exception>
        /// <exception cref="InvalidOperationException">Thrown if items[nth] is out of range.</exception>
        [MemberNotNull(nameof(_items))]
        public void MoveItemFrom(Inventory from, int nth = 0)
        {
            if (!from.HasItems)
            {
                throw new InvalidOperationException("No item to take from the source inventory");
            }

            if (from._items.Count() < nth + 1)
            {
                throw new IndexOutOfRangeException("nth is out of range");
            }

            _items = _items.Append(from._items.ElementAt(nth)).AsEnumerable();
            from._items = from._items.Where((_, index) => index != nth);
        }

        /// <summary>
        /// Swaps items between inventories (if any)
        /// </summary>
        /// <param name="from">The inventory to swap items from</param>
        public void SwapItems(Inventory from)
        {
            var tmp = _items;

            _items = from._items;
            from._items = tmp;
        }

        protected IEnumerable<ICollectable>? _items = items ?? Enumerable.Empty<ICollectable>();
    }
}
