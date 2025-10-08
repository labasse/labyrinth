using Labyrinth.Models.Tiles;
using Labyrinth.Models.Collectables;

namespace Labyrinth.Models;

public class Labyrinth
{
    public int Width { get; }
    public int Height { get; }

    private readonly Tile[,] labs;
    public Tile this[int x, int y] => labs[y, x];

    public Labyrinth(string multiLine)
    {
        if (multiLine is null) throw new ArgumentNullException(nameof(multiLine));

        //on sépare la chaîne de caractères en lignes en se basant sur les retours à la ligne
        var s = multiLine.Split('\n').ToList();

        //on enlève les lignes vides au début et à la fin (au cas ou)
        while (s.Count > 0 && string.IsNullOrWhiteSpace(s[0])) s.RemoveAt(0);
        while (s.Count > 0 && string.IsNullOrWhiteSpace(s[^1])) s.RemoveAt(s.Count - 1);
        if (s.Count == 0) throw new ArgumentException("Carte vide.", nameof(multiLine));

        //on calcule l'indentation commune
        int CommonIndent(string s)
        {
            int i = 0;
            while (i < s.Length && (s[i] == ' ' || s[i] == '\t')) i++;
            return string.IsNullOrWhiteSpace(s) ? int.MaxValue : i;
        }
        int indent = s.Where(l => !string.IsNullOrWhiteSpace(l))
                        .Select(CommonIndent)
                        .DefaultIfEmpty(0)
                        .Min();
        if (indent == int.MaxValue) indent = 0;

        //on enlève l'indentation commune (pour avoir un width correct)
        var lines = s.Select(l => l.Length >= indent ? l[indent..] : l).ToList();

        //on a maintenant des valeurs sûres pour width et height
        string firstNonEmpty = lines.First(l => l.Length > 0);
        Width  = firstNonEmpty.Length;
        Height = lines.Count;

        //on vérifie que toutes les lignes ont la même largeur
        for (int i = 0; i < lines.Count; i++)
            if (lines[i].Length != Width)
                throw new FormatException($"Ligne {i + 1}: largeur {lines[i].Length} différente de {Width}.");


        //on remplit le tableau de tiles en appelant FromChar pour chaque caractère
        labs = new Tile[Height, Width];

        for (int y = 0; y < Height; y++)
        {
            var line = lines[y];
            for (int x = 0; x < Width; x++)
            {
                labs[y, x] = FromChar(line[x]);
            }
        }
    }

    private static Tile FromChar(char c) => c switch
    {
        ' ' => new Room(),                 
        'k' => new Room { Item = new Key() }, 
        '/' => new Door(),                 
        '+'
        or '-'
        or '|' => new Wall(),              
        _ => new Wall(),                  
    };
}
