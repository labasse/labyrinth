namespace Labyrinth;

public sealed class Door : Tile
{
    private Key Key { get; } = Key.New();

    public override bool IsTraversable => _isTraversable;
    
    private bool _isTraversable = false;
    
    public void Unlock(Key key)
    {
        if (key == Key) {
            _isTraversable = true;
        } else {
            throw new InvalidOperationException("Cannot unlock the door with this key !!!!!");
        }
    }
    
    public override void Pass()
    {
        if (!IsTraversable) {
            throw new InvalidOperationException("Cannot pass through a door without unlocking it !!!!!");
        }
    }
}