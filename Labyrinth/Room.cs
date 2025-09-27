public class Room : Tile
{
    public override bool IsTraversable => true;

    public Collectable? Item { get; set; }  // autorise null

    public Room() { Item = null; }

    public Room(Collectable item)
    {
        Item = item;
    }

    public override void Pass()
    {
        // salle toujours traversable
    }
}
