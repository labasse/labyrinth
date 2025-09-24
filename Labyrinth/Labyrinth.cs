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

            // Association des clés de porte aux Room adjacentes
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                if (Tiles[y, x] is Door door)
                {
                    (int dx, int dy)[] dirs = { (-1,0), (1,0), (0,-1), (0,1) };
                    foreach (var (dx, dy) in dirs)
                    {
                        int nx = x + dx, ny = y + dy;
                        if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && Tiles[ny, nx] is Room room && room.Item == null)
                        {
                            room.Item = door.Key;
                            break;
                        }
                    }
                }
            }
        }

        public Tile GetTileAt(int x, int y) => Tiles[y, x];

    }
}
