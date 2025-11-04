using Labyrinth;
using Labyrinth.Crawl;
using Labyrinth.Random;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

public class ExplorerTest : AbstractLabyrinthTest
{
    
    private class FakeRandomDirectionGenerator : IDirectionGenerator
    {
        private readonly List<Direction> _directions;
        private int _nextDirectionIndex = 0;

        public FakeRandomDirectionGenerator(IEnumerable<Direction> directions)
        {
            _directions = new List<Direction>(directions);
        }

        public Direction NextDirection()
        {
            return _directions[_nextDirectionIndex++];
        }
    }

    [Test]
    public void ExplorerFollowInstructions()
    {
        var test = NewCrawlerFor("""
                                 +---+
                                 |    
                                 |   |
                                 |↑  |
                                 +---+
                                 """);

        var fakeDirections = new List<Direction>
        {
            Direction.North, Direction.North, Direction.East, Direction.East, Direction.East
        };
        var fakeRandomDirectionGenerator = new FakeRandomDirectionGenerator(fakeDirections);
        var explorer = new RandomExplorer(test, fakeRandomDirectionGenerator);
        explorer.Explore();
        AssertThat(test, 4, 1, Direction.East, typeof(Outside));
    }
}