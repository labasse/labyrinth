using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Randomization;
using Labyrinth.Tiles;

namespace Labyrinth;

public class Explorer(ICrawler crawler, IRandomSource? rng = null)
{
    private readonly ICrawler _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
    private readonly IRandomSource _rng = rng ?? new RandomSource();
    private enum Act { Walk = 0, TurnLeft = 1, TurnRight = 2 }
    
    public event EventHandler<CrawlingEventArgs>? PositionChanged;
    public event EventHandler<CrawlingEventArgs>? DirectionChanged;
    
    private void OnPositionChanged() =>
        PositionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));
    
    private void OnDirectionChanged() =>
        DirectionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));

    
    /// <summary>
    /// Performs up to <paramref name="n"/> random actions (walk or turn).
    /// Stops and returns true as soon as the tile ahead is an <see cref="Outside"/> tile.
    /// Returns false if no exit was found after all actions.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="n"/> is negative.</exception>
    public bool GetOut(int n) {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be >= 0");

        for (var i = 0; i < n; i++) {
            if (_crawler.FacingTile is Outside) return true;

            var action = (Act)_rng.Next(0, 3);
            switch (action) {
                case Act.Walk:
                    try {
                        _crawler.Walk();
                        OnPositionChanged();
                    }
                    catch (InvalidOperationException) { /* Ignored: cannot walk into wall */ }
                    break;
                case Act.TurnLeft:
                    _crawler.Direction.TurnLeft();
                    OnDirectionChanged();
                    break;
                case Act.TurnRight:
                    _crawler.Direction.TurnRight();
                    OnDirectionChanged();
                    break;
                default:
                    throw new System.Diagnostics.UnreachableException();
            }
        }
        return _crawler.FacingTile is Outside;
    }
}