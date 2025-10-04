namespace Labyrinth
{
    public class Labyrinth
    {
        private const char WallCorner = '+';
        private const char WallHorizontal = '-';
        private const char WallVertical = '|';
        private const char DoorSymbol = '/';
        private const char KeyRoomSymbol = 'k';
        private const char EmptySpace = ' ';
        
        public int Width { get; }
        public int Height { get; }
        private readonly Tile[,] Tiles;

        public Labyrinth(string map)
        {
            ValidateMap(map);
            
            var lines = map.Replace("\r", string.Empty).Split('\n');
            Height = lines.Length;
            Width = lines[0].Length;
            Tiles = new Tile[Height, Width];
            
            InitializeTiles(lines);
        }

        private void ValidateMap(string map)
        {
            if (string.IsNullOrEmpty(map))
                throw new ArgumentException("La carte ne peut pas être vide.", nameof(map));
                
            var lines = map.Replace("\r", string.Empty).Split('\n');
            if (lines.Length == 0)
                throw new ArgumentException("La carte doit contenir au moins une ligne.", nameof(map));
                
            var width = lines[0].Length;
            if (width == 0)
                throw new ArgumentException("La carte doit avoir une largeur positive.", nameof(map));
                
            if (lines.Any(line => line.Length != width))
                throw new ArgumentException("Toutes les lignes doivent avoir la même longueur.", nameof(map));
        }

        private void InitializeTiles(string[] lines)
        {
            var doors = new List<(int x, int y, Door door)>();
            var keyRooms = new List<(int x, int y, Room room)>();
            
            // Création des tuiles
            for (var y = 0; y < Height; y++)
            {
                var line = lines[y];
                for (var x = 0; x < Width; x++)
                {
                    var ch = line[x];
                    Tiles[y, x] = CreateTile(ch);

                    if (Tiles[y, x] is Door door)
                    {
                        doors.Add((x, y, door));
                    }
                    else if (ch == KeyRoomSymbol && Tiles[y, x] is Room room)
                    {
                        keyRooms.Add((x, y, room));
                    }
                }
            }

            AssociateKeysWithDoors(doors, keyRooms);
        }

        private static Tile CreateTile(char symbol) => symbol switch
        {
            WallCorner or WallHorizontal or WallVertical => new Wall(),
            DoorSymbol => new Door(),
            KeyRoomSymbol => new Room(),
            _ => new Room()
        };

        private static void AssociateKeysWithDoors(
            List<(int x, int y, Door door)> doors,
            List<(int x, int y, Room room)> keyRooms)
        {
            foreach (var (doorX, doorY, door) in doors)
            {
                var closestKeyRoom = FindClosestAvailableKeyRoom(doorX, doorY, keyRooms);
                if (closestKeyRoom.room != null)
                {
                    closestKeyRoom.room.Item = door.Key;
                }
            }
        }

        private static (int x, int y, Room room) FindClosestAvailableKeyRoom(
            int doorX,
            int doorY,
            List<(int x, int y, Room room)> keyRooms)
        {
            return keyRooms
                .Where(kr => kr.room.Item == null)
                .OrderBy(kr => CalculateManhattanDistance(doorX, doorY, kr.x, kr.y))
                .FirstOrDefault();
        }

        private static int CalculateManhattanDistance(int x1, int y1, int x2, int y2)
            => Math.Abs(x2 - x1) + Math.Abs(y2 - y1);

        public Tile GetTile(int x, int y)
        {
            ValidateCoordinates(x, y);
            return Tiles[y, x];
        }

        private void ValidateCoordinates(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), "La coordonnée X est hors des limites.");
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), "La coordonnée Y est hors des limites.");
        }
    }
}
