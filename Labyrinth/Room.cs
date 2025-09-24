namespace Labyrinth
{
    public class Room : Tile
    {
        public ICollectable? Item { get; set; }
        public override bool IsTraversable => true;
        public override void Pass() { }
    }
}
