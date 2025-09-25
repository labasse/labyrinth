namespace Labyrinth;

/// <summary>
/// Represents a room tile that can be traversed and may contain collectible items.
/// </summary>
public class Room : Tile
{
    /// <summary>
    /// Gets a value indicating whether this room can be traversed.
    /// Rooms are always traversable.
    /// </summary>
    public override bool IsTraversable => true;
    
    /// <summary>
    /// Gets or sets the collectible item in this room, if any.
    /// </summary>
    public ICollectable? Item { get; set; }
    
    /// <summary>
    /// Passes through the room. This operation always succeeds for rooms.
    /// </summary>
    public override void Pass() {}
}
