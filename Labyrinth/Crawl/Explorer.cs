using System;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    public class Explorer
    {
        public Explorer(ICrawler crawler, Random? random = null)
        {
            _crawler = crawler;
            _random = random ?? Random.Shared;
        }

        public bool GetOut(int n)
        {
            for (var i = 0; i < n; i++)
            {
                if (_crawler.FacingTile is Outside)
                {
                    return true;
                }

                var action = _random.Next(3);
                if (action == 0)
                {
                    if (_crawler.FacingTile.IsTraversable)
                    {
                        _crawler.Walk();
                    }
                }
                else if (action == 1)
                {
                    _crawler.Direction.TurnLeft();
                }
                else
                {
                    _crawler.Direction.TurnRight();
                }
            }

            return _crawler.FacingTile is Outside;
        }

        private readonly ICrawler _crawler;
        private readonly Random _random;
    }
}
