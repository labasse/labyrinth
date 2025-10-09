using Labyrinth.Items;

namespace Labyrinth.Tiles
{
    /// <summary>
    /// A room in the labyrinth.
    /// </summary>
    /// <remarks>
    /// Initialize a new room, optionally with a collectable item.
    /// </remarks>
    /// <param name="item">Item in the room</param>
    /// <param name="isStart">Room is the starting point of the crawler</param>
    public class Room(ICollectable? item = null, bool isStart = false) : Tile(item)
    {
        public override bool IsTraversable => true;
        
        /// <summary>
        /// Gets a value indicating whether the tile is the starting point of the labyrinth.
        /// </summary>
        public bool IsStartingPoint { get; set; } = isStart;
    }
}
