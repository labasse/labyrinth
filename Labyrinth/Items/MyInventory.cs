namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory class that exposes the Items it contains.
    /// </summary>
    /// <param name="Items">Optional initial Items in the inventory.</param>
    public class MyInventory(ICollectable? Items = null) : Inventory(Items)
    {
        /// <summary>
        /// Items in the inventory, or null if empty.
        /// </summary>
        public IEnumerable<ICollectable> Items => _items;
    }
}
