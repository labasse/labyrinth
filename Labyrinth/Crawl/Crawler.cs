using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl;

public class Crawler: ICrawler
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Direction Direction { get; }

    private readonly Func<int, int, Direction, Tile> _getFacingTile;
    public Tile FacingTile => _getFacingTile(X, Y, Direction);
    public Inventory Walk()
    {
        Inventory inventory = FacingTile.Pass();
        X += Direction.DeltaX;
        Y += Direction.DeltaY;
        return inventory;
    }
    
    public Crawler(int x, int y, Direction direction, Func<int, int, Direction, Tile> getFacingTile)
    {
        X = x;
        Y = y;
        Direction = direction;
        _getFacingTile = getFacingTile;
    }
}