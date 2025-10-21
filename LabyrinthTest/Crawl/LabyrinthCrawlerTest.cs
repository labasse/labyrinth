using Labyrinth.Items;
using Labyrinth.Crawl;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Integration test for the crawler implementation in the labyrinth")]
public class LabyrinthCrawlerTest
{
    #region Initialization
    /// <summary>
    /// Verifies the crawler starts at the expected coordinates and orientation when the map has a single centered start.
    /// </summary>
    [Test]
    public void InitWithCenteredX()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "+--+",
            "| x|",
            "+--+"));
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    /// <summary>
    /// Ensures that when multiple start markers exist, the crawler uses the last one.
    /// </summary>
    [Test]
    public void InitWithMultipleXUsesLastOne()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "+--+",
            "|xx|",
            "+--+"));
        var crawler = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(2));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.North));
    }

    /// <summary>
    /// Confirms an exception is raised if the map contains no starting position.
    /// </summary>
    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        Assert.That(
            () => new Labyrinth.Labyrinth(Map(
                "+--+",
                "|  |",
                "+--+")),
            Throws.TypeOf<ArgumentException>());
    }
    #endregion

    #region Labyrinth borders
    /// <summary>
    /// Checks that stepping north from the top edge exposes the outside tile.
    /// </summary>
    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "x++",
            "+++",
            "+++"));
        var crawler = laby.NewCrawler();

        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }

    /// <summary>
    /// Checks that facing west on the leftmost column yields the outside tile.
    /// </summary>
    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "+++",
            "x++",
            "+++"));
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnLeft();

        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }

    /// <summary>
    /// Checks that facing east on the rightmost column yields the outside tile.
    /// </summary>
    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "+++",
            "++x",
            "+++"));
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnRight();

        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }

    /// <summary>
    /// Checks that facing south on the bottom row yields the outside tile.
    /// </summary>
    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "+++",
            "+++",
            "++x"));
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnRight();
        crawler.Direction.TurnRight();

        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }
    #endregion

    #region Moves
    /// <summary>
    /// Confirms turning left from the initial orientation points to the expected tile.
    /// </summary>
    [Test]
    public void TurnLeftFacesWestTile()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "   ",
            " x ",
            "   "));
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnLeft();

        Assert.That(crawler.FacingTile, Is.TypeOf<Room>());
    }

    /// <summary>
    /// Validates that walking forward updates position and returns an empty inventory.
    /// </summary>
    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "   ",
            " x ",
            "   "));
        var crawler = laby.NewCrawler();

        var inventory = crawler.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(1));
        Assert.That(crawler.Y, Is.EqualTo(0));
        Assert.That(inventory.HasItem, Is.False);
        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }

    /// <summary>
    /// Ensures a turn followed by a walk moves to the correct tile and reports inventory.
    /// </summary>
    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "   ",
            " x ",
            "   "));
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnRight();
        var inventory = crawler.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(2));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(inventory.HasItem, Is.False);
        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }

    /// <summary>
    /// Verifies walking into a non-traversable tile throws an invalid operation exception.
    /// </summary>
    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "+++",
            "+x+",
            "+++"));
        var crawler = laby.NewCrawler();

        Assert.That(() => crawler.Walk(), Throws.InvalidOperationException);
    }
    #endregion

    #region Items and doors
    /// <summary>
    /// Checks that entering a room containing an item adds it to the crawler's inventory.
    /// </summary>
    [Test]
    public void WalkInARoomWithAnItem()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "/  ",
            "kx ",
            "   "));
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnLeft();
        var inventory = crawler.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler.X, Is.EqualTo(0));
        Assert.That(crawler.Y, Is.EqualTo(1));
        Assert.That(inventory.HasItem, Is.True);
        Assert.That(inventory.ItemType, Is.EqualTo(typeof(Key)));
        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }

    /// <summary>
    /// Ensures attempting to unlock a door with the wrong key keeps it closed and preserves the inventory.
    /// </summary>
    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "/k/++",
            " xk  ",
            "+++++"));
        var crawler = laby.NewCrawler();

        var wrongKeyInventory = crawler.Walk();

        crawler.Direction.TurnRight();
        var door = (Door)crawler.FacingTile;
        var opened = door.Open(wrongKeyInventory);

        using var all = Assert.EnterMultipleScope();
        Assert.That(opened, Is.False);
        Assert.That(wrongKeyInventory.HasItem, Is.True);
        Assert.That(door.IsLocked, Is.True);
    }

    /// <summary>
    /// Validates a correct key unlocks the door, the crawler passes through, and item flow is consistent.
    /// </summary>
    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
        var laby = new Labyrinth.Labyrinth(Map(
            "/x ",
            "k  ",
            "   "));
        var crawler = laby.NewCrawler();

        crawler.Direction.TurnRight();
        crawler.Direction.TurnRight();
        var firstStepInventory = crawler.Walk();

        crawler.Direction.TurnRight();
        var keyInventory = crawler.Walk();

        crawler.Direction.TurnRight();
        var door = (Door)crawler.FacingTile;
        var opened = door.Open(keyInventory);

        var doorInventory = crawler.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(firstStepInventory.HasItem, Is.False);
        Assert.That(opened, Is.True);
        Assert.That(keyInventory.HasItem, Is.False);
        Assert.That(doorInventory.HasItem, Is.True);
        Assert.That(doorInventory.ItemType, Is.EqualTo(typeof(Key)));
        Assert.That(crawler.X, Is.EqualTo(0));
        Assert.That(crawler.Y, Is.EqualTo(0));
        Assert.That(crawler.Direction, Is.EqualTo(Direction.North));
        Assert.That(crawler.FacingTile, Is.SameAs(Outside.Singleton));
    }
    #endregion

    private static string Map(params string[] rows) => string.Join('\n', rows);
}
