public class Room : Tile, ICollectable
{
    public ICollectable? Item { get; set; }
    public override String Character { 
        get { if(this.Item == null)
            {
                return " ";
            }
            else
            {
                return "K";
            }
        }}
    public override bool IsTraversable { get { return true; } }
    public override void Pass()
    {
        // Logic for passing through a room
    }

    public void PlaceItem(ICollectable item)
    {
        Item = item;
    }
}