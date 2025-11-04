using System;

namespace Labyrinth.Crawl
{
    public class CrawlingEventArgs : EventArgs
    {
        public CrawlingEventArgs(int x, int y, Direction direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public int X { get; }

        public int Y { get; }

        public Direction Direction { get; }
    }
}
