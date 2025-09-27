using System;

public abstract class Tile
{
    public abstract bool IsTraversable { get; }

    public virtual void Pass()
    {
        if (!IsTraversable)
        {
            throw new Exception("Impossible de traverser cette case !");
        }
    }
}
