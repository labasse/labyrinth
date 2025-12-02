namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory class that exposes the item it contains.
    /// </summary>
    /// <param name="item">Optional initial item in the inventory.</param>
    public class MyInventory(IEnumerable<ICollectable>? items = null) : Inventory(items)
    {
        /// <summary>
        /// Item in the inventory, or null if empty.
        /// </summary>
        public IEnumerable<ICollectable>? Items => _items;
    }
}
