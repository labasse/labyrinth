using System;

namespace Labyrinth.Build
{
    /// <summary>
    /// Event args raised when a start position is found while parsing an ASCII map.
    /// </summary>
    public sealed class StartEventArgs : EventArgs
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
