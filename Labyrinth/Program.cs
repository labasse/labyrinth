using Labyrinth;
using Labyrinth.Crawl;

var labyrinth = new Labyrinth.Labyrinth("""
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
var crawler = labyrinth.NewCrawler();
var explorer = new Explorer(crawler);

Console.Clear();
Console.WriteLine(labyrinth.ToString());

int prevX = crawler.X, prevY = crawler.Y;

DrawExplorer(crawler.X, crawler.Y, crawler.Direction);

explorer.PositionChanged += (_, e) =>
{
    DrawExplorer(e.X, e.Y, e.Direction);
};

explorer.DirectionChanged += (_, e) =>
{
    DrawExplorer(e.X, e.Y, e.Direction);
};

var reached = explorer.GetOut(100);

Console.SetCursorPosition(0, labyrinth.ToString().Split('\n').Length + 1);
Console.WriteLine(reached ? "Exit found!" : "No exit.");

return 0;


void DrawExplorer(int x, int y, Direction dir)
{
    Console.SetCursorPosition(prevX, prevY);
    Console.Write(" ");
    
    Console.SetCursorPosition(x, y);
    Console.Write(GetDirectionSymbol(dir));
    Console.SetCursorPosition(0, 0);
    
    prevX = x;
    prevY = y;
}

static char GetDirectionSymbol(Direction d)
{
    if (d == Direction.North) return '^';
    if (d == Direction.East)  return '>';
    if (d == Direction.South) return 'v';
    if (d == Direction.West)  return '<';
    return '?';
}
