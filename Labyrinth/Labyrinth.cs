using Labyrinth.Crawl;
using Labyrinth.Tiles;
using System.Text;

namespace Labyrinth
{
    public class Labyrinth
    {
        
        /// <summary>
        /// Gets the starting X coordinate of the crawler in the labyrinth.
        /// </summary>
        public int StartX { get; }
        
        /// <summary>
        /// Gets the starting Y coordinate of the crawler in the labyrinth.
        /// </summary>
        public int StartY { get; }
        
        /// <summary>
        /// Labyrinth with walls, doors and collectable items.
        /// </summary>
        /// <param name="ascii_map">A multiline string with '+', '-' or '|' for walls, '/' for doors and 'k' for key locations</param>
        /// <exception cref="ArgumentException">Thrown when string argument reveals inconsistent map sizes or characters.</exception>
        /// <exception cref="NotSupportedException">Thrown for multiple doors (resp. key locations) before key locations (resp. doors).</exception>
        public Labyrinth(string ascii_map)
        {
            _tiles = Build.AsciiParser.Parse(ascii_map, out var start_x, out var start_y);
            if (_tiles.GetLength(0) < 3 || _tiles.GetLength(1) < 3)
            {
                throw new ArgumentException("Labyrinth must be at least 3x3");
            }
            StartX = start_x;
            StartY = start_y;
        }

        /// <summary>
        /// Labyrinth width (number of columns).
        /// </summary>
        public int Width { get; private init; }

        /// <summary>
        /// Labyrinth height (number of rows).
        /// </summary>
        public int Height { get; private init; }

        /// <summary>
        /// An ascii representation of the labyrinth.
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            var res = new StringBuilder();

            for (int y = 0; y < _tiles.GetLength(1); y++)
            {
                for (int x = 0; x < _tiles.GetLength(0); x++)
                {
                    res.Append(_tiles[x, y] switch
                    {
                        Room => ' ',
                        Wall => '#',
                        Door => '/',
                        _ => throw new NotSupportedException("Unknown tile type")
                    });
                }
                res.AppendLine();
            }
            return res.ToString();
        }

        /// <summary>
        /// Creates a new crawler positioned at the starting coordinates.
        /// Throws <see cref="ArgumentException"/> if no starting point ('x') is defined.
        /// </summary>
        /// <returns>An instance of <see cref="ICrawler"/> positioned at the start.</returns>
        /// <exception cref="ArgumentException">Thrown if the labyrinth has no starting point.</exception>
        public ICrawler NewCrawler()
        {
            if (StartX < 0 || StartY < 0)
                throw new ArgumentException("No starting point ('x') defined in the labyrinth.");
            
            return new Crawler(_tiles, StartX, StartY);
        }

        /// <summary>
        /// Internal 2D array representing the labyrinth tiles.
        /// </summary>
        private readonly Tile[,] _tiles;
    }
}
