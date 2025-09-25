using System;

public class Wall : Tile
{
    public override bool IsTraversable { get { return false; } }
    public override String Character { get { return "#"; } }
    public override void Pass()
    {
        throw new InvalidOperationException("Cannot pass through a wall.");
    }
}
