using Labyrinth.Crawl;
using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    public class AsciiParser
    {
        public static (Tile[,],(int x, int y)) Parse(string ascii_map)
        {
            var lines = ascii_map.Split("\n,\r\n".Split(','), StringSplitOptions.None);
            var width = lines[0].Length;
            var tiles = new Tile[width, lines.Length];
            var spawnPoint = (-1,-1);
            
            using var km = new Keymaster();

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (lines[y].Length != width)
                {
                    throw new ArgumentException("Invalid map: all lines must have the same length.");
                }
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    if (lines[y][x] == 'x') {
                        spawnPoint = (x, y);
                    }
                    tiles[x, y] = lines[y][x] switch {
                   
                        ' ' or 'x' => new Room(),
                        '+' or '-' or '|' => Wall.Singleton,
                        '/' => km.NewDoor(),
                        'k' => km.NewKeyRoom(),
                        _ => throw new ArgumentException($"Invalid map: unknown character '{lines[y][x]}' at line {y}, col {x}.")
                    };
                }
            }
            return spawnPoint == (-1,-1) ? throw new ArgumentException("Invalid map: no spawn point ('x') found.") : (tiles, spawnPoint);
        }
    }
}
