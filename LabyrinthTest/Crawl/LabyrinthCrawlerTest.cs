using Labyrinth.Crawl;
using Labyrinth.Models;
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
                                           | ↑|
                                           +--+
                                           """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.Coord.X, Is.EqualTo(2));
        Assert.That(test.Coord.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithMultipleXUsesLastOne()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |→↑|
                                           +--+
                                           """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.Coord.X, Is.EqualTo(2));
        Assert.That(test.Coord.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => new Labyrinth.Labyrinth("""
            +--+
            |  |
            +--+
            """));
        Assert.That(ex.Message, Does.Contain("Labyrinth must contains one crawler"));
    }

    #endregion

    #region Labyrinth borders

    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +↑-+
                                           |  |
                                           +--+
                                           """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.FacingTile, Is.EqualTo(Outside.Singleton));
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           ←  |
                                           +--+
                                           """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();
        Console.WriteLine(laby.ToString());

        Assert.That(test.FacingTile, Is.EqualTo(Outside.Singleton));
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |  →
                                           +--+
                                           """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.FacingTile, Is.EqualTo(Outside.Singleton));
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |  |
                                           +-↓+
                                           """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.FacingTile, Is.EqualTo(Outside.Singleton));
    }

    #endregion

    #region Moves

    [Test]
    public void TurnLeftFacesWestTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +---+
                                           |   |
                                           |+↑ |
                                           |   |
                                           +---+
                                           """);
        var test = laby.NewCrawler();
        test.Direction.TurnLeft();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.FacingTile, Is.EqualTo(Wall.Singleton));
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +---+
                                           |   |
                                           | ↑ |
                                           |   |
                                           +---+
                                           """);
        var test = laby.NewCrawler();
        var inv = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.FacingTile, Is.EqualTo(Wall.Singleton));
        Assert.That(test.Coord, Is.EqualTo(new Coord(2,1)));
        Assert.That(inv.HasItem, Is.False);
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +---+
                                           |   |
                                           | ↑ |
                                           |   |
                                           +---+
                                           """);
        var test = laby.NewCrawler();
        test.Direction.TurnLeft();
        var inv = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.FacingTile, Is.EqualTo(Wall.Singleton));
        Assert.That(test.Coord, Is.EqualTo(new Coord(1,2)));
        Assert.That(inv.HasItem, Is.False);
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +---+
                                           | + |
                                           | ↑ |
                                           |   |
                                           +---+
                                           """);
        var test = laby.NewCrawler();
        var ex = Assert.Throws<InvalidOperationException>(() => test.Walk());
        Assert.That(ex.Message, Does.Contain("Cannot pass through a non-traversable tile"));
    }
    
    [Test]
    public void WalkAndTurnMultipleTimesUpdatesPositionCorrectly()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +---+
                                           |   |
                                           |   |
                                           |↑  |
                                           +---+
                                           """);
        var test = laby.NewCrawler();
        test.Direction.TurnRight();
        test.Walk();
        test.Direction.TurnLeft();
        test.Walk();
        test.Walk();

        Assert.That(test.Coord, Is.EqualTo(new Coord(2,1)));
        Assert.That(test.FacingTile, Is.EqualTo(Wall.Singleton));
    }

    #endregion

    #region Items and doors

    [Test]
    public void WalkInARoomWithAnItem()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +---+
                                           | k |
                                           | ↑ |
                                           | / |
                                           +---+
                                           """);
        var test = laby.NewCrawler();
        var inv = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inv.HasItem, Is.True);
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +---+
                                           | k/|
                                           | ↑ |
                                           |k/ |
                                           +---+
                                           """);
        var test = laby.NewCrawler();
        var inv = test.Walk();
        test.Direction.TurnRight();
        test.Direction.TurnRight();
        test.Walk();
        
        bool is_oppened = ((Door)test.FacingTile).Open(inv);
        
        Assert.That(is_oppened, Is.False);
    }
    
    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
        var laby = new Labyrinth.Labyrinth("""
                                           +--+
                                           |↑k|
                                           +-/|
                                           """);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();

        var inventory = test.Walk();

        test.Direction.TurnRight();
        ((Door)test.FacingTile).Open(inventory);

        test.Walk();

        using var all = Assert.EnterMultipleScope();
        
        Assert.That(test.Coord, Is.EqualTo(new Coord(2,2)));
        Assert.That(test.Direction, Is.EqualTo(Direction.South));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    #endregion
}