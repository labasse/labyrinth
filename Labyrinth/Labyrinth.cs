using Labyrinth.Crawl;
using Labyrinth.Tiles;
using System.Text;

namespace Labyrinth
{
    public class Labyrinth
    {
        /// <summary>
        /// Labyrinth with walls, doors and collectable items.
        /// </summary>
        /// <param name="ascii_map">A multiline string with '+', '-' or '|' for walls, '/' for doors and 'k' for key locations</param>
        /// <exception cref="ArgumentException">Thrown when string argument reveals inconsistent map sizes or characters.</exception>
        /// <exception cref="NotSupportedException">Thrown for multiple doors (resp. key locations) before key locations (resp. doors).</exception>
        public Labyrinth(string ascii_map)
        {
            _tiles = Build.AsciiParser.Parse(ascii_map, this);
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
        /// Starting room of the labyrinth.
        /// </summary>
        public Room? StartingRoom { get; private set; } = null;
        
        /// <summary>
        /// Creates a new starting room, replacing the previous one if any.
        /// </summary>
        /// <returns>The newly created starting room.</returns>
        public Room CreateStartingRoom()
        {
            if (StartingRoom != null)
            {
                StartingRoom.IsStartingPoint = false;
            }
            StartingRoom = new Room(isStart: true);
            return StartingRoom;
        }

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
            for (int y = 0; y < _tiles.GetLength(1); y++)
            {
                for (int x = 0; x < _tiles.GetLength(0); x++)
                {
                    if (_tiles[x, y] is Room r && r.IsStartingPoint)
                    {
                        return new Crawl.Crawler(x, y, Direction.North, GetFacingTile);
                    }
                }
            }
            throw new ArgumentException("No starting room defined in the labyrinth");
        }
        
        private Tile GetFacingTile(int x, int y, Direction direction)
        {
            int fx = x + direction.DeltaX;
            int fy = y + direction.DeltaY;
            if (fx < 0 || fy < 0 || fx >= _tiles.GetLength(0) || fy >= _tiles.GetLength(1))
                return Outside.Singleton;
            return _tiles[fx, fy];
        }

        private readonly Tile[,] _tiles;
    }
}
