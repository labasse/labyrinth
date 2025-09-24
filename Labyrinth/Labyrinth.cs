namespace Labyrinth
{
    public class Labyrinth
    {
        public int Width { get; }
        public int Height { get; }
        public Tile[,] Tiles { get; }

        public Labyrinth(string map)
        {
            var lines = map.Replace("\r", string.Empty).Split('\n');
            Height = lines.Length;
            Width = Height > 0 ? lines[0].Length : 0;
            Tiles = new Tile[Height, Width];
            for (var y = 0; y < Height; y++)
            {
                var line = lines[y];
                for (var x = 0; x < Width; x++)
                {
                    var ch = x < line.Length ? line[x] : ' ';
                    Tiles[y, x] = ch switch
                    {
                        '+' or '-' or '|' => new Wall(),
                        '/' => new Door(),
                        'k' => new Room { Item = new Key() },
                        _ => new Room()
                    };
                }
            }
        }

    }
}

