namespace Labyrinth;

/// <summary>
/// Represents an abstract base class for all tiles in the labyrinth.
/// </summary>
public abstract class Tile
{
    /// <summary>
    /// Gets a value indicating whether this tile can be traversed by a player.
    /// </summary>
    public abstract bool IsTraversable { get; }
    
    /// <summary>
    /// Attempts to pass through this tile. Implementation varies by tile type.
    /// </summary>
    public abstract void Pass();
}
