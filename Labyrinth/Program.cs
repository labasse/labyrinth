using Labyrinth;
using Labyrinth.Crawl;

var l = new Labyrinth.Labyrinth("""
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


var explorer = new RandomExplorer(l.NewCrawler());

explorer.PositionChanged += (s, e) =>
{
    Console.SetCursorPosition(e.X, e.Y);
    Console.Write(GetDirectionChar(e.Direction));
};

explorer.DirectionChanged += (s, e) =>
{
    Console.SetCursorPosition(e.X, e.Y);
    Console.Write(GetDirectionChar(e.Direction));
};

static char GetDirectionChar(Direction dir)
{
    if (dir == Direction.North) return '^';
    if (dir == Direction.East) return '>';
    if (dir == Direction.South) return 'v';
    if (dir == Direction.West) return '<';
    return '?';
}

explorer.GetOut(5);
