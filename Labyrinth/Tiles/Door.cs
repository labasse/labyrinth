using Labyrinth.Items;
using System.Linq;

namespace Labyrinth.Tiles
{
    public class Door : Tile
    {
        public Door() : base(new Key())
        {
            _key = LocalInventory.Items.OfType<Key>().First();
        }

        public override bool IsTraversable => IsOpened;

        public bool IsOpened => !IsLocked;

        // Une porte est verrouillée si elle contient encore une clé
        public bool IsLocked => LocalInventory.HasItems;

        public bool Open(Inventory keySource)
        {
            if (IsOpened)
                throw new InvalidOperationException("Door is already unlocked.");

            LocalInventory.MoveItemFrom(keySource);
            var insertedKey = LocalInventory.Items.First();

            if (!insertedKey.Equals(_key))
            {
                // mauvaise clé → on la rend
                keySource.MoveItemFrom(LocalInventory);
            }

            return IsOpened;
        }

        public void LockAndTakeKey(Inventory whereKeyGoes)
        {
            if (!IsLocked)
                throw new InvalidOperationException("Door is already unlocked.");

            // déplacer la clé vers l’inventaire cible
            whereKeyGoes.MoveItemFrom(LocalInventory);
        }

        private readonly Key _key;
    }
}
