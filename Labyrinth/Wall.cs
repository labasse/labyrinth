namespace Labyrinth
{
    public class Wall : Tile
    {
        public override bool IsTraversable => false;
        public override void Pass() => throw new System.InvalidOperationException("Cannot pass through a wall.");
    }
}

