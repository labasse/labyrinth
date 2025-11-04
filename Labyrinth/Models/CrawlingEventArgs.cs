using Labyrinth.Crawl;

namespace Labyrinth.Models;

public class CrawlingEventArgs : EventArgs
{
    public Coord Coord { get; }
    public Direction Direction { get; }

    public CrawlingEventArgs(Coord coord, Direction direction)
    {
        Coord = coord;
        Direction = direction;
    }
}