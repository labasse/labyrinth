using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Models;
using Labyrinth.Tiles;

namespace Labyrinth
{
    public partial class Labyrinth
    {
        private class LabyrinthCrawler : ICrawler
        {
            private Coord _coord;
            private Direction _direction;
            private readonly Tile[,] _tiles;

            public LabyrinthCrawler(Coord coord, Direction direction, Tile[,] tiles)
            {
                _coord = coord;
                _direction = direction;
                _tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
            }

            public Coord Coord => _coord;

            public Tile FacingTile => ProcessFacingTile((coord, tile) => tile);

            Direction ICrawler.Direction => _direction;

            public Inventory Walk() =>
                ProcessFacingTile((coord, tile) =>
                {
                    var inventory = tile.Pass();
                    _coord = coord;
                    return inventory;
                });

            private bool IsOut(int pos, int dimension) =>
                pos < 0 || pos >= _tiles.GetLength(dimension);

            private T ProcessFacingTile<T>(Func<Coord, Tile, T> process)
            {
                int facingX = _coord.X + _direction.DeltaX;
                int facingY = _coord.Y + _direction.DeltaY;

                bool outside = IsOut(facingX, dimension: 0) || IsOut(facingY, dimension: 1);

                var tile = outside ? (Tile)Outside.Singleton : _tiles[facingX, facingY];
                var facingCoord = new Coord(facingX, facingY);

                return process(facingCoord, tile);
            }
        }
    }
}
