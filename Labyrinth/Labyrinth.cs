using Labyrinth.Crawl;
using Labyrinth.Tiles;
using System.Text;
using Labyrinth.Items;

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
            (_tiles, _spawnPoint) = Build.AsciiParser.Parse(ascii_map);
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

        public ICrawler NewCrawler() => new Crawler(_spawnPoint.x, _spawnPoint.y, _tiles);

        private readonly Tile[,] _tiles;
        
        private readonly (int x, int y) _spawnPoint;
        
        
        private class Crawler(int x, int y, Tile[,] tiles)
            : ICrawler
        {
            public int X { get; private set; } = x;
    
            public int Y { get; private set; } = y;
    
            public Direction Direction { get; } = Direction.North;

            private Tile[,] Tiles { get; } = tiles;

            private Inventory PlayerInventory { get; } = new MyInventory();

            public Tile FacingTile
            {
                get {
                    var newX = X + Direction.DeltaX;
                    var newY = Y + Direction.DeltaY;
                    if (newX < 0 || newX >= Tiles.GetLength(0) || newY < 0 || newY >= Tiles.GetLength(1)) {
                        return Outside.Singleton;
                    }
                    return Tiles[newX,  newY]; 
                }
            } 
    
            public Inventory Walk()
            {
                var next = FacingTile;
                
                if (next == Outside.Singleton) {
                    throw new InvalidOperationException("Cannot walk outside the labyrinth bounds."); // In future implementation, we might want to implement "wining" here
                }
                if (!next.IsTraversable) {
                    throw new InvalidOperationException("Cannot walk into a non-traversable tile.");
                }
                
                if (next is Door { IsLocked: true } door) {
                    try {
                        door.Open(PlayerInventory);
                    }
                    catch (InvalidOperationException) {
                        throw new InvalidOperationException("Cannot open this door with current inventory.");
                    }
                }
                
                MoveForward();
                TryPickupItem();
                
                return PlayerInventory;
            }
            
            private void MoveForward()
            {
                X += Direction.DeltaX;
                Y += Direction.DeltaY;
            }

            private void TryPickupItem()
            {
                if (Tiles[X, Y] is not Room room) return;
                var roomInv = room.Pass();
                if (roomInv.HasItem) {
                    PlayerInventory.MoveItemFrom(roomInv);
                }

            }
        }
    }
}


