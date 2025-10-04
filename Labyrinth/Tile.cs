namespace Labyrinth
{
    public abstract class Tile
    {
        public abstract bool IsTraversable { get; }
        public abstract void Pass();
    }
}
