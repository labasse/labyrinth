namespace Labyrinth.Tiles
{
    /// <summary>
    /// A wall tile in the labyrinth.
    /// </summary>
    public class Wall(char symbol) : Tile
    {
        
        public char Symbol { get; } = symbol;

        /// <summary>
        /// Singletons instance of the Wall class (memory optimization).
        /// </summary>
        public static Wall Horizontal { get; } = new('-');
        public static Wall Vertical { get; } = new('|');
        public static Wall Corner { get; } = new('+');

        public override bool IsTraversable => false;
    }
}
