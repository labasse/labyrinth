using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Integration test for the crawler implementation in the labyrinth")]
public class LabyrinthCrawlerTest
{
    #region Initialization
    [Test]
    public void InitWithCenteredX()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                | x|
                +--+
                """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithMultipleXUsesLastOne()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |xx|
                                           +--+
                                           """);
        var crawler = laby.NewCrawler();

        Assert.That(crawler.X, Is.EqualTo(2));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.North));
        Assert.That(crawler.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var laby = new Labyrinth.Labyrinth("""
                                               +--+
                                               |  |
                                               +--+
                                               """);
            laby.NewCrawler();
        });
    }
    #endregion

    #region Labyrinth borders
    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +x-+
                                           |  |
                                           +--+
                                           """);
        var crawler = laby.NewCrawler();

        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           x  |
                                           +--+
                                           """);
        var crawler = laby.NewCrawler();
        crawler.Direction.TurnLeft();

        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |  x
                                           +--+
                                           """);
        var crawler = laby.NewCrawler();
        crawler.Direction.TurnRight();

        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |  |
                                           +x-+
                                           """);
        var crawler = laby.NewCrawler();
        crawler.Direction.TurnLeft();
        crawler.Direction.TurnLeft();

        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion

    #region Moves
    [Test]
    public void TurnLeftFacesWestTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           | x|
                                           +--+
                                           """);
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnLeft();

        Assert.That(crawler.Direction, Is.EqualTo(Direction.West));
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |  |
                                           | x|
                                           +--+
                                           """);
        var test = laby.NewCrawler();
        var inventory = test.Walk();

        Assert.That(inventory.HasItem, Is.False);
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |  |
                                           | x|
                                           +--+
                                           """);
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnLeft();
        var inventory = crawler.Walk();

        Assert.That(inventory.HasItem, Is.False);

        Assert.That(crawler.X, Is.EqualTo(1));
        Assert.That(crawler.Y, Is.EqualTo(2));

        Assert.That(crawler.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           | x|
                                           +--+
                                           """);
        var crawler = laby.NewCrawler();

        Assert.Throws<InvalidOperationException>(() => crawler.Walk());
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |xk|
                                           |-/|
                                           |  |
                                           +--+
                                           """);
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnRight();

        var inventory = crawler.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory.HasItem, Is.True);
        Assert.That(inventory.ItemType, Is.EqualTo(typeof(Key)));
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +---- +
                                           |xk /k|
                                           +-/---|
                                           """);
        
        var test = laby.NewCrawler();
        
        test.Direction.TurnRight();
        
        var inventory = test.Walk();
        
        test.Direction.TurnRight();
        ((Door)test.FacingTile).Open(inventory);
        
        Assert.Throws<InvalidOperationException>(() => test.Walk());
    }

    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |xk|
                +-/|
                """);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();

        var inventory = test.Walk();

        test.Direction.TurnRight();
        ((Door)test.FacingTile).Open(inventory);

        test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.South));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion
}
