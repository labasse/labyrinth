using Xunit;

namespace Labyrinth.Tests;

public class KeyTests
{
    [Fact]
    public void Key_DefaultConstructor_ShouldCreateUniqueId()
    {
        var key1 = new Key();
        var key2 = new Key();
        Assert.NotEqual(key1.Id, key2.Id);
    }

    [Fact]
    public void Key_CustomIdConstructor_ShouldUseProvidedId()
    {
        var id = Guid.NewGuid();
        var key = new Key(id);
        Assert.Equal(id, key.Id);
    }

    [Fact]
    public void Key_SameId_ShouldBeEqual()
    {
        var id = Guid.NewGuid();
        var key1 = new Key(id);
        var key2 = new Key(id);
        Assert.Equal(key1.Id, key2.Id);
    }
}
