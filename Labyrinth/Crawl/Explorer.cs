using System;
using Labyrinth.Items;
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
                if (FacingOutside())
                {
                    return true;
                }

                var action = _random.Next(3);
                switch (action)
                {
                    case 0:
                        if (TryMove())
                        {
                            return true;
                        }
                        break;
                    case 1:
                        _crawler.Direction.TurnLeft();
                        OnDirectionChanged();
                        break;
                    case 2:
                        _crawler.Direction.TurnRight();
                        OnDirectionChanged();
                        break;
                }

                if (FacingOutside())
                {
                    return true;
                }
            }

            return FacingOutside();
        }

        private readonly ICrawler _crawler;
        private readonly Random _random;
        private readonly MyInventory _bag = new();

        private void OnPositionChanged() =>
            PositionChanged?.Invoke(this, NewArgs());

        private void OnDirectionChanged() =>
            DirectionChanged?.Invoke(this, NewArgs());

        private CrawlingEventArgs NewArgs() =>
            new(_crawler.X, _crawler.Y, _crawler.Direction);

        private bool TryMove()
        {
            if (_crawler.FacingTile is Door door)
            {
                TryOpenDoor(door);
            }

            if (!_crawler.FacingTile.IsTraversable)
            {
                return false;
            }

            var inventory = _crawler.Walk();
            if (!_bag.HasItem && inventory.HasItem)
            {
                _bag.MoveItemFrom(inventory);
            }

            OnPositionChanged();
            return FacingOutside();
        }

        private void TryOpenDoor(Door door)
        {
            if (!door.IsTraversable && _bag.HasItem)
            {
                door.Open(_bag);
            }
        }

        private bool FacingOutside() =>
            _crawler.FacingTile is Outside;
    }
}
