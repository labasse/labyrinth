using Labyrinth.Items;
using Labyrinth.Tiles;
using System.Collections.Generic;

namespace Labyrinth.Build
{
    /// <summary>
    /// Manage the creation of doors and key rooms ensuring each door has a corresponding key room.
    /// </summary>
    public sealed class Keymaster : IDisposable
    {
        private readonly Queue<Door> pendingDoors = new();
        private readonly Queue<Room> pendingKeyRooms = new();

        /// <summary>
        /// Ensure all created doors have a corresponding key room and vice versa.
        /// </summary>
        /// <exception cref="InvalidOperationException">Some keys or doors are unmatched.</exception>
        public void Dispose()
        {
            if (pendingDoors.Count > 0 || pendingKeyRooms.Count > 0)
            {
                throw new InvalidOperationException("Unmatched key/door creation");
            }
        }

        /// <summary>
        /// Create a new door and place its key in a previously created empty key room (if any).
        /// </summary>
        /// <returns>Created door</returns>
        public Door NewDoor()
        {
            var door = new Door();

            if (pendingKeyRooms.Count > 0)
            {
                // Associer immédiatement une clé
                var room = pendingKeyRooms.Dequeue();
                var keyInventory = new MyInventory(new Key());
                door.LockAndTakeKey(keyInventory);
                room.Pass().MoveItemFrom(keyInventory);
            }
            else
            {
                // Mettre la porte en attente
                pendingDoors.Enqueue(door);
            }

            return door;
        }

        /// <summary>
        /// Create a new room with key and place the key if a door was previously created.
        /// </summary>
        /// <returns>Created key room</returns>
        public Room NewKeyRoom()
        {
            var room = new Room();

            if (pendingDoors.Count > 0)
            {
                // Associer immédiatement une clé
                var door = pendingDoors.Dequeue();
                var keyInventory = new MyInventory(new Key());
                door.LockAndTakeKey(keyInventory);
                room.Pass().MoveItemFrom(keyInventory);
            }
            else
            {
                // Mettre la salle en attente
                pendingKeyRooms.Enqueue(room);
            }

            return room;
        }
    }
}
