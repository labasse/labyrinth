using Labyrinth;
using Labyrinth.Crawl;

var laby = new Labyrinth.Labyrinth("""
    +--+--------+
    |  /        |
    |  +--+--+  |
    |     |k    |
    +--+  |  +--+
       |k  x    |
    +  +-------/|
    |           |
    +-----------+
    """);

var crawler = laby.NewCrawler();
var explorer = new Explorer(crawler);

// S'abonner aux évènements
explorer.PositionChanged += (s, e) =>
{
    Console.SetCursorPosition(e.X, e.Y);
    Console.Write(GetArrow(e.Direction));
};

explorer.DirectionChanged += (s, e) =>
{
    Console.SetCursorPosition(e.X, e.Y);
    Console.Write(GetArrow(e.Direction));
};

// Démarrer l'exploration
explorer.GetOut(50);

static char GetArrow(Direction d)
{
    if (d == Direction.North) return '^';
    if (d == Direction.East) return '>';
    if (d == Direction.South) return 'v';
    return '<';
}
