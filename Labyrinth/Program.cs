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

CrawlingEventArgs? lastPoint = null;

explorer.PositionChanged += (s, e) =>
{
    if (lastPoint != null)
    {
        Console.SetCursorPosition(e.X, e.Y);
        Console.Write(" ");
    }

    Console.SetCursorPosition(e.X, e.Y);
    Console.Write(GetDirectionChar(e.Direction));
    lastPoint = e;
};

explorer.DirectionChanged += (s, e) =>
{
    if (lastPoint != null)
    {
        Console.SetCursorPosition(e.X, e.Y);
        Console.Write(" ");
    }

    Console.SetCursorPosition(e.X, e.Y);
    Console.Write(GetDirectionChar(e.Direction));
    lastPoint = e;
};

static char GetDirectionChar(Direction dir)
{
    if (dir == Direction.North) return '^';
    if (dir == Direction.East) return '>';
    if (dir == Direction.South) return 'v';
    if (dir == Direction.West) return '<';
    return '?';
}


Console.Clear();
Console.WriteLine(l);

explorer.GetOut(5);
