using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    public static class AsciiParser
    {
        public static Tile[,] Parse(string asciiMap, out int startX, out int startY)
        {
            var lines = asciiMap.Split("\n,\r\n".Split(','), StringSplitOptions.None);
            var width = lines[0].Length;
            var tiles = new Tile[width, lines.Length];
            
            using var km = new KeyMaster();
            
            int? sx = null, sy = null;

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (lines[y].Length != width)
                {
                    throw new ArgumentException("Invalid map: all lines must have the same length.");
                }
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    var car = lines[y][x];
                    tiles[x, y] = car switch
                    {
                        ' ' or 'x' => new Room(),
                        '+' or '-' or '|' => Wall.Singleton,
                        '/' => km.NewDoor(),
                        'k' => km.NewKeyRoom(),
                        _ => throw new ArgumentException($"Invalid map: unknown character '{lines[y][x]}' at line {y}, col {x}.")
                    };
                    
                    if (car == 'x')
                    {
                        sx = x;
                        sy = y;
                    }
                }
            }

            if (sx == null || sy == null)
            {
                throw new ArgumentException("Invalid map: start position is missing.");
            }
            startX = sx.Value;
            startY = sy.Value;
            return tiles;
        }
    }
}
