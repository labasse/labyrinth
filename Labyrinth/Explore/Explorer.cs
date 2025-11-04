using Labyrinth.Crawl;
using Labyrinth.Tiles;

namespace Labyrinth.Explore;

/// <summary>
/// Fait avancer un crawler aléatoirement jusqu'à sortir ou après n actions.
/// </summary>
public class Explorer
{
    private readonly ICrawler _crawler;
    private readonly Random _rnd;

    public Explorer(ICrawler crawler, Random? rnd = null)
    {
        _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
        _rnd = rnd ?? new Random();
    }

    public event EventHandler<CrawlingEventArgs>? PositionChanged;
    public event EventHandler<CrawlingEventArgs>? DirectionChanged;

    private void OnPositionChanged() =>
        PositionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));
    private void OnDirectionChanged() =>
        DirectionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));

    public bool GetOut(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));

        OnPositionChanged();
        OnDirectionChanged();

        for (int i = 0; i < n; i++)
        {
            if (_crawler.FacingTile is Outside) return true;

            // 0 = left, 1 = right, 2 = walk
            int action = _rnd.Next(3);

            switch (action)
            {
                case 0:
                    _crawler.Direction.TurnLeft();
                    OnDirectionChanged();
                    break;
                case 1:
                    _crawler.Direction.TurnRight();
                    OnDirectionChanged();
                    break;
                case 2:
                    try
                    {
                        _crawler.Walk();
                        OnPositionChanged();
                        if (_crawler.FacingTile is Outside) return true;
                    }
                    catch (InvalidOperationException)
                    {
                        if (_rnd.Next(2) == 0)
                            _crawler.Direction.TurnLeft();
                        else
                            _crawler.Direction.TurnRight();
                        OnDirectionChanged();
                    }
                    break;
            }

            // petite pause pour voir l'animation
            Thread.Sleep(40);
        }
        return false;
    }
}
