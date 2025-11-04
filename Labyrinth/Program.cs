using Labyrinth.Crawl;
using Labyrinth.Explore;

class Program
{
    static void Main()
    {
        var map = """
            +--+--------+
            |  /        |
            |  +--+--+  |
            |     |k    |
            +--+  |  +--+
               |k  x    |
            +  +-------/|
            |           |
            +-----------+
            """;

        var lab = new Labyrinth.Labyrinth(map);
        var crawler = lab.NewCrawler();

        var explorer = new Explorer(crawler);

        Console.Clear();
        Console.Write(lab.ToString());

        explorer.PositionChanged += (_, e) =>
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
        };

        explorer.DirectionChanged += (_, e) =>
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
        };

        explorer.GetOut(200);

        Console.SetCursorPosition(0, lab.Height + 2);
        Console.WriteLine("Terminé. Appuyez sur une touche.");
        Console.ReadKey();
    }
}
