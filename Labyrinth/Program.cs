using System;

class Program
{
    static void Main()
    {
        string map = @"+--+--------+
|  /        |
|  +--+--+  |
|     |k    |
+--+  |  +--+
   |k       |
+  +-------/|
|           |
+-----------+";

        Labyrinth lab = new Labyrinth(map);

        // Affichage de la grille pour vérifier
        Console.WriteLine("Grille du labyrinthe :");
        for (int y = 0; y < lab.Height; y++)
        {
            for (int x = 0; x < lab.Width; x++)
            {
                if (lab.Grid[y, x] is Wall) Console.Write("+");
                else if (lab.Grid[y, x] is Door) Console.Write("/");
                else if (lab.Grid[y, x] is Room r && r.Item != null) Console.Write("k");
                else Console.Write(" ");
            }
            Console.WriteLine();
        }

        // Chercher la salle avec la clé
        Room? roomWithKey = null;
        Key? key = null;

for (int y = 0; y < lab.Height; y++)
{
    for (int x = 0; x < lab.Width; x++)
    {
        if (lab.Grid[y, x] is Room r && r.Item is Key)
        {
            roomWithKey = r;
            key = (Key)r.Item; // récupère la clé ici
            break;
        }
    }
    if (roomWithKey != null) break;
}

if (roomWithKey == null || key == null)
{
    Console.WriteLine("Aucune clé trouvée !");
    return;
}

roomWithKey.Item = null; // ramasser la clé
Console.WriteLine("Clé récupérée : " + key.Name);


        // Chercher la porte
        Door? door = null;
        for (int y = 0; y < lab.Height; y++)
        {
            for (int x = 0; x < lab.Width; x++)
            {
                if (lab.Grid[y, x] is Door d)
                {
                    door = d;
                    break;
                }
            }
            if (door != null) break;
        }

        if (door == null)
        {
            Console.WriteLine("Aucune porte trouvée !");
            return;
        }

        // Test porte fermée
        Console.WriteLine("\nTEST 1 : Essayer de passer une porte FERMEE");
        try
        {
            door.Pass();
            Console.WriteLine("ERREUR : tu as traversé une porte fermée !");
        }
        catch (Exception e)
        {
            Console.WriteLine("OK : exception reçue -> " + e.Message);
        }

        // Déverrouiller et traverser
        door.Unlock(key);
        Console.WriteLine("\nTEST 2 : Porte ouverte ? " + door.IsOpen);
        try
        {
            door.Pass();
            Console.WriteLine("OK : tu as traversé la porte !");
        }
        catch (Exception e)
        {
            Console.WriteLine("ERREUR : " + e.Message);
        }

        // Verrouiller et retenter
        door.Lock();
        Console.WriteLine("\nTEST 3 : Porte ouverte après verrouillage ? " + door.IsOpen);
        try
        {
            door.Pass();
            Console.WriteLine("ERREUR : tu as traversé une porte verrouillée !");
        }
        catch (Exception e)
        {
            Console.WriteLine("OK : exception reçue -> " + e.Message);
        }

        // Traverser un mur
        Wall wall = new Wall();
        Console.WriteLine("\nTEST 4 : Essayer de traverser un mur");
        try
        {
            wall.Pass();
            Console.WriteLine("ERREUR : tu as traversé un mur !");
        }
        catch (Exception e)
        {
            Console.WriteLine("OK : exception reçue -> " + e.Message);
        }

        Console.WriteLine("\nFIN DES TESTS");
    }
}
