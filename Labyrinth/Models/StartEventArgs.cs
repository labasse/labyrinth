using Labyrinth.Crawl;

namespace Labyrinth.Models;
public class StartEventArgs : EventArgs
{
    public Coord Coord { get; }
    public Direction Direction { get; }

    public StartEventArgs(Coord coord, Direction direction)
    {
        Coord = coord;
        Direction = direction;
    }
}