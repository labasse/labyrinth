using Xunit;

namespace Labyrinth.Tests;

public class WallTests
{
    [Fact]
    public void Wall_ShouldNotBeTraversable()
    {
        var wall = new Wall();
        Assert.False(wall.IsTraversable);
    }
    
    [Fact]
    public void Wall_PassShouldThrowException()
    {
        var wall = new Wall();
        Assert.Throws<InvalidOperationException>(() => wall.Pass());
    }
}
