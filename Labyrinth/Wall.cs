namespace Labyrinth;

public sealed class Wall : Tile
{
    public override bool IsTraversable => false;
    
    public override void Pass()
    {
        throw new InvalidOperationException("Cannot pass through a wall !!!!!");
    }
}