namespace Labyrinth;

public abstract class Tile
{
    public abstract bool IsTraversable { get; }

    public abstract void Pass();
    
    public override abstract string ToString();
}

public class Wall : Tile
{
    public override bool IsTraversable => false;

    public override void Pass()
    {
        throw new InvalidOperationException("Cannot pass through a wall.");
    }
    
    public override string ToString()
    {
        return "+";
    }
}

public class Room : Tile
{
    public Room(ICollectable? item = null)
    {
        Item = item;
    }

    public ICollectable? Item { get; set; }

    public override bool IsTraversable => true;

    public override void Pass()
    {
    }

    public override string ToString()
    {
        return Item == null ? " " : $"{Item}";
    }
}

public class Door : Tile
{
    private bool _isLocked = true;

    public Door()
    {
        Key = new Key();
    }

    public override bool IsTraversable => !_isLocked;

    public Key Key { get; }

    public bool IsLocked => _isLocked;

    public override void Pass()
    {
        if (_isLocked)
        {
            throw new InvalidOperationException("Cannot pass through a locked door.");
        }
    }

    public bool MatchesKey(Key key) => key == Key;

    public void Unlock(Key key)
    {
        if (!MatchesKey(key))
        {
            throw new InvalidOperationException("This key does not operate this door.");
        }

        _isLocked = false;
    }

    public void Lock(Key key)
    {
        if (!MatchesKey(key))
        {
            throw new InvalidOperationException("This key does not operate this door.");
        }

        _isLocked = true;
    }

    public override string ToString()
    {
        return _isLocked ? "/" : "=";
    }
}
