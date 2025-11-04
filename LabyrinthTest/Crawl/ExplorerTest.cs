using Labyrinth;
using Labyrinth.Crawl;
using Labyrinth.Randomization;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

public class AssertThatEventArgs
{
    private enum Act { Walk = 0, TurnLeft = 1, TurnRight = 2 }
    
    private sealed class SequenceRandom(params int[] seq) : IRandomSource {
        private readonly Queue<int> _queue = new(seq);

        public int Next(int minInclusive, int maxExclusive) {
            if (_queue.Count == 0) {
                throw new AssertionException("SequenceRandom empty: provide at least one value.");
            }
            
            var fakeRandomValue = _queue.Dequeue();
            _queue.Enqueue(fakeRandomValue);

            if (fakeRandomValue < minInclusive || fakeRandomValue >= maxExclusive) {
                throw new AssertionException($"RNG value {fakeRandomValue} out of bounds [{minInclusive},{maxExclusive}).");
            }
            
            return fakeRandomValue;
        }
    }
    
    private static ICrawler NewCrawlerFor(string asciiMap) =>
        new Labyrinth.Labyrinth(asciiMap).NewCrawler();
    
    private static Explorer NewExplorerWithSequenceRng(ICrawler crawler, params int[] seq) =>
        new Explorer(crawler, new SequenceRandom(seq));
    
    private static void AssertThatArg(CrawlingEventArgs args, int x, int y, Direction dir) {
        using var all = Assert.EnterMultipleScope();

        Assert.That((args.X, args.Y), Is.EqualTo((x, y)));
        Assert.That(args.Direction, Is.EqualTo(dir));
    }

    private static void AssertThatOutside(ICrawler crawler, bool result) {
        using var all = Assert.EnterMultipleScope();
        Assert.That(result, Is.True);
        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }
    
    private static void AssertThatNotOutside(ICrawler crawler, bool result) {
        using var all = Assert.EnterMultipleScope();
        Assert.That(result, Is.False);
        Assert.That(crawler.FacingTile, Is.Not.TypeOf<Outside>());
    }

    private static void AssertThatEventCount(int dirCount, int posCount, int expectedDirCount, int expectedPosCount) {
        using var all = Assert.EnterMultipleScope();
        Assert.That(dirCount, Is.EqualTo(expectedDirCount));
        Assert.That(posCount, Is.EqualTo(expectedPosCount));
    }
    
    #region GetOut

    [Test]
    public void GetOut_AlreadyFacingOutsideAtStart_ReturnsTrue() {
        var crawler = NewCrawlerFor("""
                                    +x+
                                    | |
                                    +-+
                                    """);
        
        var explorer = NewExplorerWithSequenceRng(crawler, 0);
        
        int posCalls = 0, dirCalls = 0;
        explorer.PositionChanged += (_, __) => posCalls++;
        explorer.DirectionChanged += (_, __) => dirCalls++;
        var result = explorer.GetOut(n: 0);
        
        using var all = Assert.EnterMultipleScope();
        AssertThatEventCount(dirCalls, posCalls, 0, 0);
        AssertThatOutside(crawler, result);
    }

    [Test]
    public void GetOut_TurnRightOnceAndWalk_SeesOutsideAndReturnsTrue() {
        var crawler = NewCrawlerFor("""
                                    +-+
                                    |x 
                                    +-+
                                    """);

        var explorer = NewExplorerWithSequenceRng(crawler, (int)Act.TurnRight, (int)Act.Walk);
        
        var dirEvents = new List<CrawlingEventArgs>();
        var posEvents = new List<CrawlingEventArgs>();
        explorer.DirectionChanged += (_, e) => dirEvents.Add(e);
        explorer.PositionChanged  += (_, e) => posEvents.Add(e);

        var result = explorer.GetOut(n: 2);

        using var all = Assert.EnterMultipleScope();
        AssertThatOutside(crawler, result);
        AssertThatArg(dirEvents[0], 1, 1, Direction.East);
        AssertThatArg(posEvents[0], crawler.X, crawler.Y, crawler.Direction);
    }

    [Test]
    public void GetOut_TurnLeftOnceAndWalk_SeesOutsideAndReturnsTrue()
    {
        var crawler = NewCrawlerFor("""
                                    +-+
                                     x|
                                    +-+
                                    """);

        var explorer = NewExplorerWithSequenceRng(crawler, (int)Act.TurnLeft, (int)Act.Walk);

        var dirEvents = new List<CrawlingEventArgs>();
        var posEvents = new List<CrawlingEventArgs>();
        explorer.DirectionChanged += (_, e) => dirEvents.Add(e);
        explorer.PositionChanged += (_, e) => posEvents.Add(e);

        var result = explorer.GetOut(n: 2);

        using var all = Assert.EnterMultipleScope();
        
        AssertThatOutside(crawler, result);

        AssertThatArg(dirEvents[0], 1, 1, Direction.West);
        AssertThatArg(posEvents[0], crawler.X, crawler.Y, crawler.Direction);
    }

    [Test]
    public void GetOut_MaxStepsExceeded_ReturnsFalse_WhenOutsideNotReached()
    {
        var crawler = NewCrawlerFor("""
                                    +-+
                                    |x|
                                    +-+
                                    """);

        var explorer = NewExplorerWithSequenceRng(crawler, (int)Act.Walk, (int)Act.TurnLeft, (int)Act.Walk);

        var posCalls = 0;
        var dirEvents = new List<CrawlingEventArgs>();
        explorer.PositionChanged  += (_, __) => posCalls++;
        explorer.DirectionChanged += (_, e) => dirEvents.Add(e);

        var result = explorer.GetOut(n: 3);
        
        using var all = Assert.EnterMultipleScope();
        
        AssertThatArg(dirEvents[0], crawler.X, crawler.Y, crawler.Direction);
        AssertThatEventCount(dirEvents.Count, posCalls, 1, 0);
        AssertThatNotOutside(crawler, result);
    }
    
    #endregion
}