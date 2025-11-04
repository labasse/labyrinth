using System;

namespace Labyrinth.Build
{
    public class StartEventArgs : EventArgs
    {
        public int X { get; }
        public int Y { get; }

        public StartEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
