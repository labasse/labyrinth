using Labyrinth.Crawl;
using Labyrinth.Tiles;
using System.Text;
using Labyrinth.Models;

namespace Labyrinth
{
    public partial class Labyrinth
    {
        private readonly Coord _initial_crawler_coord;
        private readonly Direction _initial_crawler_direction;
        
        /// <summary>
        /// Labyrinth with walls, doors and collectable items.
        /// </summary>
        /// <param name="ascii_map">A multiline string with '+', '-' or '|' for walls, '/' for doors and 'k' for key locations</param>
        /// <exception cref="ArgumentException">Thrown when string argument reveals inconsistent map sizes or characters.</exception>
        /// <exception cref="NotSupportedException">Thrown for multiple doors (resp. key locations) before key locations (resp. doors).</exception>
        public Labyrinth(string ascii_map)
        {
            _tiles = Build.AsciiParser.Parse(ascii_map, out _initial_crawler_coord, out _initial_crawler_direction);
            if (_tiles.GetLength(0) < 3 || _tiles.GetLength(1) < 3)
            {
                throw new ArgumentException("Labyrinth must be at least 3x3");
            }
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

        private Tile GetFacingTile(Coord coord, Direction direction)
        {
            int x = coord.X + direction.DeltaX;
            int y = coord.Y + direction.DeltaY;

            if (x < 0 || y < 0 || 
                x >= _tiles.GetLength(0) || 
                y >= _tiles.GetLength(1))
            {
                return Outside.Singleton;
            }

            return _tiles[x, y];
        }

        public ICrawler NewCrawler() => new Crawler(this, _initial_crawler_coord, _initial_crawler_direction);
        
        private readonly Tile[,] _tiles;

    }
}
