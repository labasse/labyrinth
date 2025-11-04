using Labyrinth;
using Labyrinth.Crawl;
using Labyrinth.Events;

const string asciiMap = """
    +--+--------+
    |  /        |
    |  +--+--+  |
    |     |k    |
    +--+  | /+--+
       |k  x    |
    +  +-----+  |
    |           |
    +-----------+
    """;
    
    
var labyrinth = new Labyrinth.Labyrinth(asciiMap);
var crawler = labyrinth.NewCrawler();
var explorator = new CrawlerAI(crawler);

var previousPosition = (X: crawler.X, Y: crawler.Y);
const int maxTries = 10000;

Console.CursorVisible = false;
Console.Clear();
Console.WriteLine(labyrinth.ToString());

static char GetDirectionChar(Direction direction) =>
    direction == Direction.North ? '^' :
    direction == Direction.East ? '>' :
    direction == Direction.South ? 'v' : '<';

void DrawPosition(CrawlingEventArgs args)
{
    Console.SetCursorPosition(previousPosition.X, previousPosition.Y);
    Console.Write(" ");
    
    Console.SetCursorPosition(args.X, args.Y);
    Console.Write(GetDirectionChar(args.Direction));
    
    previousPosition = (args.X, args.Y);
}

DrawPosition(new CrawlingEventArgs(crawler.X, crawler.Y, crawler.Direction));

explorator.PositionChanged += (_, args) => DrawPosition(args);
explorator.DirectionChanged += (_, args) => DrawPosition(args);

var foundExit = explorator.GetOut(maxTries);

Console.SetCursorPosition(0, labyrinth.ToString().Split('\n').Length + 1);
Console.WriteLine(foundExit ? "Exit found" : "Still stuck in the labyrinth after " + maxTries + " actions.");