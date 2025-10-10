using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl;

public class Crawler : ICrawler
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Direction Direction { get; }
    
    private readonly Func<int, int, Direction, Tile> _getFacingTile;
    public Tile FacingTile => _getFacingTile(X, Y, Direction);
    
    public Crawler(int x, int y, Direction direction, Func<int, int, Direction, Tile> getFacingTile)
    {
        X = x;
        Y = y;
        Direction = direction;
        _getFacingTile = getFacingTile;
    }
    
    /// <summary>
    /// Pass the tile in front of the crawler and move into it.
    /// </summary>
    /// <returns>An inventory of the collectable items in the place reached.</returns>
    public Inventory Walk()
    {
        if (!FacingTile.IsTraversable)
        {
            throw new InvalidOperationException("Cannot walk on a non-traversable tile.");
        }
        
        var inventory = FacingTile.Pass();
        
        X = X + Direction.DeltaX;
        Y = Y + Direction.DeltaY;
        
        return inventory;
    }
    
}