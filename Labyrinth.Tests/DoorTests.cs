using Xunit;

namespace Labyrinth.Tests;

public class DoorTests
{
    [Fact]
    public void Door_ShouldNotBeTraversableByDefault()
    {
        var door = new Door();
        Assert.False(door.IsTraversable);
    }
    
    [Fact]
    public void Door_UnlockWithCorrectKey_ShouldMakeTraversable()
    {
        var door = new Door();
        var key = door.Key;
        
        Assert.True(door.TryUnlock(key));
        Assert.True(door.IsTraversable);
    }
    
    [Fact]
    public void Door_UnlockWithWrongKey_ShouldNotMakeTraversable()
    {
        var door = new Door();
        var wrongKey = new Key();
        
        Assert.False(door.TryUnlock(wrongKey));
        Assert.False(door.IsTraversable);
    }
    
    [Fact]
    public void Door_PassWhenLocked_ShouldThrowException()
    {
        var door = new Door();
        Assert.Throws<InvalidOperationException>(() => door.Pass());
    }
    
    [Fact]
    public void Door_PassWhenUnlocked_ShouldNotThrowException()
    {
        var door = new Door();
        door.TryUnlock(door.Key);
        
        var exception = Record.Exception(() => door.Pass());
        Assert.Null(exception);
    }
}
