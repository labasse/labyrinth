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
        var crawler = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(crawler.X, Is.EqualTo(2));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.North));
        Assert.That(crawler.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithMultipleXUsesLastOne()
    {
        var laby = new Labyrinth.Labyrinth("""
                +---+
                |x x|
                +---+
                """);
        var crawler = laby.NewCrawler();
        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(3));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.North));
        Assert.That(crawler.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |  |
                +--+
                """);
        Assert.Throws<ArgumentException>(() => laby.NewCrawler());
    }
    #endregion

    #region Labyrinth borders
    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                +-x+
                |  |
                +--+
                """);
        var crawler = laby.NewCrawler();
        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(2));
        Assert.That(crawler.Y, Is.EqualTo(0));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.North));
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
        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(0));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.West));
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
        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(3));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.East));
        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |  |
                +-x+
                """);
        var crawler = laby.NewCrawler();
        crawler.Direction.TurnRight();
        crawler.Direction.TurnRight();
        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(2));
        Assert.That(crawler.Y, Is.EqualTo(2));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.South));
        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion

    #region Moves
    [Test]
    public void TurnLeftFacesWestTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |x |
                +--+
                """);
        var crawler = laby.NewCrawler();
        crawler.Direction.TurnLeft();
        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(1));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.West));
        Assert.That(crawler.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |  |
                |x |
                +--+
                """);
        var crawler = laby.NewCrawler();
        var inventory = crawler.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory, Is.Not.Null);
        Assert.That(inventory, Is.AssignableTo<Inventory>());
        Assert.That(crawler.X, Is.EqualTo(1));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.North));
        Assert.That(crawler.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |x |
                +--+
                """);
        var crawler = laby.NewCrawler();
        crawler.Direction.TurnRight();
        var inventory = crawler.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory, Is.Not.Null);
        Assert.That(inventory, Is.AssignableTo<Inventory>());
        Assert.That(crawler.X, Is.EqualTo(2));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.East));
        Assert.That(crawler.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |x |
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
        Assert.That(false);
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        Assert.That(false);
    }

    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |xk|
                +-/|
                """);
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnRight();

        var inventory = crawler.Walk();

        crawler.Direction.TurnRight();
        ((Door)crawler.FacingTile).Open(inventory);

        crawler.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(crawler.X, Is.EqualTo(2));
        Assert.That(crawler.Y, Is.EqualTo(2));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.South));
        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion
}
