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
        // Doors created but not yet matched with a key room
        private readonly Queue<Door> pendingDoors = new();

        // Rooms created but not yet matched with a key
        private readonly Queue<Room> pendingRooms = new();

        /// <summary>
        /// Ensure all created doors have a corresponding key room and vice versa.
        /// </summary>
        /// <exception cref="InvalidOperationException">Some keys are missing or are not placed.</exception>
        public void Dispose()
        {
            if (pendingDoors.Count > 0 || pendingRooms.Count > 0)
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
            pendingDoors.Enqueue(door);
            TryMatch();
            return door;
        }

        /// <summary>
        /// Creates a new "key" room, adds it to the queue of pending rooms, 
        /// and attempts to match it with other existing rooms if possible.
        /// </summary>
        /// <returns>The newly created room</returns>
        public Room NewKeyRoom()
        {
            var room = new Room();
            pendingRooms.Enqueue(room);
            TryMatch();
            return room;
        }

        /// <summary>
        /// Matches pending doors with pending rooms and places keys accordingly.
        /// </summary>
        private void TryMatch()
        {
            while (pendingDoors.Count > 0 && pendingRooms.Count > 0)
            {
                var door = pendingDoors.Dequeue();
                var room = pendingRooms.Dequeue();

                // Each door has its own key inventory
                var keyInv = new MyInventory();
                door.LockAndTakeKey(keyInv);
                room.Pass().MoveItemFrom(keyInv);
            }
        }
    }
}