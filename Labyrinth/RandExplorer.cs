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
    public class RandExplorer(ICrawler crawler, IEnumRandomizer<RandExplorer.Actions> rnd, IDoorOpeningStrategy doorOpener)
    {
        private readonly ICrawler _crawler = crawler;
        private readonly IEnumRandomizer<Actions> _rnd = rnd;
        private readonly IDoorOpeningStrategy _doorOpener = doorOpener;

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
                    _doorOpener.TryOpen(door, bag);
                }

                changeEvent?.Invoke(this, new CrawlingEventArgs(_crawler));
            }

            return n;
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
