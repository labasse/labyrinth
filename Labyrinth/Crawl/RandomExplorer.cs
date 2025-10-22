using System;
using Labyrinth.Crawl;
using Labyrinth.Tiles;

namespace Labyrinth
{
    public class RandomExplorer
    {
        private readonly ICrawler crawler;
        private readonly Random random;

        public RandomExplorer(ICrawler crawler, Random? random = null)
        {
            this.crawler = crawler;
            this.random = random ?? new Random();
        }

        /// <summary>
        /// Effectue des déplacements aléatoires jusqu’à trouver une tuile Outside ou atteindre la limite n.
        /// </summary>
        public void GetOut(int n)
        {
            int i = 0;
            while (i < n)
            {
                // Choix aléatoire : 0 = Walk, 1 = TurnLeft, 2 = TurnRight
                var action = random.Next(3);

                switch (action)
                {
                    case 0:
                        {
                            if (crawler.FacingTile.IsTraversable)
                            {
                                ++i;
                                crawler.Walk();

                                if (crawler.FacingTile is Outside)
                                {
                                    return;
                                }
                            }

                            break;

                        }
                    case 1:
                        crawler.Direction.TurnLeft();
                        break;
                    case 2:
                        crawler.Direction.TurnRight();
                        break;
                }
            }
        }
    }
}
