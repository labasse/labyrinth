using Labyrinth;

class Program
{
    static void Main()
    {
        string map = @"
+--+--------+
|  /        |
|  +--+--+  |
|     |k    |
+--+  |  +--+
   |k       |
+  +-------/|
|           |
+-----------+";

        var labyrinth = new Labyrinth.Labyrinth(map.Trim());

        Console.WriteLine($"Labyrinthe {labyrinth.Width}x{labyrinth.Height}\n");

        for (int y = 0; y < labyrinth.Height; y++)
        {
            for (int x = 0; x < labyrinth.Width; x++)
            {
                var tile = labyrinth.Tiles[y, x]; // Correction ici
                string type = tile switch
                {
                    Wall => "Mur",
                    Room r when r.Item is Key => "Salle avec clé",
                    Room => "Salle",
                    Door => "Porte",
                    _ => "Inconnu"
                };
                Console.Write($"{type,-15}");
            }
            Console.WriteLine();
        }

        // Test d’interaction avec une porte
        for (int y = 0; y < labyrinth.Height; y++)
        {
            for (int x = 0; x < labyrinth.Width; x++)
            {
                if (labyrinth.Tiles[y, x] is Door door) // Correction ici
                {
                    Console.WriteLine("\nTest de la porte :");
                    try
                    {
                        door.Pass();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    door.Unlock(door.Key);
                    Console.WriteLine("Porte ouverte : " + door.IsTraversable);

                    door.Lock(door.Key);
                    Console.WriteLine("Porte fermée : " + door.IsTraversable);
                }
            }
        }
    }
}