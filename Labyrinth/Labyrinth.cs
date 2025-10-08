namespace Labyrinth;

public sealed class Labyrinth
{
    public Tile[,] Grid { get; }
    public int Width { get; }
    public int Height { get; }

    public Labyrinth(string multiline)
    {
        if (multiline is null) throw new ArgumentNullException(multiline + "Multiline is null");
        var lines = multiline.Split('\n');
        Height = lines.Length;
        Width = lines[0].Length;
        Grid = new Tile[Width, Height];
        
        for(int y = 0; y < Height; y++)
        {
            if (lines[y].Length != Width) throw new ArgumentException("All lines must have the same length");
            for(int x = 0; x < Width; x++)
            {
                Grid[x, y] = FromChar(lines[y][x]);
            }
        }
    }
    
    private static Tile FromChar(char c) => c switch
    {
        ' ' => new Room(),
        '/' => new Door(),
        'k' => new Room { Item = Key.New() },
        _ => new Wall()
    };
    
    public override string ToString()
    {
        var result = new System.Text.StringBuilder();
        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                result.Append(Grid[x, y] switch
                {
                    Room r when r.Item is Key => 'k',
                    Room => ' ',
                    Door => '/',
                    Wall => '#',
                    _ => '?'
                });
            }
            result.Append('\n');
        }
        return result.ToString();
    }
}