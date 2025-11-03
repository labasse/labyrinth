using System;

namespace Labyrinth.Crawl
{
    /// <summary>
    /// Event args fired when the crawler's position or direction changes.
    /// </summary>
    public sealed class CrawlingEventArgs : EventArgs
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
