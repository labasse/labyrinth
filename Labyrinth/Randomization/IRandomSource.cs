namespace Labyrinth.Randomization;

public interface IRandomSource {
    int Next(int minInclusive, int maxExclusive);
}