using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Sys;
using Labyrinth.Tiles;
using System.Linq;

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
            MyInventory bag = new();

            for (; n > 0 && _crawler.FacingTile is not Outside; n--)
            {
                EventHandler<CrawlingEventArgs>? changeEvent;

                // Sécuriser l’appel à _rnd.Next()
                Actions action;
                try
                {
                    action = _rnd.Next();
                }
                catch
                {
                    action = Actions.TurnLeft; // valeur par défaut
                }

                if (_crawler.FacingTile.IsTraversable && action == Actions.Walk)
                {
                    var inventory = _crawler.Walk();
                    while (inventory.HasItems)
                    {
                        bag.MoveItemFrom(inventory);
                    }
                    changeEvent = PositionChanged;
                }
                else
                {
                    _crawler.Direction.TurnLeft();
                    changeEvent = DirectionChanged;
                }

                // Essayer les clés uniquement si la porte est verrouillée
                if (_crawler.FacingTile is Door door && door.IsLocked)
                {
                    foreach (var key in bag.Items.OfType<Key>().ToList())
                    {
                        if (door.Open(new MyInventory(key)))
                        {
                            break;
                        }
                    }
                }

                changeEvent?.Invoke(this, new CrawlingEventArgs(_crawler));
            }
            return n;
        }

        public event EventHandler<CrawlingEventArgs>? PositionChanged;
        public event EventHandler<CrawlingEventArgs>? DirectionChanged;
    }
}
