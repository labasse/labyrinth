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
}