using Labyrinth.Crawl;
using Labyrinth.Tiles;
using System.Text;

namespace Labyrinth
{
    public class Labyrinth
    {
        public int StartX { get; }
        public int StartY { get; }
        
        /// <summary>
        /// Labyrinth with walls, doors and collectable items.
        /// </summary>
        /// <param name="ascii_map">A multiline string with '+', '-' or '|' for walls, '/' for doors and 'k' for key locations</param>
        /// <exception cref="ArgumentException">Thrown when string argument reveals inconsistent map sizes or characters.</exception>
        /// <exception cref="NotSupportedException">Thrown for multiple doors (resp. key locations) before key locations (resp. doors).</exception>
        public Labyrinth(string ascii_map)
        {
            _tiles = Build.AsciiParser.Parse(ascii_map, out var startX, out var startY);
            if (_tiles.GetLength(0) < 3 || _tiles.GetLength(1) < 3)
            {
                throw new ArgumentException("Labyrinth must be at least 3x3");
            }
            StartX = startX;
            StartY = startY;
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

        public ICrawler NewCrawler()
        {
            if (StartX < 0 || StartY < 0)
            {
                throw new ArgumentException("StartX or StartY cannot be less than 0");
            }
            ICrawler crawler = new Crawler(StartX, StartY, Direction.North, GetFacingTile);
            return crawler;
        }

        private Tile GetFacingTile(int x, int y, Direction direction)
        {
            int newX = x + direction.DeltaX;
            int newY = y + direction.DeltaY;
            Tile facingTile;
            if (newX >= _tiles.GetLength(0) || newY >= _tiles.GetLength(1) || newX < 0 || newY < 0)
            {
                facingTile = Outside.Singleton;
            }
            else
            {
                facingTile = _tiles[newX, newY];
            }

            return facingTile;
        }

        private readonly Tile[,] _tiles;
    }
}
