using System;
using System.Linq;
using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Sys;
using Labyrinth.Tiles;

namespace Labyrinth
{
    /// <summary>
    /// Explorer that moves randomly through the labyrinth using an action randomizer.
    /// Collects items on visited tiles, attempts to open doors with any collected key,
    /// and publishes events when the crawler changes position or direction.
    /// </summary>
    /// <param name="crawler">Crawler instance used to navigate the labyrinth.</param>
    /// <param name="rnd">Randomizer that decides the next action.</param>
    public class RandExplorer(ICrawler crawler, IEnumRandomizer<RandExplorer.Actions> rnd)
    {
        private readonly ICrawler _crawler = crawler;
        private readonly IEnumRandomizer<Actions> _rnd = rnd;

        /// <summary>
        /// Possible actions performed by the explorer.
        /// </summary>
        public enum Actions
        {
            /// <summary>Turn left (counter-clockwise).</summary>
            TurnLeft,

            /// <summary>Attempt to step forward if the facing tile is traversable.</summary>
            Walk
        }

        /// <summary>
        /// Tries to exit the labyrinth within at most <paramref name="n"/> steps.
        /// Stops early if an <see cref="Outside"/> tile is reached.
        /// Collects all items from visited tiles and attempts to unlock doors using any collected key.
        /// Raises <see cref="PositionChanged"/> or <see cref="DirectionChanged"/> depending on the action executed.
        /// </summary>
        /// <param name="n">Maximum number of actions the explorer may perform. Must be strictly positive.</param>
        /// <returns>The number of unused steps (0 if all steps were consumed).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="n"/> is 0 or negative.</exception>
        public int GetOut(int n)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(n, 0, nameof(n));

            MyInventory bag = new();

            for (; n > 0 && _crawler.FacingTile is not Outside; n--)
            {
                EventHandler<CrawlingEventArgs>? changeEvent;

                if (_crawler.FacingTile.IsTraversable
                    && _rnd.Next() == Actions.Walk)
                {
                    var tileInventory = _crawler.Walk();

                    while (tileInventory.HasItems)
                    {
                        bag.MoveItemFrom(tileInventory);
                    }

                    changeEvent = PositionChanged;
                }
                else
                {
                    _crawler.Direction.TurnLeft();
                    changeEvent = DirectionChanged;
                }

                if (_crawler.FacingTile is Door door && door.IsLocked && bag.HasItems)
                {
                    TryOpenDoorWithCollectedKeys(door, bag);
                }

                changeEvent?.Invoke(this, new CrawlingEventArgs(_crawler));
            }

            return n;
        }

        /// <summary>
        /// Attempts to unlock the specified door using any key found in the explorer's inventory.
        /// Tries keys in order of collection, stopping when the correct one is used.
        /// Wrong keys are returned to the explorer's inventory.
        /// </summary>
        /// <param name="door">The door to open.</param>
        /// <param name="bag">Explorer's inventory containing all collected items.</param>
        private static void TryOpenDoorWithCollectedKeys(Door door, MyInventory bag)
        {
            var temp = new MyInventory();

            var keysToTry = bag.Items.OfType<Key>().Count();

            for (int i = 0; i < keysToTry && door.IsLocked; i++)
            {
                var itemsList = bag.Items.ToList();
                int keyIndex = itemsList.FindIndex(item => item is Key);
                if (keyIndex < 0)
                {
                    break;
                }

                temp.MoveItemFrom(bag, keyIndex);

                bool opened = door.Open(temp);

                if (!opened)
                {
                    bag.MoveItemFrom(temp);
                }
            }
        }

        /// <summary>
        /// Event raised when the explorer changes its position (after a successful walk).
        /// </summary>
        public event EventHandler<CrawlingEventArgs>? PositionChanged;

        /// <summary>
        /// Event raised when the explorer changes its facing direction (after turning).
        /// </summary>
        public event EventHandler<CrawlingEventArgs>? DirectionChanged;
    }
}
