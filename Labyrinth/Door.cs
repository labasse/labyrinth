public class Door : Tile
{
    private Key myKey;
    private bool isOpened = false;
    public override bool IsTraversable { get { return isOpened; } }
    public override String Character { get { return "/"; } }
    public override void Pass()
    {
        // Logic for passing through a room
    }
    public void openDoor(Key key)
    {
        if(myKey == key)
        {
            isOpened = true;
        } else
        {
            throw new InvalidOperationException("The key does not match the door.");
        }
        // Logic for opening the door
    }

    public Door()
    {
        this.myKey = new Key();
    }
}