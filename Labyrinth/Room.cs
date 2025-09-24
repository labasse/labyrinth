public class Room : Tile, ICollectable
{
    ICollectable? item { get; set; }
    public override bool IsTraversable { get { return true; } }
    public override void Pass()
    {
        // Logic for passing through a room
    }
}