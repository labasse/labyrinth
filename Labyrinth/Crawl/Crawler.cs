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

        /// <summary>
        /// Creates a new crawler positioned at the given coordinates and facing North by default.
        /// </summary>
        /// <param name="tiles">The 2D grid of tiles representing the labyrinth.</param>
        /// <param name="startX">The starting X position (column index) in the tiles grid.</param>
        /// <param name="startY">The starting Y position (row index) in the tiles grid.</param>
        public Crawler(Tile[,] tiles, int startX, int startY)
        {
            _tiles = tiles;
            _width = tiles.GetLength(0);
            _height = tiles.GetLength(1);
            X = startX;
            Y = startY;
            Direction = Direction.North;
        }

        /// <summary>
        /// Current X position of the crawler in the grid.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Current Y position of the crawler in the grid.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Current facing direction of the crawler.
        /// </summary>
        public Direction Direction { get; private set; }

        /// <summary>
        /// The tile located directly in front of the crawler given its current position and direction.
        /// </summary>
        /// <remarks>
        /// Returns <see cref="Outside.Singleton"/> if the next position is out of bounds.
        /// </remarks>
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

        /// <summary>
        /// Moves the crawler forward by one tile in the current direction.
        /// </summary>
        /// <returns>An <see cref="Inventory"/> possibly containing items collected while passing through the tile.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the tile in front is not traversable.</exception>
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

