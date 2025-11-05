using Labyrinth.Crawl;
using Labyrinth.Events;
using Labyrinth.Exploration;

namespace Labyrinth;

internal static class Program
{
    private static void Main()
    {
        const string map = """
                           +--+--------+
                           |  /        |
                           |  +--+--+  |
                           |     |k    |
                           +--+  |  +--+
                              |k       |
                           +  +-------/|
                           |      x    |
                           +-----------+
                           """;

        var lab = new global::Labyrinth.Labyrinth(map);
        var crawler = lab.NewCrawler();
        var explorer = new Explorer(crawler);

        Console.Clear();
        Console.Write(lab.ToString());

        explorer.PositionChanged += (_, e) =>
        {
            DrawCrawler(e);
        };

        explorer.DirectionChanged += (_, e) =>
        {
            DrawCrawler(e);
        };

        explorer.ExitFound += (_, e) =>
        {
            Console.SetCursorPosition(0, lab.Height + 1);
            Console.WriteLine($"Sortie trouvée : ({e.X}, {e.Y})");
        };

        explorer.GetOut(100);

        Console.SetCursorPosition(0, lab.Height + 2);
        Console.WriteLine("Exploration terminée");
    }

    private static void DrawCrawler(CrawlingEventArgs e)
    {
        var symbol = e.Direction switch
        {
            var d when d == Direction.North => '^',
            var d when d == Direction.East  => '>',
            var d when d == Direction.South => 'v',
            var d when d == Direction.West  => '<',
            _ => '?'
        };

        Console.SetCursorPosition(e.X, e.Y);
        Console.Write(symbol);
    }
}