using Labyrinth.Tiles;
using System;

namespace Labyrinth.Crawl
{
    public class Explorer
    {
        private readonly ICrawler _crawler;
        private readonly Random _random;

        // --- Évènements pour la couche présentation
        public event EventHandler<CrawlingEventArgs>? PositionChanged;
        public event EventHandler<CrawlingEventArgs>? DirectionChanged;

        public Explorer(ICrawler crawler)
        {
            _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
            _random = new Random();
        }

        public void GetOut(int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (_crawler.FacingTile is Outside)
                {
                    Console.WriteLine("🚪 Sortie trouvée !");
                    break;
                }

                int action = _random.Next(3); // 0 = avancer, 1 = tourner gauche, 2 = tourner droite

                switch (action)
                {
                    case 0:
                        _crawler.Walk();
                        OnPositionChanged();
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
            }
        }

        private void OnPositionChanged()
        {
            PositionChanged?.Invoke(this,
                new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));
        }

        private void OnDirectionChanged()
        {
            DirectionChanged?.Invoke(this,
                new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));
        }
    }
}
