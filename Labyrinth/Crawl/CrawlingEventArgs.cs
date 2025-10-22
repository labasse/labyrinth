using Labyrinth.Crawl;

namespace Labyrinth
{
    public class CrawlingEventArgs : EventArgs
    {
        public int X { get; }
        public int Y { get; }
        public Direction Direction { get; }

        public CrawlingEventArgs(int x, int y, Direction direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }
    }
}
