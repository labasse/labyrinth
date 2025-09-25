namespace Labyrinth.Tile;

public class Wall: Tile
{
    public override bool IsTraversable { get; } = false;
    public override void Pass()
    {
        throw new NotImplementedException();
    }
}