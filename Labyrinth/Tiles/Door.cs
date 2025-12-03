using System;
using System.Linq;
using Labyrinth.Items;

namespace Labyrinth.Tiles
{
    public class Door : Tile
    {
        public Door() : base(new Key())
        {
            // On capture la clé propre à cette porte.
            _key = LocalInventory.Items.OfType<Key>().Single();
        }

        public override bool IsTraversable => IsOpened;

        public bool IsOpened => !IsLocked;

        public bool IsLocked =>
            // Verrouillée si la clé de cette porte n’est pas dans l’inventaire local.
            !LocalInventory.Items.OfType<Key>().Any(k => ReferenceEquals(k, _key));

        public bool Open(Inventory keySource)
        {
            if (IsOpened)
            {
                throw new InvalidOperationException("Door is already unlocked.");
            }

            // On déplace le premier item de la source vers la porte.
            LocalInventory.MoveItemFrom(keySource, 0);

            // Si après ça la porte ne contient toujours pas sa clé spécifique, on a
            // utilisé une mauvaise clé : on la renvoie dans la source.
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

            // On trouve l’index de la clé de cette porte dans LocalInventory
            var keyIndex = LocalInventory.Items
                .Select((item, index) => new { item, index })
                .First(x => x.item is Key k && ReferenceEquals(k, _key))
                .index;

            whereKeyGoes.MoveItemFrom(LocalInventory, keyIndex);
        }

        private readonly Key _key;
    }
}