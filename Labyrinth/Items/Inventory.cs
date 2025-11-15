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
        [MemberNotNullWhen(true, nameof(_items))]
        public bool HasItems => _items is { Count: > 0 };

        /// <summary>
        /// Gets the type of the item in the room.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the room has no item (check with <see cref="HasItem"/>).</exception>
        public IEnumerable<Type> ItemType
        {
            get
            {
                if (HasItems) return _items.Select(item => item.GetType());
                throw new InvalidOperationException("No items in the room");
            }
        }

        /// <summary>
        /// Places an item in the inventory, removing it from another one.
        /// </summary>
        /// <param name="from">The inventory from which the item is taken. The item is removed from this inventory.</param>
        /// <param name="nth"></param>
        /// <exception cref="InvalidOperationException">Thrown if the room already contains an item (check with <see cref="HasItem"/>).</exception>
        [MemberNotNull(nameof(_items))]
        public void MoveItemFrom(Inventory from, int nth = 0)
        {
            if (from._items == null || from._items.Count <= nth)
            {
                throw new InvalidOperationException("No item to take from the source inventory");
            }

            var fromElement = from._items.ElementAt(nth);
            _items?.Add(fromElement);
            from._items.Remove(fromElement);
        }

        public void MoveAllItemsFrom(Inventory from)
        {
            if (from._items == null)
            {
                throw new InvalidOperationException("No item to take from the source inventory");
            }
            _items.InsertRange(0, from._items);
            from._items.Clear();
        }

        protected List<ICollectable> _items = item != null ? [item] : [];
    }
}
