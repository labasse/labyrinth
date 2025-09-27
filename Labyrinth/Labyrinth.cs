using System;

// Classe représentant le labyrinthe entier
public class Labyrinth
{
    // La grille du labyrinthe : chaque case est un Tile (Room, Wall ou Door)
    public Tile[,] Grid { get; private set; }

    // Largeur du labyrinthe (nombre de colonnes)
    public int Width { get; private set; }

    // Hauteur du labyrinthe (nombre de lignes)
    public int Height { get; private set; }

    // Constructeur qui prend une chaîne de texte représentant le labyrinthe
    public Labyrinth(string map)
    {
        // On découpe le texte en lignes
        var lines = map.Split('\n');

        // La hauteur du labyrinthe = nombre de lignes
        Height = lines.Length;

        // On calcule la largeur max des lignes
        Width = 0;
        for (int i = 0; i < lines.Length; i++)
            if (lines[i].Length > Width) Width = lines[i].Length;

        // On crée la grille vide avec la taille Height x Width
        Grid = new Tile[Height, Width];

        // On parcourt chaque ligne
        for (int y = 0; y < Height; y++)
        {
            var line = lines[y]; // récupérer la ligne y

            // On parcourt chaque caractère de la ligne
            for (int x = 0; x < Width; x++)
            {
                // Si la ligne est plus courte que Width, on met un espace par défaut
                char c = x < line.Length ? line[x] : ' ';

                // Selon le caractère, on crée la case correspondante
                switch (c)
                {
                    case '+': // Mur
                        Grid[y, x] = new Wall(); break;

                    case 'k': // Salle contenant une clé
                        Grid[y, x] = new Room(new Key("doorKey")); break;

                    case '/': // Porte
                        Grid[y, x] = new Door(new Key("doorKey")); break;

                    case ' ': // Salle vide (traversable)
                    default:
                        Grid[y, x] = new Room(); break;
                }
            }
        }
    }
}


