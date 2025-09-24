namespace Labyrinth;

public class Door : Tile
{
    private bool _isOpen;
    public Key Key { get; }
    
    public Door()
    {
        Key = new Key();
        _isOpen = false;
    }
    
    public override bool IsTraversable => _isOpen;
    
    public override void Pass()
    {
        if (!_isOpen)
        {
            throw new InvalidOperationException("La porte est fermée.");
        }
    }
    
    public void Unlock(Key key)
    {
        if (key == Key)
        {
            _isOpen = true;
        }
        else
        {
            throw new InvalidOperationException("La clé ne correspond pas à cette porte.");
        }
    }

    public void Lock(Key key)
    {
        if (key == Key)
        {
            _isOpen = false;
        }
        else
        {
            throw new InvalidOperationException("La clé ne correspond pas à cette porte.");
        }
    }
}