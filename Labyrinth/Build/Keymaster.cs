using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    /// <summary>
    /// Manage the creation of doors and key rooms ensuring each door has a corresponding key room.
    /// </summary>
    public sealed class KeyMaster : IDisposable
    {
        /// <summary>
        /// Ensure all created doors have a corresponding key room and vice versa.
        /// </summary>
        /// <exception cref="InvalidOperationException">Some keys are missing or are not placed.</exception>
        public void Dispose()
        {
            if (_unplacedKey.HasItem || _emptyKeyRoom is not null)
            {
                throw new InvalidOperationException("Unmatched key/door creation");
            }
        }

        /// <summary>
        /// Create a new door and place its key in a previously created empty key room (if any).
        /// </summary>
        /// <returns>Created door</returns>
        /// <exception cref="NotSupportedException">Multiple doors before key placement</exception>
        public Door NewDoor()
        {
            if (_unplacedKey.HasItem)
            {
                throw new NotSupportedException("Unable to handle multiple doors before key placement");
            }
            var door = new Door();

            door.LockAndTakeKey(_unplacedKey);
            PlaceKey();
            return door;
        }

        /// <summary>
        /// Create a new room with key and place the key if a door was previously created.
        /// </summary>
        /// <returns>Created key room</returns>
        /// <exception cref="NotSupportedException">Multiple keys before key placement</exception>
        public Room NewKeyRoom()
        {
            if (_emptyKeyRoom is not null)
            {
                throw new NotSupportedException("Unable to handle multiple keys before door creation");
            }
            var room = _emptyKeyRoom = new Room();
            PlaceKey();
            return room;
        }

        private void PlaceKey()
        {
            if (_unplacedKey.HasItem && _emptyKeyRoom is not null)
            {
                _emptyKeyRoom.Pass().MoveItemFrom(_unplacedKey);
                _emptyKeyRoom = null;
            }
        }

        private readonly MyInventory _unplacedKey = new();
        private Room? _emptyKeyRoom;
    }
}