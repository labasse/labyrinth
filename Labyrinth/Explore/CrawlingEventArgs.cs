using Labyrinth.Crawl;

namespace Labyrinth.Explore;

public class CrawlingEventArgs : EventArgs
{
    public CrawlingEventArgs(int x, int y, Direction direction) => (X, Y, Direction) = (x, y, direction);
    public int X { get; }
    public int Y { get; }
    public Direction Direction { get; }
}
