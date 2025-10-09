
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    /// <summary>
    /// Creates a new crawler at the given starting position, facing north.
    /// </summary>
    /// <param name="tiles">The tile map.</param>
    /// <param name="startX">The starting X position.</param>
    /// <param name="startY">The starting Y position.</param>
    public class Crawler(Tile[,] tiles, Point start) : ICrawler
    {
        private Point Position { get; set; } = start;

        public Direction Direction { get; private set; } = Direction.North;

        public Tile FacingTile
        {
            get
            {
                var front = Position + Direction;
                if (front.X < 0 || front.Y < 0 || front.X >= _tiles.GetLength(0) || front.Y >= _tiles.GetLength(1))
                {
                    return Outside.Singleton;
                }
                return _tiles[front.X, front.Y];
            }
        }

        public int X => Position.X;

        public int Y => Position.Y;

        private readonly Tile[,] _tiles = tiles;

        public Inventory Walk()
        {
            throw new NotImplementedException();
        }
    }
}