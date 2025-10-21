using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    public class AsciiParser
    {
        public static Tile[,] Parse(string ascii_map, out (int X, int Y)? start)
        {
            var normalized = ascii_map.ReplaceLineEndings("\n");
            var lines = normalized.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                throw new ArgumentException("Invalid map: empty content.");
            }
            var width = lines[0].Length;
            var tiles = new Tile[width, lines.Length];

            using var km = new Keymaster();
            start = null;

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (lines[y].Length != width)
                {
                    throw new ArgumentException("Invalid map: all lines must have the same length.");
                }
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    var current = lines[y][x];
                    tiles[x, y] = current switch
                    {
                        ' ' => new Room(),
                        '+' or '-' or '|' => Wall.Singleton,
                        '/' => km.NewDoor(),
                        'k' => km.NewKeyRoom(),
                        'x' => new Room(),
                        _ => throw new ArgumentException($"Invalid map: unknown character '{current}' at line {y}, col {x}.")
                    };
                    if (current == 'x')
                    {
                        start = (x, y);
                    }
                }
            }
            return tiles;
        }
    }
}
