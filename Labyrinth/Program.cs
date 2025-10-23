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
var lines = l.ToString().Split('\n', StringSplitOptions.None);

CrawlingEventArgs? lastPoint = null;

explorer.PositionChanged += (s, e) =>
{
    if (lastPoint != null)
    {
        ClearLine(lines, lastPoint);
    }

    Console.SetCursorPosition(e.X, e.Y);
    Console.Write(GetDirectionChar(e.Direction));
    lastPoint = e;
};

explorer.DirectionChanged += (s, e) =>
{
    if (lastPoint != null)
    {
        ClearLine(lines, lastPoint);
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

static void ClearLine(string[] lines, CrawlingEventArgs lastPoint)
{
    var line = lines[lastPoint.Y];
    Console.SetCursorPosition(lastPoint.X, lastPoint.Y);
    Console.Write(line[lastPoint.X]);
}
