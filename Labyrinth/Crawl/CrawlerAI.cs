using Labyrinth.Crawl;
using Labyrinth.Tiles;
using Labyrinth.Items;

namespace Labyrinth
{
    /// <summary>
    /// Contrôle automatique d’un crawler pour tenter de sortir du labyrinthe.
    /// </summary>
    public class CrawlerAI
    {
        private readonly ICrawler _crawler;
        private readonly Random _random = new();

        public CrawlerAI(ICrawler crawler)
        {
            _crawler = crawler;
        }

        /// <summary>
        /// Effectue des déplacements aléatoires jusqu'à atteindre une tuile Outside ou après n pas.
        /// </summary>
        /// <param name="n">Nombre maximum de déplacements.</param>
        /// <returns>true si une sortie a été trouvée, false sinon.</returns>
        public bool GetOut(int n)
        {
            for (int i = 0; i < n; i++)
            {
                // Si la tuile devant est une sortie, mission accomplie
                if (_crawler.FacingTile is Outside)
                {
                    Console.WriteLine($"Sortie atteinte après {i} déplacements !");
                    return true;
                }

                // Décision aléatoire : avancer ou tourner
                int action = _random.Next(3); // 0, 1, 2
                switch (action)
                {
                    case 0:
                        _crawler.Walk();
                        break;
                    case 1:
                        _crawler.Direction.TurnLeft();
                        break;
                    case 2:
                        _crawler.Direction.TurnRight();
                        break;
                }
            }

            Console.WriteLine($"Aucune sortie trouvée après {n} déplacements.");
            return false;
        }
    }
}
