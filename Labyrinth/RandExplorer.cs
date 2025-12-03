using System;
using System.Linq;
using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Sys;
using Labyrinth.Tiles;

namespace Labyrinth
{
    public class RandExplorer(ICrawler crawler, IEnumRandomizer<RandExplorer.Actions> rnd)
    {
        private readonly ICrawler _crawler = crawler;
        private readonly IEnumRandomizer<Actions> _rnd = rnd;

        public enum Actions
        {
            TurnLeft,
            Walk
        }

        public int GetOut(int n)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(n, 0, "n must be strictly positive");
            MyInventory bag = new ();

            for( ; n > 0 && _crawler.FacingTile is not Outside; n--)
            {
                EventHandler<CrawlingEventArgs>? changeEvent;

                if (_crawler.FacingTile.IsTraversable && _rnd.Next() == Actions.Walk)
                {
                    _crawler.Walk();

                    CollectTileInventory(bag, _crawler.FacingTile);

                    changeEvent = PositionChanged;
                }
                else
                {
                    _crawler.Direction.TurnLeft();
                    changeEvent = DirectionChanged;
                }

                if (_crawler.FacingTile is Door door && door.IsLocked)
                {
                    TryOpenDoor(door, bag);
                }

                changeEvent?.Invoke(this, new CrawlingEventArgs(_crawler));
            }

            return n;
        }

        private static void CollectTileInventory(Inventory bag, Tile tile)
        {
            while (tile.LocalInventory.HasItems)
            {
                bag.MoveItemFrom(tile.LocalInventory, 0);
            }
        }

        private static void TryOpenDoor(Door door, Inventory bag)
        {
            while (door.IsLocked && bag.HasItems)
            {
                // Chaque tentative déplace la clé puis
                // le Door.Open la rend à l'inventaire si mauvaise
                // ou la garde si bonne
                door.Open(bag);
            }
        }

        public event EventHandler<CrawlingEventArgs>? PositionChanged;
        public event EventHandler<CrawlingEventArgs>? DirectionChanged;
    }
}
