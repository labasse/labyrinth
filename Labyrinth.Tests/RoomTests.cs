using Xunit;

namespace Labyrinth.Tests;

public class RoomTests
{
    [Fact]
    public void Room_ShouldBeTraversableByDefault()
    {
        var room = new Room();
        Assert.True(room.IsTraversable);
    }
    
    [Fact]
    public void Room_ShouldHaveNoItemByDefault()
    {
        var room = new Room();
        Assert.Null(room.Item);
    }
    
    [Fact]
    public void Room_CanStoreAndRetrieveItem()
    {
        var room = new Room();
        var key = new Key();
        room.Item = key;
        Assert.Same(key, room.Item);
    }
}
