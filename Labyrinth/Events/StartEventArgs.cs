namespace Labyrinth.Events;

public class StartEventArgs : EventArgs
{
    // Propriétés en lecture seule
    public int X { get; }
    public int Y { get; }

    // Constructeur pour initialiser les propriétés
    public StartEventArgs(int x, int y)
    {
        X = x;
        Y = y;
    }
}