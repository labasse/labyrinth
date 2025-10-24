using Labyrinth.Build;
using Labyrinth.Events;
using Labyrinth.Tiles;

namespace LabyrinthTest.Build;

[TestFixture(Description = "AsciiParser and StartPositionFound unit tests")]
public class AsciiParserTest
{
    [Test]
    public void Parse_RaisesStartPositionFoundEvent_WhenXIsFound()
    {
        var parser = new AsciiParser();
        StartEventArgs? capturedArgs = null;
        parser.StartPositionFound += (sender, e) => capturedArgs = e;

        parser.Parse("""
            +---+
            | x |
            +---+
            """);

        Assert.That(capturedArgs, Is.Not.Null);
        using var all = Assert.EnterMultipleScope();
        Assert.That(capturedArgs!.X, Is.EqualTo(2));
        Assert.That(capturedArgs.Y, Is.EqualTo(1));
    }

    [Test]
    public void Parse_RaisesEventMultipleTimes_WhenMultipleXAreFound()
    {
        var parser = new AsciiParser();
        var capturedPositions = new List<(int X, int Y)>();
        parser.StartPositionFound += (sender, e) => 
            capturedPositions.Add((e.X, e.Y));

        parser.Parse("""
            +---+
            | x |
            |x  |
            +---+
            """);

        Assert.That(capturedPositions, Has.Count.EqualTo(2));
        Assert.That(capturedPositions[0], Is.EqualTo((2, 1)));
        Assert.That(capturedPositions[1], Is.EqualTo((1, 2)));
    }

    [Test]
    public void Parse_EventSenderIsParser()
    {
        var parser = new AsciiParser();
        object? capturedSender = null;
        parser.StartPositionFound += (sender, e) => capturedSender = sender;

        parser.Parse("""
            +---+
            | x |
            +---+
            """);

        Assert.That(capturedSender, Is.SameAs(parser));
    }

    [Test]
    public void Parse_ReturnsCorrectTiles_WhenEventHandlerIsAttached()
    {
        var parser = new AsciiParser();
        parser.StartPositionFound += (sender, e) => { /* Do nothing */ };

        // Act
        var tiles = parser.Parse("""
            +---+
            | x |
            +---+
            """);

        // Assert
        Assert.That(tiles[2, 1], Is.TypeOf<Room>());
    }

    [Test]
    public void Parse_WorksWithoutEventHandler()
    {
        var parser = new AsciiParser();

        // Act
        var tiles = parser.Parse("""
            +---+
            | x |
            +---+
            """);

        Assert.That(tiles, Is.Not.Null);
        Assert.That(tiles[2, 1], Is.TypeOf<Room>());
    }

    [Test]
    public void Parse_NoLongerHasStartParameter()
    {
        var parser = new AsciiParser();

        var method = typeof(AsciiParser).GetMethod("Parse");
        var parameters = method?.GetParameters();

        Assert.That(parameters, Has.Length.EqualTo(1));
        Assert.That(parameters![0].ParameterType, Is.EqualTo(typeof(string)));
        Assert.That(parameters[0].IsOut, Is.False);
        Assert.That(parameters[0].ParameterType.IsByRef, Is.False);
    }

    [Test]
    public void Parse_RaisesEventWithCorrectCoordinates_TopLeftCorner()
    {
        var parser = new AsciiParser();
        StartEventArgs? capturedArgs = null;
        parser.StartPositionFound += (sender, e) => capturedArgs = e;

        parser.Parse("""
            +---+
            |x  |
            |   |
            +---+
            """);

        Assert.That(capturedArgs!.X, Is.EqualTo(1));
        Assert.That(capturedArgs.Y, Is.EqualTo(1));
    }

    [Test]
    public void Parse_RaisesEventWithCorrectCoordinates_BottomRightCorner()
    {
        var parser = new AsciiParser();
        StartEventArgs? capturedArgs = null;
        parser.StartPositionFound += (sender, e) => capturedArgs = e;

        parser.Parse("""
            +---+
            |   |
            |  x|
            +---+
            """);

        Assert.That(capturedArgs!.X, Is.EqualTo(3));
        Assert.That(capturedArgs.Y, Is.EqualTo(2));
    }
}
