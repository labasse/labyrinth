using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    public class AsciiParser
    {
        public static (Tile[,] tiles, int startX, int startY) Parse(string ascii_map)
        {
            var lines = ascii_map.Split("\n,\r\n".Split(','), StringSplitOptions.None);
            var width = lines[0].Length;
            var tiles = new Tile[width, lines.Length];
            
            using var km = new Keymaster();
            
            int startX = -1;
            int startY = -1;

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (lines[y].Length != width)
                {
                    throw new ArgumentException("Invalid map: all lines must have the same length.");
                }
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    tiles[x, y] = lines[y][x] switch
                    {
                        ' ' => new Room(),
                        'x' => new Room(),
                        '+' or '-' or '|' => Wall.Singleton,
                        '/' => km.NewDoor(),
                        'k' => km.NewKeyRoom(),
                        _ => throw new ArgumentException($"Invalid map: unknown character '{lines[y][x]}' at line {y}, col {x}.")
                    };
                    
                    // Store starting position (last 'x' found)
                    if (lines[y][x] == 'x')
                    {
                        startX = x;
                        startY = y;
                    }
                }
            }
            
            if (startX == -1 || startY == -1)
            {
                throw new ArgumentException("Invalid map: no starting position 'x' found.");
            }
            
            return (tiles, startX, startY);
        }
    }
}
