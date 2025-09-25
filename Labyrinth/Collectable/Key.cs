namespace Labyrinth.Collectable;

public class Key(Guid guid) : ICollectable
{
    public Guid Guid { get; set; } = guid;
}