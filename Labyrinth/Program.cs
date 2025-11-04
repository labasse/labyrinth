using Labyrinth.Crawl;

var ascii = """
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

var labyrinth = new Labyrinth.Labyrinth(ascii);
var view = labyrinth.ToString();
if (Console.IsOutputRedirected)
{
    Console.Write(view);
    return;
}

var lines = view.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

Console.Clear();
Console.Write(view);

var crawler = labyrinth.NewCrawler();
var explorer = new Explorer(crawler);

var lastX = crawler.X;
var lastY = crawler.Y;

void Draw(CrawlingEventArgs args)
{
    if (!InBounds(args.X, args.Y))
    {
        return;
    }

    Console.SetCursorPosition(args.X, args.Y);
    Console.Write(DirectionSymbol(args.Direction));
}

char DirectionSymbol(Direction direction) => direction switch
{
    { DeltaX: 0, DeltaY: -1 } => '^',
    { DeltaX: 1, DeltaY: 0 } => '>',
    { DeltaX: 0, DeltaY: 1 } => 'v',
    { DeltaX: -1, DeltaY: 0 } => '<',
    _ => '?'
};

void Update(CrawlingEventArgs args)
{
    Restore(lastX, lastY);
    lastX = args.X;
    lastY = args.Y;
    Draw(args);
}

explorer.PositionChanged += (_, args) => Update(args);
explorer.DirectionChanged += (_, args) => Draw(args);

Draw(new CrawlingEventArgs(crawler.X, crawler.Y, crawler.Direction));
explorer.GetOut(2000);
var bottom = Math.Clamp(lines.Length + 1, 0, Console.BufferHeight > 0 ? Console.BufferHeight - 1 : 0);
Console.SetCursorPosition(0, bottom);

bool InBounds(int x, int y) =>
    x >= 0 &&
    y >= 0 &&
    y < lines.Length &&
    x < lines[y].Length &&
    x < Console.BufferWidth &&
    y < Console.BufferHeight;

void Restore(int x, int y)
{
    if (!InBounds(x, y))
    {
        return;
    }

    Console.SetCursorPosition(x, y);
    Console.Write(lines[y][x]);
}
