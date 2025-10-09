using Labyrinth.Crawl;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Direction unit test class")]
public class PointTest
{
    [Test]
    public void TestInit()
    {
        var test = new Point(3, 4);

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(3));
        Assert.That(test.Y, Is.EqualTo(4));
    }

    [Test]
    public void TestAddDirection()
    {
        var test = new Point(3, 4) + Direction.North;

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(3));
        Assert.That(test.Y, Is.EqualTo(3));
    }

    [Test]
    public void TestSubtractDirection()
    {
        var test = new Point(3, 4) - Direction.North;

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(3));
        Assert.That(test.Y, Is.EqualTo(5));
    }
}