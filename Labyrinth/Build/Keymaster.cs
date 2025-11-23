using System;
using System.Collections.Generic;
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    /// <summary>
    /// Manage the creation of doors and key rooms ensuring each door has a corresponding key room.
    /// Works with any distribution of doors and keys in the map.
    /// </summary>
    public sealed class Keymaster : IDisposable
    {
        /// <summary>
        /// Ensure all created doors have a corresponding key room and vice versa.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if some doors still have no key room or some key rooms never received a key.
        /// </exception>
        public void Dispose()
        {
            if (_unplacedKeys.Count > 0 || _emptyKeyRooms.Count > 0)
            {
                throw new InvalidOperationException("Unmatched key/door creation");
            }
        }

        /// <summary>
        /// Create a new door and register its key so it can be placed later
        /// in any key room encountered afterwards.
        /// </summary>
        /// <returns>Created door.</returns>
        public Door NewDoor()
        {
            var door = new Door();

            // Take the key from the door and keep it aside until we find a key room.
            var keyInventory = new MyInventory();
            door.LockAndTakeKey(keyInventory);
            _unplacedKeys.Add(keyInventory);

            PlaceKeys();
            return door;
        }

        /// <summary>
        /// Create a new room that may receive a key for a previously created door.
        /// </summary>
        /// <returns>Created key room.</returns>
        public Room NewKeyRoom()
        {
            var room = new Room();
            _emptyKeyRooms.Add(room);

            PlaceKeys();
            return room;
        }

        /// <summary>
        /// Try to place waiting keys into waiting rooms, pairing them in creation order.
        /// </summary>
        private void PlaceKeys()
        {
            while (_unplacedKeys.Count > 0 && _emptyKeyRooms.Count > 0)
            {
                var keyInventory = _unplacedKeys[0];
                var room = _emptyKeyRooms[0];

                // Put the door's key into the room inventory.
                room.Pass().MoveItemFrom(keyInventory);

                _unplacedKeys.RemoveAt(0);
                _emptyKeyRooms.RemoveAt(0);
            }
        }

        private readonly List<MyInventory> _unplacedKeys = new();
        private readonly List<Room> _emptyKeyRooms = new();
    }
}
