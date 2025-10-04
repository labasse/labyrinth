using System;

namespace Labyrinth
{
    public class Program
    {
        private const string MAZE_LAYOUT = @"+--+--------+
|  /        |
|  +--+--+  |
|     |k    |
+--+  |  +--+
   |k       |
+  +-------/|
|           |
+-----------+";

        public static void Main()
        {
            Console.WriteLine("Démarrage de la démonstration du labyrinthe\n");
            var labyrinth = new Labyrinth(MAZE_LAYOUT);
            
            Console.WriteLine("État initial du labyrinthe :");
            DisplayMaze(labyrinth);
            
            var door = labyrinth.GetTile(3, 1) as Door;
            if (door != null)
            {
                Console.WriteLine("\nPorte trouvée !");
                Console.WriteLine("ID de la clé attendue par la porte : " + door.Key.Id);
                Console.WriteLine("État de la porte : " + (door.IsTraversable ? "ouverte" : "fermée"));
                
                try
                {
                    door.Pass();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Impossible de passer : la porte est verrouillée.");
                }

                Console.WriteLine("\nRecherche de la clé...");
                bool keyFound = false;
                for (int y = 0; y < labyrinth.Height && !keyFound; y++)
                {
                    for (int x = 0; x < labyrinth.Width && !keyFound; x++)
                    {
                        if (labyrinth.GetTile(x, y) is Room room && room.Item is Key key)
                        {
                            Console.WriteLine($"\nClé trouvée à la position ({x},{y}) !");
                            Console.WriteLine("ID de la clé trouvée : " + key.Id);
                            Console.WriteLine("Tentative de déverrouillage...");
                            
                            if (door.TryUnlock(key))
                            {
                                Console.WriteLine("Déverrouillage réussi ! Les IDs correspondent.");
                                Console.WriteLine("État de la porte : " + (door.IsTraversable ? "ouverte" : "fermée"));
                                
                                Console.WriteLine("\nNouvelle tentative de passage...");
                                door.Pass();
                                Console.WriteLine("Passage réussi !");
                                keyFound = true;
                            }
                            else
                            {
                                Console.WriteLine("Cette clé ne correspond pas à la porte (IDs différents).");
                            }
                        }
                    }
                }

                if (!keyFound)
                {
                    Console.WriteLine("Impossible de trouver la bonne clé !");
                }
            }
            else
            {
                Console.WriteLine("Impossible de trouver la porte !");
            }
        }

        private static void DisplayMaze(Labyrinth labyrinth)
        {
            var originalMaze = MAZE_LAYOUT.Split('\n');
            for (int y = 0; y < labyrinth.Height; y++)
            {
                for (int x = 0; x < labyrinth.Width; x++)
                {
                    var tile = labyrinth.GetTile(x, y);
                    char symbol = tile switch
                    {
                        Wall => originalMaze[y][x],
                        Door => '/',
                        Room room => room.Item is Key ? 'k' : ' ',
                        _ => '?'
                    };
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }
    }
}
