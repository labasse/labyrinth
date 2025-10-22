using Labyrinth;
using Labyrinth.Crawl;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Direction unit test class")]
public class RandomExplorerTest
{
    [Test]
    public void ExplorerFiresPositionChangedEventsWhenMoving()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
            +--+--------+
            |  /        |
            |  +--+--+  |
            |     |k    |
            +--+  |  +--+
               |k  x    |
            +  +-------/|
            |           |
            +-----------+
            """);

        var explorer = new RandomExplorer(labyrinth.NewCrawler());

        var positions = new List<CrawlingEventArgs>();
        explorer.PositionChanged += (s, e) => positions.Add(e);

        explorer.GetOut(5);

        Assert.That(positions, Is.Not.Empty);
    }

    [Test]
    public void ExplorerFiresDirectionChangedEventsWhenTurning()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
            +--+--------+
            |  /        |
            |  +--+--+  |
            |     |k    |
            +--+  |  +--+
               |k  x    |
            +  +-------/|
            |           |
            +-----------+
            """);

        var explorer = new RandomExplorer(labyrinth.NewCrawler());

        var directions = new List<Direction>();
        explorer.DirectionChanged += (s, e) => directions.Add(e.Direction);

        explorer.GetOut(5);

        Assert.That(directions, Is.Not.Empty);
    }
}
