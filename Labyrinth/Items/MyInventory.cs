using System.Collections.Generic;

namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory class that exposes the items it contains.
    /// </summary>
    /// <param name="item">Optional initial item in the inventory.</param>
    public class MyInventory(ICollectable? item = null) : Inventory(item)
    {
        /// <summary>
        /// Items in the inventory (may be empty).
        /// </summary>
        public IEnumerable<ICollectable> Items => _items;
    }
}
