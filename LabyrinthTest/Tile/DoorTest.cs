using Labyrinth;

namespace LabyrinthTest.Tile;
using Labyrinth.Collectable;
using Xunit;

public class DoorTest
{
    [Fact]
    public void Door_Should_BeClosed_ByDefault()
    {
        // Arrange
        var key = new Key(Guid.NewGuid());
        var door = new Door(key); // And act at the same time

        // Assert
        Assert.False(door.Open);
    }

    [Fact]
    public void OpenDoor_WithCorrectKey_Should_OpenDoor()
    {
        // Arrange
        var key = new Key(Guid.NewGuid());
        var door = new Door(key);

        // Act
        door.OpenDoor(key);

        // Assert
        Assert.True(door.Open);
    }

    [Fact]
    public void OpenDoor_OnAlreadyOpenDoor_Should_KeepDoorOpen()
    {
        // Arrange
        var key = new Key(Guid.NewGuid());
        var door = new Door(key);
        door.OpenDoor(key);
        
        // Act
        door.OpenDoor(key);

        // Assert
        Assert.True(door.Open);
    }

    [Fact]
    public void OpenDoor_WithWrongKey_ShouldNot_OpenDoor()
    {
        // Arrange
        var key = new Key(Guid.NewGuid());
        var wrongKey = new Key(Guid.NewGuid());
        var door = new Door(key);

        // Act
        door.OpenDoor(wrongKey);

        // Assert
        Assert.False(door.Open);
    }

    [Fact]
    public void CloseDoor_WithCorrectKey_Should_CloseDoor()
    {
        // Arrange
        var key = new Key(Guid.NewGuid());
        var door = new Door(key);
        door.OpenDoor(key); // ouvrir d'abord

        // Act
        door.CloseDoor(key);

        // Assert
        Assert.False(door.Open);
    }

    [Fact]
    public void CloseDoor_OnAlreadyClosedDoor_Should_KeepDoorClosed()
    {
        // Arrange
        var key = new Key(Guid.NewGuid());
        var door = new Door(key); // ferme automatiquement
        
        // Act
        door.CloseDoor(key);

        // Assert
        Assert.False(door.Open);
    }

    [Fact]
    public void CloseDoor_WithWrongKey_ShouldNot_CloseDoor()
    {
        // Arrange
        var key = new Key(Guid.NewGuid());
        var wrongKey = new Key(Guid.NewGuid());
        var door = new Door(key);
        door.OpenDoor(key); // on ouvre d'abord

        // Act
        door.CloseDoor(wrongKey);

        // Assert
        Assert.True(door.Open);
    }
}