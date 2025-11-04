using Labyrinth;
using Labyrinth.Crawl;
using Labyrinth.Randomization;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

public class ExplorerTest
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
    
    #region GetOut

    [Test]
    public void GetOut_AlreadyFacingOutsideAtStart_ReturnsTrue() {
        var crawler = NewCrawlerFor("""
                                    +x+
                                    | |
                                    +-+
                                    """);
        var explorer = NewExplorerWithSequenceRng(crawler, 0);
        var result = explorer.GetOut(n: 0);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public void GetOut_TurnRightOnceAndWalk_SeesOutsideAndReturnsTrue() {
        var crawler = NewCrawlerFor("""
                                    +-+
                                    |x 
                                    +-+
                                    """);

        var explorer = NewExplorerWithSequenceRng(crawler, (int)Act.TurnRight, (int)Act.Walk);

        var result = explorer.GetOut(n: 2);

        Assert.That(result, Is.True);
        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }
    
    [Test]
    public void GetOut_TurnLeftOnceAndWalk_SeesOutsideAndReturnsTrue() {
        var crawler = NewCrawlerFor("""
                                    +-+
                                     x|
                                    +-+
                                    """);

        var explorer = NewExplorerWithSequenceRng(crawler, (int)Act.TurnLeft,(int)Act.Walk);

        var result = explorer.GetOut(n: 2);

        Assert.That(result, Is.True);
        Assert.That(crawler.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void GetOut_MaxStepsExceeded_ReturnsFalse_WhenOutsideNotReached()
    {
        var crawler = NewCrawlerFor("""
                                    +-+
                                    |x|
                                    +-+
                                    """);

        var explorer = NewExplorerWithSequenceRng(crawler, (int)Act.Walk, (int)Act.TurnLeft, (int)Act.Walk, (int)Act.TurnRight, (int)Act.Walk);

        var result = explorer.GetOut(n: 5);
        
        Assert.That(result, Is.False);
        Assert.That(crawler.FacingTile, Is.Not.TypeOf<Outside>());
    }
    
    #endregion
}