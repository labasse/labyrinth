using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    public class AsciiParser
    {
        private static Tile CreateStartRoom(int x, int y, out int outStartX, out int outStartY)
        {
            outStartX = x;
            outStartY = y;
            return new Room();
        }

        public static Tile[,] Parse(string ascii_map, out int outStartX, out int outStartY)
        {
            var lines = ascii_map.Split("\n,\r\n".Split(','), StringSplitOptions.None);
            var width = lines[0].Length;
            var tiles = new Tile[width, lines.Length];
            outStartX = -1;
            outStartY = -1;

            using var km = new Keymaster();

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
                        '+' or '-' or '|' => Wall.Singleton,
                        '/' => km.NewDoor(),
                        'k' => km.NewKeyRoom(),
                        'x' => CreateStartRoom(x, y, out outStartX, out outStartY),
                        _ => throw new ArgumentException($"Invalid map: unknown character '{lines[y][x]}' at line {y}, col {x}.")
                    };
                }
            }
            return tiles;
        }
    }
}
