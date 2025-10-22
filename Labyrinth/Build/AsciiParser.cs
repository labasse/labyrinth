using Labyrinth.Tiles;
using System;

namespace Labyrinth.Build
{
    public class AsciiParser
    {
        public static Tile[,] Parse(string ascii_map, out int? startX, out int? startY)
        {
            var lines = ascii_map.Split("\n,\r\n".Split(','), StringSplitOptions.None);

            // Recherche de la position de départ 'x'
            startX = null;
            startY = null;
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'x')
                    {
                        startX = x;
                        startY = y;
                        // Ne pas break : on veut la dernière occurrence si plusieurs (convention existante)
                    }
                }
            }

            // Préparer des lignes nettoyées (remplacer 'x' par espace) pour le parsing
            var cleanedLines = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                cleanedLines[i] = lines[i].Replace('x', ' ');
            }

            var width = cleanedLines[0].Length;
            var tiles = new Tile[width, cleanedLines.Length];
            
            using var km = new Keymaster();

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (cleanedLines[y].Length != width)
                {
                    throw new ArgumentException("Invalid map: all lines must have the same length.");
                }
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    tiles[x, y] = cleanedLines[y][x] switch
                    {
                        ' ' => new Room(),
                        '+' or '-' or '|' => Wall.Singleton,
                        '/' => km.NewDoor(),
                        'k' => km.NewKeyRoom(),
                        _ => throw new ArgumentException($"Invalid map: unknown character '{cleanedLines[y][x]}' at line {y}, col {x}.")
                    };
                }
            }
            return tiles;
        }
    }
}
