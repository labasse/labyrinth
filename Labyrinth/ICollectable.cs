namespace Labyrinth;

public interface ICollectable
{
}

public class Key : ICollectable
{
    public override string ToString()
    {
        return "k";
    }
}
