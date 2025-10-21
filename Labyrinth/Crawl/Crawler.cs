using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    /// <summary>
    /// Labyrinth crawler implementation.
    /// </summary>
    internal class Crawler : ICrawler
    {
        private readonly Tile[,] _tiles;
        private readonly int _width;
        private readonly int _height;

        public Crawler(Tile[,] tiles, int startX, int startY)
        {
            _tiles = tiles;
            _width = tiles.GetLength(0);
            _height = tiles.GetLength(1);
            X = startX;
            Y = startY;
            Direction = Direction.North;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Direction Direction { get; private set; }

        public Tile FacingTile
        {
            get
            {
                int nextX = X + Direction.DeltaX;
                int nextY = Y + Direction.DeltaY;

                // Check boundaries
                if (nextX < 0 || nextX >= _width || nextY < 0 || nextY >= _height)
                {
                    return Outside.Singleton;
                }

                return _tiles[nextX, nextY];
            }
        }

        public Inventory Walk()
        {
            if (!FacingTile.IsTraversable)
            {
                throw new InvalidOperationException("Cannot walk through a non-traversable tile.");
            }

            var inventory = FacingTile.Pass();
            X += Direction.DeltaX;
            Y += Direction.DeltaY;

            return inventory;
        }
    }
}

