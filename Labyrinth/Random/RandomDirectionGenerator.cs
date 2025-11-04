using Labyrinth.Crawl;

namespace Labyrinth.Random;

public class RandomDirectionGenerator : IDirectionGenerator
{
    private static readonly System.Random Random = new();

    public Direction NextDirection()
    {
        int value = Random.Next(4);
        return value switch
        {
            0 => Direction.North,
            1 => Direction.East,
            2 => Direction.South,
            _ => Direction.West
        };
    }
}