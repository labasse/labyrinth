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
        var test = laby.NewCrawler();
        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
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
                +x-+
                |  |
                +--+
                """);
        var test = laby.NewCrawler();
        
        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(0));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
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
        
        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(0));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());

        test.Direction.TurnLeft();

        Assert.That(test.Direction, Is.EqualTo(Direction.West));
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
        
        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(3));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());

        test.Direction.TurnRight();

        Assert.That(test.Direction, Is.EqualTo(Direction.East));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |  |
                +-x+
                """);
        var test = laby.NewCrawler();
        
        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());

        test.Direction.TurnRight();
        test.Direction.TurnRight();

        Assert.That(test.Direction, Is.EqualTo(Direction.South));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
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
        var test = laby.NewCrawler();

        test.Direction.TurnLeft();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.West));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());
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

        test.Direction.TurnLeft();
        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory.HasItem, Is.False);
        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.West));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                +---+
                |  x|
                +---+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnLeft();
        
        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(3));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.West));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());
        
        var inventory = test.Walk();
        Assert.That(inventory.HasItem, Is.False);
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.West));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());
        
        inventory = test.Walk();
        Assert.That(inventory.HasItem, Is.False);
        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.West));
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
        Assert.Throws<InvalidOperationException>(() => test.Walk());
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
        var test = laby.NewCrawler();

        test.Direction.TurnRight();
        var inventory = test.Walk();
        
        using var all = Assert.EnterMultipleScope();
        Assert.That(inventory.HasItem, Is.True);
        Assert.That(inventory.ItemType, Is.EqualTo(typeof(Key)));
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +----- +
                                           |xk  /k|
                                           +-/----|
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
