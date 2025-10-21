using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    /// <summary>
    /// Parses ASCII representations of labyrinth layouts into the in-memory tile grid.
    /// </summary>
    public class AsciiParser
    {
        /// <summary>
        /// Converts an ASCII map into tiles and locates the crawler starting point.
        /// </summary>
        /// <param name="ascii_map">Multi-line string containing the ASCII map to parse.</param>
        /// <returns>A tuple with the tile grid and coordinates of the start position.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the map lines have inconsistent lengths or contain unexpected characters.
        /// </exception>
        public static (Tile[,] tiles, int startX, int startY) Parse(string ascii_map)
        {
            var lines = ascii_map.Split("\n,\r\n".Split(','), StringSplitOptions.None);
            var width = lines[0].Length;
            var tiles = new Tile[width, lines.Length];

            int startX = -1, startY = -1;

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
                        'x' => new Room(),
                        _ => throw new ArgumentException($"Invalid map: unknown character '{lines[y][x]}' at line {y}, col {x}.")
                    };

                    if (lines[y][x] == 'x')
                    {
                        startX = x;
                        startY = y;
                    }
                }
            }
            if (startX == -1 || startY == -1)
            {
                throw new ArgumentException("Invalid map");
            }

            return (tiles, startX, startY);
        }
    }
}
