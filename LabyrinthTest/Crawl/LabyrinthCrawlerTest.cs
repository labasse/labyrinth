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
        var labyrinth = new Labyrinth.Labyrinth("""
                +--+
                | x|
                +--+
                """);
        var test = labyrinth.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithMultipleXUsesLastOne()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +---+
                                                |  x|
                                                | x |
                                                |x  |
                                                +---+
                                                """);
        var test = labyrinth.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(3));
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var unused = new Labyrinth.Labyrinth("""
                                                    +--+
                                                    |  |
                                                    +--+
                                                    """);
        });
    }
    #endregion

    #region Labyrinth borders
    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                           +-x+
                                           |  |
                                           +--+
                                           """);
        var test = labyrinth.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(0));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                x  |
                                                +--+
                                                """);
        var test = labyrinth.NewCrawler();
        test.Direction.TurnLeft();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(0));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.West));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                |  x
                                                +--+
                                                """);
        var test = labyrinth.NewCrawler();
        test.Direction.TurnRight();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(3));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.East));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                |  |
                                                +x-+
                                                """);
        var test = labyrinth.NewCrawler();
        test.Direction.TurnRight();
        test.Direction.TurnRight();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.South));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion

    #region Moves
    [Test]
    public void TurnLeftFacesWestTile()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                | x|
                                                +--+
                                                """);
        var test = labyrinth.NewCrawler();
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
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                |  |
                                                | x|
                                                +--+
                                                """);
        var test = labyrinth.NewCrawler();
        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory, Is.Not.Null);
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                |  |
                                                | x|
                                                +--+
                                                """);
        var test = labyrinth.NewCrawler();
        test.Direction.TurnLeft();
        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory, Is.Not.Null);
        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.West));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                | x|
                                                |  |
                                                +--+
                                                """);
        var test = labyrinth.NewCrawler();
        Assert.Throws<InvalidOperationException>(() => test.Walk());
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                |k |
                                                |/x|
                                                +--+
                                                """);
        var test = labyrinth.NewCrawler();
        var inventory = test.Walk();
        test.Direction.TurnLeft();
        var inventoryWithKey = test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(inventory, Is.Not.Null);
        Assert.That(inventoryWithKey, Is.Not.Null);
        Assert.That(inventory.HasItem, Is.False);
        Assert.That(inventoryWithKey.HasItem, Is.True);
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +----+
                                                |/   |
                                                |k/k |
                                                |x   |
                                                +----+
                                                """);
        var test = labyrinth.NewCrawler();
        var inventory = test.Walk();
        test.Direction.TurnRight();
        var open = ((Door)test.FacingTile).Open(inventory);

        using var all = Assert.EnterMultipleScope();

        Assert.That(open, Is.False);
    }

    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
        var labyrinth = new Labyrinth.Labyrinth("""
                                                +--+
                                                |xk|
                                                +-/|
                                                """);
        var test = labyrinth.NewCrawler();

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
