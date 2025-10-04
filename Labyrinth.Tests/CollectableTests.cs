using Xunit;

namespace Labyrinth.Tests;

public class CollectableTests
{
    [Fact]
    public void Key_ShouldImplementICollectable()
    {
        var key = new Key();
        Assert.IsAssignableFrom<ICollectable>(key);
    }

    [Fact]
    public void Room_ShouldAcceptICollectable()
    {
        var room = new Room();
        var collectable = new Key();
        room.Item = collectable;
        Assert.IsAssignableFrom<ICollectable>(room.Item);
    }

    [Fact]
    public void Room_ShouldAcceptNullAsItem()
    {
        var room = new Room();
        room.Item = null;
        Assert.Null(room.Item);
    }
}
