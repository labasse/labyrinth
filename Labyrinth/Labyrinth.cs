using Labyrinth.Collectable;
using Labyrinth.Tile;

namespace Labyrinth;

public class Labyrinth
{
    private int Width { get; }
    private int Height { get; }

    private Tile.Tile[][] Tiles { get; set; }
    
    public Labyrinth(string labyrinth)
    {
        Key key = new Key(Guid.NewGuid());
        string[] rows = labyrinth.Split('\n');
        Width = rows[0].Length;
        Height = rows.Length;
        Console.WriteLine($"Width={Width}, Height={Height}");
        InitLab();
        for (var i = 0; i < Height; i++)
        {
            Console.Write("\n");
            string row = rows[i];
            for (var j = 0; j < row.Length; j++)
            {
                Console.Write(row[j]);
                switch (row[j])
                {
                    case ' ':
                        Tiles[i][j] = new Room(null);
                        break;
                    case 'k':
                        Tiles[i][j] = new Room(key);
                        break;
                    case '/':
                        Tiles[i][j] = new Door(key);
                        break;
                    default:
                        Tiles[i][j] = new Wall();
                        break;
                }
            }
        }
        Console.WriteLine();
    }

    private void InitLab()
    {
        // Création du tableau en "jagged array"
        Tiles = new Tile.Tile[Height][];
        for (int i = 0; i < Height; i++)
        {
            Tiles[i] = new Tile.Tile[Width];
        }
    }
}