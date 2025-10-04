using Xunit;

namespace Labyrinth.Tests;

public class LabyrinthTests
{
    private readonly string testMazeLayout = @"+--+--+
|  /  |
|     |
+--+--+";

    [Fact]
    public void Labyrinth_Constructor_ShouldInitializeCorrectDimensions()
    {
        var labyrinth = new Labyrinth(testMazeLayout);
        Assert.Equal(7, labyrinth.Width);
        Assert.Equal(4, labyrinth.Height);
    }

    [Fact]
    public void Labyrinth_Tiles_ShouldReturnCorrectTileTypes()
    {
        var labyrinth = new Labyrinth(testMazeLayout);
        Assert.IsType<Wall>(labyrinth.GetTile(0, 0));  // + character
        Assert.IsType<Wall>(labyrinth.GetTile(1, 0));  // - character
        Assert.IsType<Wall>(labyrinth.GetTile(0, 1));  // | character
        Assert.IsType<Room>(labyrinth.GetTile(2, 1));  // space character
        Assert.IsType<Door>(labyrinth.GetTile(3, 1));  // / character
    }

    [Fact]
    public void Labyrinth_GetTile_ShouldThrowOnInvalidCoordinates()
    {
        var labyrinth = new Labyrinth(testMazeLayout);
        Assert.Throws<ArgumentOutOfRangeException>(() => labyrinth.GetTile(-1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => labyrinth.GetTile(0, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => labyrinth.GetTile(labyrinth.Width, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => labyrinth.GetTile(0, labyrinth.Height));
    }

    [Fact]
    public void Labyrinth_ComplexLayout_ShouldHandleAllCharacters()
    {
        var complexLayout = @"+--+--------+
|  /        |
|  +--+--+  |
|     |k    |
+--+  |  +--+
   |k       |
+  +-------/|
|           |
+-----------+";

        var labyrinth = new Labyrinth(complexLayout);
        // Vérifier quelques éléments clés
        Assert.IsType<Wall>(labyrinth.GetTile(0, 0));  // +
        Assert.IsType<Wall>(labyrinth.GetTile(1, 0));  // -
        Assert.IsType<Wall>(labyrinth.GetTile(0, 1));  // |
        Assert.IsType<Door>(labyrinth.GetTile(3, 1));  // /
        
        // Vérifier une salle avec clé
        var keyRoom = labyrinth.GetTile(7, 3) as Room;
        Assert.NotNull(keyRoom);
        Assert.NotNull(keyRoom.Item);
        Assert.IsType<Key>(keyRoom.Item);
    }

    [Fact]
    public void Labyrinth_DoorAndKey_ShouldBeProperlyLinked()
    {
        var layout = @"+--+--+
|  /  |
| k   |
+--+--+";

        var labyrinth = new Labyrinth(layout);
        var door = labyrinth.GetTile(3, 1) as Door;
        var room = labyrinth.GetTile(2, 2) as Room;
        
        Assert.NotNull(door);
        Assert.NotNull(room);
        Assert.NotNull(room.Item);
        
        var key = Assert.IsType<Key>(room.Item);
        Assert.True(door.TryUnlock(key));
    }

    [Fact]
    public void Labyrinth_Constructor_ShouldCreateValidWalls()
    {
        var layout = @"+--+
|  |
+--+";
        
        var labyrinth = new Labyrinth(layout);
        
        // Vérifier les coins
        Assert.IsType<Wall>(labyrinth.GetTile(0, 0)); // +
        Assert.IsType<Wall>(labyrinth.GetTile(3, 0)); // +
        Assert.IsType<Wall>(labyrinth.GetTile(0, 2)); // +
        Assert.IsType<Wall>(labyrinth.GetTile(3, 2)); // +
        
        // Vérifier les murs horizontaux
        Assert.IsType<Wall>(labyrinth.GetTile(1, 0)); // -
        Assert.IsType<Wall>(labyrinth.GetTile(2, 0)); // -
        
        // Vérifier les murs verticaux
        Assert.IsType<Wall>(labyrinth.GetTile(0, 1)); // |
        Assert.IsType<Wall>(labyrinth.GetTile(3, 1)); // |
    }
}
