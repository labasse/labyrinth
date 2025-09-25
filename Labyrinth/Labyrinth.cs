namespace Labyrinth;

/// <summary>
/// Represnts a labyrinth game board composed of tiles.
/// </summary>
public class Labyrinth
{
    private readonly Tile[,] _tiles;
    
    /// <summary>
    /// Gets the width of the labyrinth in tiles.
    /// </summary>
    public int Width { get; }
    
    /// <summary>
    /// Gets the height of the labyrinth in tiles.
    /// </summary>
    public int Height { get; }
    
    /// <summary>
    /// Initializes a new instance of the Labyrinth class from a string representation.
    /// </summary>
    /// <param name="maze">A string representation of the maze where each character represents a tile type.</param>
    public Labyrinth(string maze)
    {
        if (string.IsNullOrEmpty(maze))
        {
            Width = 0;
            Height = 0;
            _tiles = new Tile[0, 0];
            return;
        }
        
        var lines = maze.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        Height = lines.Length;
        Width = lines.Max(line => line.Length);
        
        _tiles = new Tile[Height, Width];
        
        for (int y = 0; y < Height; y++)
        {
            var line = lines[y];
            for (int x = 0; x < Width; x++)
            {
                char symbol = x < line.Length ? line[x] : ' ';
                _tiles[y, x] = CreateTile(symbol);
            }
        }
    }
    
    /// <summary>
    /// Creates a tile based on the provided symbol character.
    /// </summary>
    /// <param name="symbol">The character symbol representing the tile type.</param>
    /// <returns>A Tile instance corresponding to the symbol.</returns>
    private Tile CreateTile(char symbol)
    {
        return symbol switch
        {
            '+' or '-' or '|' => new Wall(),
            '/' => new Door(),
            'k' => new Room { Item = new Key() },
            ' ' => new Room(),
            _ => new Wall()
        };
    }
    
    /// <summary>
    /// Gets the tile at the specified coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the tile.</param>
    /// <param name="y">The y-colordinate of the tile.</param>
    /// <returns>The tile at the specified position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when coordinates are outside the labyrinth bounds.</exception>
    public Tile GetTile(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException();
        return _tiles[y, x];
    }
}
