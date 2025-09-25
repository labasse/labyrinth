using Xunit;

namespace Labyrinth.Tests;

public class CollectableTests
{
    [Fact]
    public void Key_IsCollectable()
    {
        var key = new Key();
        Assert.IsAssignableFrom<ICollectable>(key);
    }
    
    [Fact]
    public void Key_Equality_SameInstance()
    {
        var key = new Key();
        Assert.Equal(key, key);
    }
    
    [Fact]
    public void Key_Equality_DifferentInstances()
    {
        var key1 = new Key();
        var key2 = new Key();
        Assert.Equal(key1, key2);
    }
}
