using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl;

public class Crawler : ICrawler
{
    private readonly Tile[,] _tiles;
    public Crawler(Tile[,] tiles, int x, int y)
    {
        _tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        X = x;
        Y = y;
        Direction = Direction.North;
        
    }
    public int X { get; private set; }
    public int Y { get; private set; }
    public Direction Direction { get; }
    public Tile FacingTile
    {
        get
        {
            var nx = X + Direction.DeltaX;
            var ny = Y + Direction.DeltaY;
            return IsInside(nx, ny) ? _tiles[nx, ny] : Outside.Singleton;
        }
    }
    
    private bool IsInside(int x, int y) =>
        x >= 0 && y >= 0 && x < _tiles.GetLength(0) && y < _tiles.GetLength(1);
    public Inventory Walk()
    {
        if (!FacingTile.IsTraversable)
        {
            throw new InvalidOperationException("Facing tile is not traversable.");
        }
        var inventory = FacingTile.Pass();
        X += Direction.DeltaX;
        Y += Direction.DeltaY;
        return inventory;
    }
}