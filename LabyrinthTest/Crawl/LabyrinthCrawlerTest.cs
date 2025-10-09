using Labyrinth.Crawl;
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
                |x |
                | x|
                +--+
                """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |  |
                +--+
                """);
        Assert.That(() => laby.NewCrawler(), Throws.ArgumentException);
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
        var test = laby.NewCrawler();
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                x  |
                +--+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnLeft();
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |  x
                +--+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnRight();
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |  |
                +x-+
                """);
        var test = laby.NewCrawler();
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
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
        var test = laby.NewCrawler();
        test.Direction.TurnLeft();
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                | x|
                +--+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnLeft(); // face West into a room
        var inv = test.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(inv, Is.Not.Null);
        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |x |
                +--+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnRight(); // face East into a room
        var inv = test.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(inv, Is.Not.Null);
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                | x|
                +--+
                """);
        var test = laby.NewCrawler();
        Assert.That(() => test.Walk(), Throws.InvalidOperationException);
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |xk|
                +--+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnRight();
        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(inventory.HasItem, Is.True);
        Assert.That(inventory.ItemType, Is.EqualTo(typeof(Labyrinth.Items.Key)));
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var laby = new Labyrinth.Labyrinth("""
                +k-+
                |x/|
                +/k+
                """);
        var test = laby.NewCrawler();

        var inv = test.Walk(); 

        test.Direction.TurnRight(); 
        test.Direction.TurnRight();
        test.Walk();
        
        var door = (Door)test.FacingTile;
        var opened = door.Open(inv);

        using var all = Assert.EnterMultipleScope();
        Assert.That(opened, Is.False);
        Assert.That(door.IsLocked, Is.True);
        Assert.That(inv.HasItem, Is.True); // key returned to inventory
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
