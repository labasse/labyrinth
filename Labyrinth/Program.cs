// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

const string lab = """
                   +--+--------+
                   |  /        |
                   |  +--+--+  |
                   |     |k    |
                   +--+  |  +--+
                      |k       |
                   +  +-------/|
                   |           |
                   +-----------+
                   """;

Console.WriteLine("Initialisation du labyrinthe :");
Labyrinth.Labyrinth labyrinth = new Labyrinth.Labyrinth(lab);