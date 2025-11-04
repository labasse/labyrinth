using System;

namespace Labyrinth.Build
{
    public class StartEventArgs : EventArgs
    {
        public StartEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }
}
