using Labyrinth.Collectable;

namespace Labyrinth;

public class Door: Tile.Tile
{
    public override bool IsTraversable { get; } = true;
    public Key Key { get; } // key needed to open and close the door
    public bool Open { get; set; } = false;
    public override void Pass()
    {
        throw new NotImplementedException();
    }

    public Door(Key key)
    {
        Key = key;
    }

    public Door(Guid keyId, bool open)
    {
        Key = new Key(keyId);
        Open = open;
    }

    public void OpenDoor(Key key)
    {
        if (Key == key)
        {
            Open = true;
        }
    }

    public void CloseDoor(Key key)
    {
        if (Key == key)
        {
            Open = false;
        }
    }
}