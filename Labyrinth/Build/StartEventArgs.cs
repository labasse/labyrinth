namespace Labyrinth.Build;

public class StartEventArgs : EventArgs
{
    public StartEventArgs(int x, int y) => (X, Y) = (x, y);
    public int X { get; }
    public int Y { get; }
}
