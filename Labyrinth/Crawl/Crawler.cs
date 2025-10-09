
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    public class Crawler : ICrawler
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Direction Direction { get; private set; }

        public Tile FacingTile { get; private set; }

        private readonly Tile[,] _tiles;

        public Inventory Walk()
        {
            throw new NotImplementedException();
        }

        public Crawler(Tile[,] tiles, int startX, int startY)
        {
            _tiles = tiles;
            X = startX;
            Y = startY;
            Direction = Direction.North;
            FacingTile = _tiles[X + Direction.DeltaX, Y + Direction.DeltaY];
        }
    }
}