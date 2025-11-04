using Labyrinth.Crawl;
using Labyrinth.Models;

namespace LabyrinthTest.Crawl;

public abstract class AbstractLabyrinthTest
{
    protected static ICrawler NewCrawlerFor(string ascii_map) =>
        new Labyrinth.Labyrinth(ascii_map).NewCrawler();

    protected static void AssertThat(ICrawler test, int x, int y, Direction dir, Type facingTile)
    {
        using var all = Assert.EnterMultipleScope();

        Assert.That(test.Coord, Is.EqualTo(new Coord(x, y)));
        Assert.That(test.Direction, Is.EqualTo(dir));
        Assert.That(test.FacingTile, Is.TypeOf(facingTile));
    }
}