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

        public event EventHandler<CrawlingEventArgs>? PositionChanged;

        public event EventHandler<CrawlingEventArgs>? DirectionChanged;

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
                        OnPositionChanged();
                        if (_crawler.FacingTile is Outside)
                        {
                            return true;
                        }
                    }
                }
                else if (action == 1)
                {
                    _crawler.Direction.TurnLeft();
                    OnDirectionChanged();
                    if (_crawler.FacingTile is Outside)
                    {
                        return true;
                    }
                }
                else
                {
                    _crawler.Direction.TurnRight();
                    OnDirectionChanged();
                    if (_crawler.FacingTile is Outside)
                    {
                        return true;
                    }
                }
            }

            return _crawler.FacingTile is Outside;
        }

        private readonly ICrawler _crawler;
        private readonly Random _random;

        private void OnPositionChanged() =>
            PositionChanged?.Invoke(this, NewArgs());

        private void OnDirectionChanged() =>
            DirectionChanged?.Invoke(this, NewArgs());

        private CrawlingEventArgs NewArgs() =>
            new(_crawler.X, _crawler.Y, _crawler.Direction);
    }
}
