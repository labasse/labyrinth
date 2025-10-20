using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl;

/// <summary>
/// Default implementation of the <see cref="ICrawler"/> interface.
/// Represents the entity moving inside the labyrinth.
/// </summary>
public class Crawler : ICrawler
{
    private readonly Tile[,] _tiles;

    /// <summary>
    /// Current X coordinate of the crawler.
    /// </summary>
    public int X { get; private set; }

    /// <summary>
    /// Current Y coordinate of the crawler.
    /// </summary>
    public int Y { get; private set; }

    /// <summary>
    /// Current facing direction of the crawler.
    /// </summary>
    public Direction Direction { get; set; }

    /// <summary>
    /// Creates a new crawler instance starting at the given coordinates.
    /// </summary>
    /// <param name="tiles">2D array representing the labyrinth tiles.</param>
    /// <param name="startX">Initial X position.</param>
    /// <param name="startY">Initial Y position.</param>
    public Crawler(Tile[,] tiles, int startX, int startY)
    {
        _tiles = tiles;
        X = startX;
        Y = startY;
        Direction = Direction.North;
    }

    /// <summary>
    /// Gets the tile currently in front of the crawler, based on its current <see cref="Direction"/>.
    /// Returns an <see cref="Outside"/> tile if the crawler is facing beyond labyrinth borders.
    /// </summary>
    public Tile FacingTile
    {
        get
        {
            int fx = X;
            int fy = Y;

            var dx = Direction.DeltaX;
            var dy = Direction.DeltaY;

            if (dx == 0 && dy == -1) fy--;
            else if (dx == 0 && dy == 1) fy++;
            else if (dx == -1 && dy == 0) fx--;
            else if (dx == 1 && dy == 0) fx++; 
            else throw new NotSupportedException();

            if (fx < 0 || fy < 0 || fx >= _tiles.GetLength(0) || fy >= _tiles.GetLength(1))
                return Outside.Singleton;

            return _tiles[fx, fy];
        }
    }

    /// <summary>
    /// Moves the crawler one tile forward if the tile is traversable.
    /// </summary>
    /// <returns>An <see cref="Inventory"/> of items collected after the move.</returns>
    /// <exception cref="NotImplementedException">Thrown until movement is implemented.</exception>
    public Inventory Walk()
    {
        var newX = X + Direction.DeltaX;
        var newY = Y + Direction.DeltaY;

        if (newX < 0 || newY < 0 || newX >= _tiles.GetLength(0) || newY >= _tiles.GetLength(1))
            throw new InvalidOperationException("Cannot walk outside the labyrinth.");

        var nextTile = _tiles[newX, newY];

        if (!nextTile.IsTraversable)
            throw new InvalidOperationException("Tile not traversable.");

        Inventory inventory = nextTile.Pass();  

        X = newX;
        Y = newY;

        return inventory;
    }
}