using Labyrinth.Collectable;

namespace Labyrinth.Tile;

public class Room(ICollectable? item) : Tile
{
    public override bool IsTraversable { get; } =  true;
    public ICollectable? Item {  get; set; } = item;

    public override void Pass()
    {
        throw new NotImplementedException();
    }
}