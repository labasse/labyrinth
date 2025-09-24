namespace Labyrinth;

public class Wall : Tile
{
    public override bool IsTraversable => false;
    
    public override void Pass()
    {
        throw new InvalidOperationException("Impossible de traverser un mur.");
    }
}