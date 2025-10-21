using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    internal sealed class Crawler : ICrawler
    {
        public Crawler(Tile[,] tiles, (int X, int Y) start)
        {
            _tiles = tiles;
            _width = tiles.GetLength(0);
            _height = tiles.GetLength(1);
            (X, Y) = start;
            Direction = Direction.North;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Direction Direction { get; }

        public Tile FacingTile => PeekTile();

        public Inventory Walk()
        {
            var (nextX, nextY) = (X + Direction.DeltaX, Y + Direction.DeltaY);
            var target = GetTile(nextX, nextY);
            if (target is Outside)
            {
                throw new InvalidOperationException("Cannot walk outside the labyrinth.");
            }

            var inventory = target.Pass();

            X = nextX;
            Y = nextY;

            return inventory;
        }

        private Tile PeekTile() => GetTile(X + Direction.DeltaX, Y + Direction.DeltaY);

        private Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
            {
                return Outside.Singleton;
            }

            return _tiles[x, y];
        }

        private readonly Tile[,] _tiles;
        private readonly int _width;
        private readonly int _height;
    }
}
