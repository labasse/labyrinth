using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    internal class Crawler : ICrawler
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Direction { get; private set; }

        public Tile FacingTile
        {
            get
            {
                var nx = X + Direction.DeltaX;
                var ny = Y + Direction.DeltaY;
                if (nx < 0 || ny < 0 || nx >= _tiles.GetLength(0) || ny >= _tiles.GetLength(1))
                {
                    return Outside.Singleton;
                }
                return _tiles[nx, ny];
            }
        }

        public Inventory Walk()
        {
            var tile = FacingTile;
            var inv = tile.Pass(); 
            X += Direction.DeltaX;
            Y += Direction.DeltaY;
            return inv;
        }

        public Crawler(Tile[,] tiles, int startX, int startY)
        {
            _tiles = tiles;
            X = startX;
            Y = startY;
            Direction = (startY == tiles.GetLength(1) - 1) ? Direction.South : Direction.North;
        }

        private readonly Tile[,] _tiles;
    }
}
