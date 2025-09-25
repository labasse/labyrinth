using Xunit;

namespace Labyrinth.Tests;

public class IntegrationTests
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
    public void CompleteWorkflow_ParseMazeAndUnlockDoor()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        Assert.Equal(13, labyrinth.Width);
        Assert.Equal(9, labyrinth.Height);
        
        var tile = labyrinth.GetTile(3, 1);
        Assert.IsType<Door>(tile);
        
        var door = (Door)tile;
        
        Assert.False(door.IsTraversable);
        
        door.Unlock(door.Key);
        
        Assert.True(door.IsTraversable);
        
        door.Pass();
    }
    
    [Fact]
    public void FindKeyInLabyrinth_UnlockDoor()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        var keyRoom = (Room)labyrinth.GetTile(7, 3);
        Assert.NotNull(keyRoom.Item);
        Assert.IsType<Key>(keyRoom.Item);
        
        var foundKey = (Key)keyRoom.Item;
        var door = (Door)labyrinth.GetTile(3, 1);

        Assert.False(door.IsTraversable);
        
        door.Unlock(foundKey);
        Assert.True(door.IsTraversable);
    }
    
    [Fact]
    public void TraverseLabyrinth_CheckAllTileTypes()
    {
        var labyrinth = new Labyrinth(_testMaze);
        
        bool foundWall = false;
        bool foundRoom = false;
        bool foundDoor = false;
        bool foundRoomWithKey = false;
        
        for (int y = 0; y < labyrinth.Height; y++)
        {
            for (int x = 0; x < labyrinth.Width; x++)
            {
                var tile = labyrinth.GetTile(x, y);
                
                switch (tile)
                {
                    case Wall:
                        foundWall = true;
                        break;
                    case Door:
                        foundDoor = true;
                        break;
                    case Room room:
                        foundRoom = true;
                        if (room.Item is Key)
                            foundRoomWithKey = true;
                        break;
                }
            }
        }
        
        Assert.True(foundWall);
        Assert.True(foundRoom);
        Assert.True(foundDoor);
        Assert.True(foundRoomWithKey);
    }
}
