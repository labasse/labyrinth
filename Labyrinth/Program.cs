// See https://aka.ms/new-console-template for more information

using Labyrinth;

Labyrinth.Labyrinth labyrinth = new Labyrinth.Labyrinth("""
                                                       +--+--------+
                                                       |  /        |
                                                       |  +--+--+  |
                                                       |     |k    |
                                                       +--+  |  +--+
                                                          |k       |
                                                       +  +-------/|
                                                       |           |
                                                       +-----------+
                                                       """);

// Affichage du labyrinthe
Console.WriteLine("Voici le labyrinthe :");
Console.WriteLine(labyrinth.ToString());
