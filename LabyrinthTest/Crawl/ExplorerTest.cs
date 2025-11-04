using Labyrinth;
using Labyrinth.Crawl;
using Labyrinth.Models;
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

        var capturedEvents = new List<CrawlingEventArgs>();

        explorer.CrawlerMoved += (sender, args) => capturedEvents.Add(args);

        explorer.Explore();

        AssertThat(test, 4, 1, Direction.East, typeof(Outside));

        var expectedEvents = new List<CrawlingEventArgs>
        {
            new CrawlingEventArgs(new Coord(1, 2), Direction.North),
            new CrawlingEventArgs(new Coord(1, 1), Direction.North),
            new CrawlingEventArgs(new Coord(1, 1), Direction.East),
            new CrawlingEventArgs(new Coord(2, 1), Direction.East),
            new CrawlingEventArgs(new Coord(3, 1), Direction.East),
            new CrawlingEventArgs(new Coord(4, 1), Direction.East)
        };

        Assert.That(capturedEvents.Count, Is.EqualTo(expectedEvents.Count));
        for (int i = 0; i < expectedEvents.Count; i++)
        {
            Assert.That(capturedEvents[i].Coord, Is.EqualTo(expectedEvents[i].Coord));
            Assert.That(capturedEvents[i].Direction, Is.EqualTo(expectedEvents[i].Direction));
        }
    }
}
