using Labyrinth.Tiles;
using System;

namespace Labyrinth.Crawl
{
    public class Explorer
    {
        private readonly ICrawler _crawler;
        private readonly Random _random;

        // 👉 Nouveaux évènements
        public event EventHandler<CrawlingEventArgs>? PositionChanged;
        public event EventHandler<CrawlingEventArgs>? DirectionChanged;

    

        public Explorer(ICrawler crawler)
        {
            _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
            _random = new Random();
        }

        public void GetOut(int n)
{
    var rand = new Random();

    for (int i = 0; i < n; i++)
    {
        // On déclenche PositionChanged avant de bouger
        PositionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));

        // On effectue un mouvement aléatoire
        int action = rand.Next(3);

        if (action == 0)
        {
            _crawler.Walk();
        }
        else if (action == 1)
        {
            _crawler.Direction.TurnLeft();
            DirectionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));
        }
        else
        {
            _crawler.Direction.TurnRight();
            DirectionChanged?.Invoke(this, new CrawlingEventArgs(_crawler.X, _crawler.Y, _crawler.Direction));
        }

        // Arrêt si on atteint une sortie
        if (_crawler.FacingTile is Outside)
        {
            Console.WriteLine("Sortie trouvée !");
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
