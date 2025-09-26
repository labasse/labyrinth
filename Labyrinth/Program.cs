public interface Collectable { }

public class Key : Collectable
{
    public override string ToString()
    {
        return "k";
    }
}

public abstract class Tile
{
    public abstract bool IsTraversable { get; }

    public abstract void Pass();
}

public class Wall : Tile
{
    public override bool IsTraversable => false;

    public override void Pass()
    {
        Console.WriteLine("Vous heurtez un mur. Vous ne pouvez pas passer.");
    }

    public override string ToString()
    {
        return "#";
    }
}

public class Room : Tile
{
    public Collectable? Item { get; set; }

    public override bool IsTraversable => true;

    public override void Pass()
    {
        Console.WriteLine("Vous entrez dans la salle.");
    }

    public override string ToString()
    {
        return Item?.ToString() ?? " ";
    }
}

public class Door : Tile
{
    private bool isOpen;

    public override bool IsTraversable => isOpen;

    public Key Key { get; }


    public Door()
    {
        Key = new Key();
        isOpen = false;
    }

    public override void Pass()
    {
        if (isOpen)
        {
            Console.WriteLine("Vous passez à travers la porte ouverte.");
        }
        else
        {
            Console.WriteLine("La porte est fermée. Vous avez besoin de la bonne clé pour l'ouvrir.");
        }
    }

    public override string ToString()
    {
        return "/";
    }

    public void Close()
    {
        isOpen = false;
        Console.WriteLine("La porte est maintenant fermée.");
    }
}

public class Labyrinth
{
    public Tile[,] Tiles { get; }

    public Labyrinth(string input)
    {
        string[] lines = input.Split('\n');

        int height = lines.Length;
        int width = lines[0].Length;

        Tiles = new Tile[height, width];

        for (int i = 0; i < height; i++)
        {
            string line = lines[i];

            for (int j = 0; j < line.Length; j++)
            {
                char symb = line[j];

                var tile = charToTile(line[j]);

                Tiles[i, j] = tile;
            }
        }
    }

    private Tile charToTile(char symb)
    {
        if (symb == '/')
        {
            return new Door();
        }

        if (symb == '-' || symb == '+' || symb == '|')
        {
            return new Wall();
        }

        if (symb == ' ')
        {
            return new Room();
        }

        if (symb == 'k')
        {
            Room r = new Room();

            r.Item = new Key();

            return r;
        }

        throw new Exception("unhandled symbole " + symb);
    }

    public override string ToString()
    {
        string res = "";

        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                res += Tiles[i, j].ToString();
            }
            res += "\n";
        }

        return res;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var l = new Labyrinth(
            @"+--+--------+
|  /        |
|  +--+--+  |
|     |k    |
+--+  |  +--+
   |k       |
+  +-------/|
|           |
+-----------+"
        );

        Console.WriteLine(l);
    }
}
