using Labyrinth.Crawl;
using Labyrinth.Models;
using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    public class AsciiParser
    {
        
        public static Tile[,] Parse(string ascii_map, out Coord initial_coord, out Direction initial_direction)
        {
            Coord? crawler_coord = null;
            Direction? crawler_direction = null;
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
                    char c = lines[y][x];
                    Tile tile;

                    if ("↑→↓←".Contains(c))
                    {
                        crawler_coord = new Coord(x, y);
                        crawler_direction = c switch
                        {
                            '↑' => Direction.North,
                            '→' => Direction.East,
                            '↓' => Direction.South,
                            '←' => Direction.West,
                        };
                        tile = new Room();
                    }
                    else
                    {
                        tile = c switch
                        {
                            ' ' => new Room(),
                            '+' or '-' or '|' => Wall.Singleton,
                            '/' => km.NewDoor(),
                            'k' => km.NewKeyRoom(),
                            _ => throw new ArgumentException($"Invalid map: unknown character '{c}' at line {y}, col {x}.")
                        };
                    }
                    
                    tiles[x, y] = tile;
                }
            }

            initial_coord = crawler_coord ?? throw new ArgumentException("Labyrinth must contains one crawler");
            initial_direction = crawler_direction!;
            
            return tiles;
        }
    }
}