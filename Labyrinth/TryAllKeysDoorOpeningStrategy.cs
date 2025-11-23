using System.Linq;
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth
{
    /// <summary>
    /// Door opening strategy that tries all collected keys
    /// until the door is opened or no keys remain.
    /// </summary>
    public class TryAllKeysDoorOpeningStrategy : IDoorOpeningStrategy
    {
        public void TryOpen(Door door, MyInventory bag)
        {
            var temp = new MyInventory();

            int keysToTry = bag.Items.OfType<Key>().Count();

            for (int i = 0; i < keysToTry && door.IsLocked; i++)
            {
                var itemsList = bag.Items.ToList();
                int keyIndex = itemsList.FindIndex(item => item is Key);

                if (keyIndex < 0)
                    break;
                
                temp.MoveItemFrom(bag, keyIndex);

                bool opened = door.Open(temp);

                if (!opened)
                {
                    bag.MoveItemFrom(temp);
                }
            }
        }
    }
}
