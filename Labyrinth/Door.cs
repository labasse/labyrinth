namespace Labyrinth;

/// <summary>
/// Represents a door tile that can be locked or unlocked using a key.
/// </summary>
public class Door : Tile
{
    private bool _isLocked = true;
    
    /// <summary>
    /// Gets the key required to unlock this door.
    /// </summary>
    public Key Key { get; } = new Key();
    
    /// <summary>
    /// Gets a value indicating whether this door can be traversed.
    /// A door is traversable only when it is unlocked.
    /// </summary>
    public override bool IsTraversable => !_isLocked;
    
    /// <summary>
    /// Unlocks the door if the provided key matches the door's key.
    /// </summary>
    /// <param name="key">The key to use for unlocking the door.</param>
    public void Unlock(Key key)
    {
        if (key == Key)
            _isLocked = false;
    }
    
    /// <summary>
    /// Locks the door, preventing traversal until unlocked.
    /// </summary>
    public void Lock()
    {
        _isLocked = true;
    }
    
    /// <summary>
    /// Attempts to pass through the door.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the door is locked.</exception>
    public override void Pass()
    {
        if (_isLocked)
            throw new InvalidOperationException("Door is locked");
    }
}
