using System;

public class Wall : Tile
{
    public override bool IsTraversable => false;

    public override void Pass()
    {
        throw new Exception("Impossible de traverser un mur !");
    }
}
