using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    /// <summary>
    /// Manage the creation of doors and key rooms ensuring each door has a corresponding key room.
    /// </summary>
    public sealed class Keymaster : IDisposable
    {
        /// <summary>
        /// Ensure all created doors have a corresponding key room and vice versa.
        /// </summary>
        /// <exception cref="InvalidOperationException">Some keys are missing or are not placed.</exception>
        public void Dispose()
        {
            if (unplacedKeys.Count > 0 || emptyKeyRooms.Count > 0)
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
            var keyInventory = new MyInventory();

            door.LockAndTakeKey(keyInventory);
            unplacedKeys.Enqueue(keyInventory);

            PlaceKeys();
            return door;
        }

        /// <summary>
        /// Create a new room with key and place the key if a door was previously created.
        /// </summary>
        /// <returns>Created key room</returns>
        public Room NewKeyRoom()
        {
            var room = new Room();
            emptyKeyRooms.Enqueue(room);

            PlaceKeys();
            return room;
        }

        private void PlaceKeys()
        {
            while (unplacedKeys.Count > 0 && emptyKeyRooms.Count > 0)
            {
                var keyInventory = unplacedKeys.Dequeue();
                var keyRoom = emptyKeyRooms.Dequeue();

                keyRoom.Pass().MoveItemFrom(keyInventory);
            }
        }

        private readonly Queue<MyInventory> unplacedKeys = new();
        private readonly Queue<Room> emptyKeyRooms = new();
    }
}
