namespace Labyrinth.Tile;

public abstract class Tile
{
    // Propriété abstraite en lecture seule
    public abstract bool IsTraversable { get; }

    // Méthode abstraite
    public abstract void Pass();
}