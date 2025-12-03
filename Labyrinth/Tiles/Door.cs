using System;
using System.Linq;
using Labyrinth.Items;

namespace Labyrinth.Tiles
{
    public class Door : Tile
    {
        public Door() : base(new Key())
        {
            _key = LocalInventory.Items.OfType<Key>().Single();
        }

        public override bool IsTraversable => IsOpened;

        public bool IsOpened => !IsLocked;

        public bool IsLocked =>
            !LocalInventory.Items.OfType<Key>().Any(k => ReferenceEquals(k, _key));

        public bool Open(Inventory keySource)
        {
            if (IsOpened)
            {
                throw new InvalidOperationException("Door is already unlocked.");
            }

            LocalInventory.MoveItemFrom(keySource, 0);
            
            if (!LocalInventory.Items.OfType<Key>().Any(k => ReferenceEquals(k, _key)))
            {
                keySource.MoveItemFrom(LocalInventory, 0);
                return false;
            }

            return true;
        }

        public void LockAndTakeKey(Inventory whereKeyGoes)
        {
            if (IsLocked)
            {
                throw new InvalidOperationException("Door is already locked.");
            }

            var keyIndex = LocalInventory.Items
                .Select((item, index) => new { item, index })
                .First(x => x.item is Key k && ReferenceEquals(k, _key))
                .index;

            whereKeyGoes.MoveItemFrom(LocalInventory, keyIndex);
        }

        private readonly Key _key;
    }
}