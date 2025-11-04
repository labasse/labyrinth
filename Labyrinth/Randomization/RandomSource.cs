namespace Labyrinth.Randomization;

public sealed class RandomSource : IRandomSource {
    public int Next(int minInclusive, int maxExclusive) => Random.Shared.Next(minInclusive, maxExclusive);
}