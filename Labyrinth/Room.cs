namespace Labyrinth;

public sealed class Room : Tile
{
    public override bool IsTraversable => true;
    
    public ICollectable? Item { get; set; }
    
    public override void Pass()
    {
        // Do nothing : congratulation you are in a room yaaay
    }
}