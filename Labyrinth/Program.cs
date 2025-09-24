using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("debut de l'instanciation");
        string labyString =
            "#############\n" +
            "#  /        #\n" +
            "#  ######  ##\n" +
            "#     #k    #\n" +
            "##  #####   #\n" +
            "    #k      #\n" +
            "#  ########/#\n" +
            "#           #\n" +
            "#############";

        Labyrinth labyrinth = new Labyrinth(labyString);

        Console.WriteLine("Labyrinthe instancié avec succès !");
    }
}