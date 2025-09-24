namespace Labyrinth;

public class Labyrinth
{
    private readonly Tile[,] _tiles;

    public int Width { get; }
    public int Height { get; }

    public Labyrinth(string multilineMap)
    {
        string[] lines = multilineMap.Split('\n');
        
        lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        
        Height = lines.Length;
        Width = lines[0].Length;
        
        _tiles = new Tile[Width, Height];
        
        for (int y = 0; y < Height; y++)
        {
            string line = lines[y];
            for (int x = 0; x < Width; x++)
            {
                if (x < line.Length)
                {
                    _tiles[x, y] = CreateTile(line[x]);
                }
                else
                {
                    
                    _tiles[x, y] = new Wall();
                }
            }
        }
    }
    
    private Tile CreateTile(char c)
    {
        return c switch
        {
            '+' or '-' or '|' => new Wall(), 
            '/' => new Door(), 
            'k' => new Room(new Key()), 
            ' ' or '_' => new Room(), 
            _ => new Wall() 
        };
    }
    
    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return _tiles[x, y];
        }
        
        throw new ArgumentOutOfRangeException(
            $"Position ({x}, {y}) is outside the labyrinth bounds of ({Width}, {Height}).");
    }
    
    public void SetTile(int x, int y, Tile tile)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            _tiles[x, y] = tile;
        }
        else
        {
            throw new ArgumentOutOfRangeException(
                $"Position ({x}, {y}) is outside the labyrinth bounds of ({Width}, {Height}).");
        }
    }

    public override string ToString()
    {
        var result = new System.Text.StringBuilder();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile = _tiles[x, y];
                if (x == 0 && y > 0)
                {
                    result.Append('\n');
                }
                result.Append(tile.ToString());
            }
        }

        return result.ToString();
    }
}
