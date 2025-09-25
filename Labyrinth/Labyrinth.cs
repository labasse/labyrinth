using Labyrinth.Collectable;
using Labyrinth.Tile;

namespace Labyrinth;

public class Labyrinth
{
    private int Width { get; }
    private int Height { get; }
    
    public Tile.Tile[][] Tiles { get; }
    
    public Labyrinth(string labyrinth)
    {
        Key key = new Key(Guid.NewGuid());
        string[] rows = labyrinth.Split('\n');
        Width = rows.Length;
        Height = rows[0].Length;
        for (var i = 0; i < Height; i++)
        {
            string row = rows[i];
            Console.WriteLine($"Ligne {i + 1}");
            for (var j = 0; j < row.Length; j++)
            {
                switch (row[j])
                {
                    case ' ':
                        Tiles[j][i] = new Room(null);
                        break;
                    case 'k':
                        Tiles[j][i] = new Room(key);
                        break;
                    case '/':
                        Tiles[j][i] = new Door(key);
                        break;
                    default:
                        Tiles[j][i] = new Wall();
                        break;
                }
            }
        }
    }
}