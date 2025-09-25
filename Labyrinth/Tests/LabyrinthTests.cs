using Xunit;

namespace Labyrinth.Tests;

public class LabyrinthTests
{
    private readonly string _testMaze = """
        +--+--------+
        |  /        |
        |  +--+--+  |
        |     |k    |
        +--+  |  +--+
           |k       |
        +  +-------/|
        |           |
        +-----------+
        """;
    
    [Fact]
    public void Constructor_ParsesMazeDimensions()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        Assert.Equal(13, labyrinth.Width);
        Assert.Equal(9, labyrinth.Height);
    }
    
    [Fact]
    public void Constructor_EmptyString_CreatesEmptyLabyrinth()
    {
        var labyrinth = new Labyrinth("");
        
        Assert.Equal(0, labyrinth.Width);
        Assert.Equal(0, labyrinth.Height);
    }
    
    [Fact]
    public void Constructor_SingleLine_ParsesCorrectly()
    {
        var maze = "+--+";
        var labyrinth = new Labyrinth(maze);
        
        Assert.Equal(4, labyrinth.Width);
        Assert.Equal(1, labyrinth.Height);
    }
    
    [Fact]
    public void GetTile_ValidCoordinates_ReturnsTile()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var tile = labyrinth.GetTile(0, 0);
        Assert.NotNull(tile);
    }
    
    [Fact]
    public void GetTile_WallSymbol_ReturnsWall()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var tile = labyrinth.GetTile(0, 0);
        Assert.IsType<Wall>(tile);
        Assert.False(tile.IsTraversable);
    }
    
    [Fact]
    public void GetTile_DoorSymbol_ReturnsDoor()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var tile = labyrinth.GetTile(3, 1);
        Assert.IsType<Door>(tile);
    }
    
    [Fact]
    public void GetTile_KeySymbol_ReturnsRoomWithKey()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var tile = labyrinth.GetTile(7, 3);
        Assert.IsType<Room>(tile);
        var room = (Room)tile;
        Assert.NotNull(room.Item);
        Assert.IsType<Key>(room.Item);
    }
    
    [Fact]
    public void GetTile_SpaceSymbol_ReturnsRoom()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var tile = labyrinth.GetTile(1, 1);
        Assert.IsType<Room>(tile);
        Assert.True(tile.IsTraversable);
    }
    
    [Fact]
    public void GetTile_HyphenSymbol_ReturnsWall()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var tile = labyrinth.GetTile(1, 0);
        Assert.IsType<Wall>(tile);
    }
    
    [Fact]
    public void GetTile_PipeSymbol_ReturnsWall()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var tile = labyrinth.GetTile(0, 1);
        Assert.IsType<Wall>(tile);
    }
    
    [Fact]
    public void GetTile_UnknownSymbol_ReturnsWall()
    {
        var maze = "x";
        var labyrinth = new Labyrinth(maze);
        
        var tile = labyrinth.GetTile(0, 0);
        Assert.IsType<Wall>(tile);
    }
    
    [Fact]
    public void GetTile_NegativeX_ThrowsArgumentOutOfRangeException()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => labyrinth.GetTile(-1, 0));
    }
    
    [Fact]
    public void GetTile_NegativeY_ThrowsArgumentOutOfRangeException()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => labyrinth.GetTile(0, -1));
    }
    
    [Fact]
    public void GetTile_XOutOfBounds_ThrowsArgumentOutOfRangeException()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => labyrinth.GetTile(labyrinth.Width, 0));
    }
    
    [Fact]
    public void GetTile_YOutOfBounds_ThrowsArgumentOutOfRangeException()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => labyrinth.GetTile(0, labyrinth.Height));
    }
    
    [Fact]
    public void GetTile_CoordinatesAtBoundary_ReturnsTile()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var tile = labyrinth.GetTile(labyrinth.Width - 1, labyrinth.Height - 1);
        Assert.NotNull(tile);
    }
    
    [Fact]
    public void Constructor_IrregularMaze_HandlesShortLines()
    {
        var irregularMaze = """
            +--+
            | 
            +--+
            """;
        
        var labyrinth = new Labyrinth(irregularMaze);
        
        var tile = labyrinth.GetTile(3, 1);
        Assert.IsType<Room>(tile);
    }
    
    [Fact]
    public void Labyrinth_MultipleDoorsAndKeys_CreatedCorrectly()
    {
        var maze = """
            /k/
            k/k
            """;
        
        var labyrinth = new Labyrinth(maze);
        
        Assert.IsType<Door>(labyrinth.GetTile(0, 0));
        Assert.IsType<Door>(labyrinth.GetTile(2, 0));
        Assert.IsType<Door>(labyrinth.GetTile(1, 1));
        
        var roomWithKey1 = (Room)labyrinth.GetTile(1, 0);
        var roomWithKey2 = (Room)labyrinth.GetTile(0, 1);
        var roomWithKey3 = (Room)labyrinth.GetTile(2, 1);
        
        Assert.IsType<Key>(roomWithKey1.Item);
        Assert.IsType<Key>(roomWithKey2.Item);
        Assert.IsType<Key>(roomWithKey3.Item);
    }
}
