namespace Labyrinth;

public record Key() : ICollectable
{
    public static Key New() => new Key();
}