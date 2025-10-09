using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Integration test for the crawler implementation in the labyrinth")]
public class LabyrinthCrawlerTest
{
    static void AssertState(ICrawler crawler, int x, int y, Direction direction, Type facingTileType)
    {
        Assert.That(crawler.X, Is.EqualTo(x));
        Assert.That(crawler.Y, Is.EqualTo(y));
        Assert.That(crawler.Direction, Is.EqualTo(direction));
        Assert.That(crawler.FacingTile, Is.TypeOf(facingTileType));
    }

    static void AssertInventory(Inventory inventory, bool hasItem = false, Type? itemType = null)
    {
        Assert.That(inventory, Is.Not.Null);
        if (!hasItem)
        {
            Assert.That(inventory.HasItem, Is.False);
            Assert.That(() => inventory.ItemType, Throws.TypeOf<InvalidOperationException>());
        }
        else Assert.That(inventory.HasItem, Is.True);
        Assert.That(inventory, Is.AssignableTo<Inventory>());
        if (itemType != null) Assert.That(inventory.ItemType, Is.EqualTo(itemType));
    }

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
        AssertState(crawler, 2, 1, Direction.North, typeof(Wall));
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
        AssertState(crawler, 3, 1, Direction.North, typeof(Wall));
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |  |
                +--+
                """);

        Assert.That(() => laby.NewCrawler(), Throws.TypeOf<ArgumentException>());
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
        AssertState(crawler, 2, 0, Direction.North, typeof(Outside));
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
        AssertState(crawler, 0, 1, Direction.West, typeof(Outside));
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
        AssertState(crawler, 3, 1, Direction.East, typeof(Outside));
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
        AssertState(crawler, 2, 2, Direction.South, typeof(Outside));
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
        AssertState(crawler, 1, 1, Direction.West, typeof(Wall));
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

        AssertInventory(inventory, false);
        AssertState(crawler, 1, 1, Direction.North, typeof(Wall));
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

        AssertInventory(inventory, false);
        AssertState(crawler, 2, 1, Direction.East, typeof(Wall));
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
        Assert.That(() => crawler.Walk(), Throws.InvalidOperationException);
    }

    [Test]
    public void WalkOutOfTheLabyrinthThrowsInvalidOperationException()
    {
        var laby = new Labyrinth.Labyrinth("""
                +-x+
                |  |
                +--+
                """);
        var crawler = laby.NewCrawler();

        Assert.That(() => crawler.Walk(), Throws.InvalidOperationException);
    }
    [Test]
    public void FailedWalkDoesNotMove()
    {
        var laby = new Labyrinth.Labyrinth("""
        +--+
        |x |
        +--+
        """);
        var crawler = laby.NewCrawler();

        int x = crawler.X;
        int y = crawler.Y;
        Direction dir = crawler.Direction;
        Assert.That(() => crawler.Walk(), Throws.InvalidOperationException);
        Assert.That((crawler.X, crawler.Y, crawler.Direction), Is.EqualTo((x, y, dir)));
    }
    [Test]
    public void FacingOutsideIsSingleton()
    {
        var laby = new Labyrinth.Labyrinth("""
        +-x+
        |  |
        +--+
        """);
        var crawler = laby.NewCrawler();

        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |k |
                |x |
                +-/|
                """);
        var crawler = laby.NewCrawler();
        var inventory = crawler.Walk();

        using var all = Assert.EnterMultipleScope();

        AssertInventory(inventory, true);
        AssertState(crawler, 1, 1, Direction.North, typeof(Wall));
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var laby = new Labyrinth.Labyrinth("""
                +---+
                |xk/|
                | /k|
                +---+
                """);
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnRight();
        var wrongKeyInventory = crawler.Walk();
        crawler.Direction.TurnRight();
        var door = crawler.FacingTile as Door ?? throw new InvalidOperationException("Expected a door");
        var opened = door.Open(wrongKeyInventory);

        using var all = Assert.EnterMultipleScope();

        Assert.That(opened, Is.False);
        Assert.That(door.IsLocked, Is.True);
        AssertInventory(wrongKeyInventory, true, typeof(Key));
        AssertState(crawler, 2, 1, Direction.South, typeof(Door));
        Assert.That(() => crawler.Walk(), Throws.InvalidOperationException);
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
        var door = crawler.FacingTile as Door ?? throw new InvalidOperationException("Expected a door");
        var opened = door.Open(inventory);
        crawler.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(opened, Is.True);
        Assert.That(door.IsLocked, Is.False);
        AssertInventory(inventory, false);
        AssertState(crawler, 2, 2, Direction.South, typeof(Outside));
    }
    #endregion
}
