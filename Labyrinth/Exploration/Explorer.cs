using Labyrinth.Crawl;
using Labyrinth.Events;
using Labyrinth.Tiles;

namespace Labyrinth.Exploration
{
    /// <summary>
    /// Controls the exploration of the labyrinth using a crawler.
    /// </summary>
    /// <remarks>
    /// The explorer performs random moves through the labyrinth until reaching
    /// an <see cref="Outside"/> tile or performing the given maximum number of steps.
    /// The random behavior can be mocked for deterministic testing.
    /// </remarks>
    public class Explorer
    {
        private readonly ICrawler _crawler;
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="Explorer"/> class with the specified crawler.
        /// </summary>
        /// <param name="crawler">The crawler used to move through the labyrinth.</param>
        /// <param name="random">Optional random number generator (used for testing).</param>
        public Explorer(ICrawler crawler, Random? random = null)
        {
            _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
            _random = random ?? new Random();
        }
        
        public event EventHandler<CrawlingEventArgs>? PositionChanged;
        public event EventHandler<CrawlingEventArgs>? DirectionChanged;
        
        public event EventHandler<CrawlingEventArgs>? ExitFound;

        private void OnPositionChanged() =>
            PositionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));
        private void OnDirectionChanged() =>
            DirectionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));
        
        private void OnExitFound() =>
            ExitFound?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));

        /// <summary>
        /// Randomly walks through the labyrinth up to <paramref name="n"/> steps
        /// or stops when an <see cref="Outside"/> tile is reached.
        /// </summary>
        /// <param name="n">The maximum number of moves to perform.</param>
        public void GetOut(int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);
            
            OnPositionChanged();
            OnDirectionChanged();

            for (var i = 0; i < n; i++)
            {
                if (TryTriggerExit())
                    break;

                PerformRandomAction();
                
                Thread.Sleep(10);
            }
        }
        
        private bool TryTriggerExit()
        {
            if (_crawler.FacingTile is not Outside) return false;
            OnExitFound();
            return true;
        }
        
        private void PerformRandomAction()
        {
            var action = _random.Next(3);
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
                    TryWalkForward();
                    break;
            }
        }

        private void TryWalkForward()
        {
            try
            {
                _crawler.Walk();
                OnPositionChanged();
                TryTriggerExit();
            }
            catch (InvalidOperationException)
            {
                TurnRandomly();
            }
        }

        private void TurnRandomly()
        {
            if (_random.Next(2) == 0)
                _crawler.Direction.TurnLeft();
            else
                _crawler.Direction.TurnRight();
            OnDirectionChanged();
        }
    }
}
