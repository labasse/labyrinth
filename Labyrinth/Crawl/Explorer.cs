using Labyrinth.Tiles;
using System;

namespace Labyrinth.Crawl
{
    /// <summary>
    /// Explorer class allowing a crawler to move randomly in the labyrinth.
    /// </summary>
    public class Explorer
    {
        private readonly ICrawler _crawler;
        private readonly Random _random;

        public Explorer(ICrawler crawler)
        {
            _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
            _random = new Random();
        }

        /// <summary>
        /// Tries to exit the labyrinth with up to n moves.
        /// </summary>
        /// <param name="n">Maximum number of moves allowed</param>
        public void GetOut(int n)
        {
            for (int i = 0; i < n; i++)
            {
                // Si la tuile devant est dehors → on s'arrête
                if (_crawler.FacingTile is Outside)
                {
                    Console.WriteLine("🚪 Sortie trouvée !");
                    return;
                }

                // Choix aléatoire : avancer ou tourner
                int action = _random.Next(3); // 0 = avancer, 1 = tourner à gauche, 2 = tourner à droite

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

            Console.WriteLine("⏹ Aucun chemin trouvé après " + n + " déplacements.");
        }
    }
}
