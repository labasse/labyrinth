using Labyrinth.Events;

namespace LabyrinthTest.Events;

[TestFixture(Description = "StartEventArgs unit test class")]
public class StartEventArgsTest
{
    [Test]
    public void Constructor_InitializesXAndYProperties()
    {
        var eventArgs = new StartEventArgs(5, 10);

        using var all = Assert.EnterMultipleScope();
        Assert.That(eventArgs.X, Is.EqualTo(5));
        Assert.That(eventArgs.Y, Is.EqualTo(10));
    }

    [Test]
    public void Constructor_AcceptsZeroValues()
    {
        var eventArgs = new StartEventArgs(0, 0);

        using var all = Assert.EnterMultipleScope();
        Assert.That(eventArgs.X, Is.EqualTo(0));
        Assert.That(eventArgs.Y, Is.EqualTo(0));
    }

    [Test]
    public void Constructor_AcceptsNegativeValues()
    {
        var eventArgs = new StartEventArgs(-1, -1);

        using var all = Assert.EnterMultipleScope();
        Assert.That(eventArgs.X, Is.EqualTo(-1));
        Assert.That(eventArgs.Y, Is.EqualTo(-1));
    }

    [Test]
    public void Properties_AreSettable()
    {
        var eventArgs = new StartEventArgs(5, 10);

        eventArgs.X = 15;
        eventArgs.Y = 20;

        using var all = Assert.EnterMultipleScope();
        Assert.That(eventArgs.X, Is.EqualTo(15));
        Assert.That(eventArgs.Y, Is.EqualTo(20));
    }

    [Test]
    public void StartEventArgs_InheritsFromEventArgs()
    {
        var eventArgs = new StartEventArgs(1, 2);

        Assert.That(eventArgs, Is.InstanceOf<EventArgs>());
    }
}