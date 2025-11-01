using Labyrinth.Events;
using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    public class AsciiParser
    {
        // Définition de l’événement
        public static event EventHandler<StartEventArgs>? StartPositionFound;
        
        public static Tile[,] Parse(string ascii_map)
        {
            var lines = ascii_map.Split("\n,\r\n".Split(','), StringSplitOptions.None);
            var width = lines[0].Length;
            var tiles = new Tile[width, lines.Length];
            
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
                        'x' => HandleStartPos(x, y),
                        ' ' => new Room(),
                        'O' => Outside.Singleton,
                        '+' or '-' or '|' => Wall.Singleton,
                        '/' => km.NewDoor(),
                        'k' => km.NewKeyRoom(),
                        _ => throw new ArgumentException($"Invalid map: unknown character '{lines[y][x]}' at line {y}, col {x}.")
                    };
                }
            }
            return tiles;
        }

        private static Room HandleStartPos(int x, int y)
        {
            // Lève l’événement quand on trouve 'x'
            StartPositionFound?.Invoke(null, new StartEventArgs(x, y));
            return new Room();
        }
    }
}
