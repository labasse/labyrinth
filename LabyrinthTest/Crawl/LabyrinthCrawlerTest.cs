using Labyrinth.Crawl;
using Labyrinth.Tiles;
using Labyrinth.Items;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Integration test for the crawler implementation in the labyrinth")]
public class LabyrinthCrawlerTest
{
    #region Initialization
    [Test]
    public void InitWithCenteredX()
    {
    // Verify initial position/direction and facing tile when 'x' is centered.
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
    // Ensure the crawler starts at the last 'x' occurrence in the map.
        var laby = new Labyrinth.Labyrinth("""
                +---+
                |x x|
                +---+
                """);
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(3));
        Assert.That(test.Y, Is.EqualTo(1));
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        // Creating a labyrinth without 'x' should throw an ArgumentException.
        Assert.Throws<ArgumentException>(() =>
        {
            var laby = new Labyrinth.Labyrinth("""
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
    // Place 'x' on the top row facing North; facing tile should be Outside.
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |x |
                +--+
                """);
        var test = laby.NewCrawler();
        
        var laby2 = new Labyrinth.Labyrinth("""
                +---+
                | x |
                |   |
                +---+
                """);
        var test2 = laby2.NewCrawler();

        var laby3 = new Labyrinth.Labyrinth("""
                +x--+
                |   |
                +---+
                """);
        var test3 = laby3.NewCrawler();

        Assert.That(test3.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
    // Place 'x' at far left, turn West, and expect Outside.
        var laby = new Labyrinth.Labyrinth("""
                x---+
                |   |
                +---+
                """);
        var test = laby.NewCrawler();

        test.Direction.TurnLeft();

        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
    // Place 'x' at far right, turn East, and expect Outside.
        var laby = new Labyrinth.Labyrinth("""
                +---x
                |   |
                +---+
                """);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();

        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
    // Place 'x' at bottom row, turn South, and expect Outside.
        var laby = new Labyrinth.Labyrinth("""
                +---+
                |   |
                +x--+
                """);
        var test = laby.NewCrawler();

        test.Direction.TurnRight();
        test.Direction.TurnRight();

        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion

    #region Moves
    [Test]
    public void TurnLeftFacesWestTile()
    {
    // From center, turning left should change facing from a Wall (North) to a Room (West).
        var laby = new Labyrinth.Labyrinth("""
                +---+
                | x |
                +---+
                """);
        var test = laby.NewCrawler();
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());

        test.Direction.TurnLeft();

        Assert.That(test.FacingTile, Is.TypeOf<Room>());
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
    // Turn East and walk into an empty room; position and facing tile update, returns inventory.
        var laby = new Labyrinth.Labyrinth("""
                +---+
                |x  |
                +---+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnRight();

        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());

        var inventory = test.Walk();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));

        Assert.That(test.FacingTile, Is.TypeOf<Room>());

        Assert.That(inventory, Is.Not.Null);
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
    // Turn South and walk; verify position update, facing Wall, and inventory returned.
        var laby = new Labyrinth.Labyrinth("""
                +---+
                |x  |
                |   |
                +---+
                """);
        var test = laby.NewCrawler();

        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));

        test.Direction.TurnRight();
        test.Direction.TurnRight();

        Assert.That(test.Direction, Is.EqualTo(Direction.South));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());

        var inventory = test.Walk();

        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(2));

        Assert.That(test.FacingTile, Is.TypeOf<Wall>());

        Assert.That(inventory, Is.Not.Null);
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
    // Facing a Wall, walking should throw InvalidOperationException.
        var laby = new Labyrinth.Labyrinth("""
                +---+
                |x  |
                +---+
                """);
        var test = laby.NewCrawler();
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
        Assert.That(test.FacingTile.IsTraversable, Is.False);

        Assert.Throws<InvalidOperationException>(() => test.Walk());
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
    // Turn East, walk onto the key tile; inventory should contain a Key.
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |xk|
                +-/+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnRight();
        Assert.That(test.FacingTile, Is.TypeOf<Room>());

        var inventory = test.Walk();

        Assert.That(inventory, Is.Not.Null);
        Assert.That(inventory.HasItem, Is.True);
        Assert.That(inventory.ItemType, Is.EqualTo(typeof(Key)));
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
    // Attempt to open a locked door with an empty inventory should throw and keep the door locked.
        var laby = new Labyrinth.Labyrinth("""
                +--+
                |xk|
                +-/+
                """);
        var test = laby.NewCrawler();
        test.Direction.TurnRight();
        test.Walk(); // Maintenant en (2,1)

        test.Direction.TurnRight();

        Assert.That(test.FacingTile, Is.TypeOf<Door>());

        var door = (Door)test.FacingTile;
        Assert.That(door.IsLocked, Is.True);

        var emptyInventory = new MyInventory();

        Assert.Throws<InvalidOperationException>(() => door.Open(emptyInventory));

        Assert.That(door.IsLocked, Is.True);
    }

    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
    // Pick up the key, open the door to the South, walk through, and verify final state.
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
