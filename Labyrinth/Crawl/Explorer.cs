using System;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    /// <summary>
    /// Explorer performs random moves using an ICrawler until it reaches an Outside tile
    /// or exhausts the allowed number of moves.
    /// </summary>
    public class Explorer
    {
        public Explorer(ICrawler crawler, Random? rng = null)
        {
            _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
            _rng = rng ?? new Random();
        }

        /// <summary>
        /// Perform up to n random moves (walk / turn left / turn right).
        /// Stops early if an Outside tile is detected in front of the crawler.
        /// Returns true if an Outside tile was reached (facing) during the process.
        /// </summary>
        public bool GetOut(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));

            for (int i = 0; i < n; i++)
            {
                // if the crawler faces Outside, consider the goal reached
                if (_crawler.FacingTile is Outside)
                {
                    return true;
                }

                int action = _rng.Next(3); // 0=walk, 1=turn left, 2=turn right
                switch (action)
                {
                    case 0:
                        // attempt to walk only when the facing tile is traversable
                        try
                        {
                            if (_crawler.FacingTile.IsTraversable)
                            {
                                _crawler.Walk();
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            // ignore failed walks and continue
                        }
                        break;
                    case 1:
                        _crawler.Direction.TurnLeft();
                        break;
                    case 2:
                        _crawler.Direction.TurnRight();
                        break;
                }
            }

            // final check
            return _crawler.FacingTile is Outside;
        }

        private readonly ICrawler _crawler;
        private readonly Random _rng;
    }
}
