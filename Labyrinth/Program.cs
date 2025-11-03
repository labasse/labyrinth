using System;
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

var lab = new Labyrinth.Labyrinth(ascii);
var mapLines = lab.ToString().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

// print the static labyrinth
for (int i = 0; i < mapLines.Length; i++)
{
   Console.Write(mapLines[i]);
   if (i < mapLines.Length - 1) Console.WriteLine();
}

var crawler = lab.NewCrawler();
var explorer = new Explorer(crawler);

// keep original chars to restore when the crawler moves
var grid = new char[mapLines.Length][];
for (int y = 0; y < mapLines.Length; y++) grid[y] = mapLines[y].ToCharArray();

int prevX = crawler.X, prevY = crawler.Y;

char DirSymbol(Direction d) => d == Direction.North ? '^' : d == Direction.East ? '>' : d == Direction.South ? 'v' : '<';

void DrawAt(int x, int y, char c)
{
   try
   {
      Console.SetCursorPosition(x, y);
      Console.Write(c);
   }
   catch (ArgumentOutOfRangeException)
   {
      // ignore if console is too small
   }
}

// draw initial crawler
DrawAt(crawler.X, crawler.Y, DirSymbol(crawler.Direction));

explorer.DirectionChanged += (_, e) =>
{
   // update orientation at current position
   DrawAt(e.X, e.Y, DirSymbol(e.Direction));
   System.Threading.Thread.Sleep(30);
};

explorer.PositionChanged += (_, e) =>
{
   // restore previous tile
   if (prevY >= 0 && prevY < grid.Length && prevX >= 0 && prevX < grid[prevY].Length)
   {
      DrawAt(prevX, prevY, grid[prevY][prevX]);
   }
   // draw crawler at new position
   DrawAt(e.X, e.Y, DirSymbol(e.Direction));
   prevX = e.X; prevY = e.Y;
   System.Threading.Thread.Sleep(30);
};

// run the explorer
explorer.GetOut(1000);

// move cursor after animation (clamp to console buffer to avoid ArgumentOutOfRange)
int finalTop = Math.Min(mapLines.Length + 1, Console.BufferHeight - 1);
if (finalTop < 0) finalTop = 0;
Console.SetCursorPosition(0, finalTop);
