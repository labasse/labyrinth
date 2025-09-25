namespace Labyrinth;

/// <summary>
/// Represents a wall tile that blocks movement through the labyrinth.
/// </summary>
public class Wall : Tile
{
    /// <summary>
    /// Gets a value indicating whether this wall can be traversed.
    /// Walls are never traversable.
    /// </summary>
    public override bool IsTraversable => false;
    
    /// <summary>
    /// Attempts to pass through the wall, which always fails.
    /// </summary>
    /// <exception cref="InvalidOperationException">Always thrown as walls cannot be passed through.</exception>
    public override void Pass()
    {
        throw new InvalidOperationException("Cannot pass through a wall");
    }
}
