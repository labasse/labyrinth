using System;
namespace Labyrinth;

public class Labyrinth
{
    public Tile[,] Tiles { get; set; }
    public int Width { get; }
    public int Height { get; }

    public Labyrinth(string map)
    {
        var lines = map.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Height = lines.Length;
        Width = lines[0].Length;
        Tiles = new Tile[Height, Width];

        for (int y = 0; y < Height; y++)
        {
            var line = lines[y].PadRight(Width, ' ');
            for (int x = 0; x < Width; x++)
            {
                char c = line[x];
                Tiles[y, x] = c switch
                {
                    '+' or '-' or '|' => new Wall(),
                    ' ' => new Room(),
                    'k' => new Room { Item = new Key() },
                    '/' => new Door(),
                    _ => new Room()
                };
            }
        }
    }

    public override string ToString()
    {
        var result = new System.Text.StringBuilder();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var tile = Tiles[y, x];
                char c = tile switch
                {
                    Wall => Tiles[y, x] is Wall ? GetWallChar(y, x) : '#',
                    Room r when r.Item is Key => 'k',
                    Room => ' ',
                    Door => '/',
                    _ => '?'
                };
                result.Append(c);
            }
            result.AppendLine();
        }
        return result.ToString();
    }

    private char GetWallChar(int y, int x)
    {
        return '+';
    }
}