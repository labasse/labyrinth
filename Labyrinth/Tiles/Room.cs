using Labyrinth.Items;

namespace Labyrinth.Tiles
{
    /// <summary>
    /// A room in the labyrinth.
    /// </summary>
    /// <remarks>
    /// Initialize a new room, optionally with a collectable Items.
    /// </remarks>
    /// <param name="Items">Items in the room</param>
    public class Room(ICollectable? Items = null) : Tile(Items)
    {
        public override bool IsTraversable => true;
    }
}
