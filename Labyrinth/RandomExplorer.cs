using Labyrinth.Crawl;
using Labyrinth.Explorer;
using Labyrinth.Models;
using Labyrinth.Random;
using Labyrinth.Tiles;

namespace Labyrinth;

public class RandomExplorer(ICrawler crawler, IDirectionGenerator directionGenerator) : AbstractExplorer
{
    public int DelayBetweenMove { private get; init; } = 0;
    public int MaxActionAllowed { private get; init; } = 1000;

    public override void Explore()
    {
        GetOut();
    }

    public override event EventHandler<CrawlingEventArgs>? CrawlerMoved;

    private void GetOut()
    {
        int currentNbAction = 0;

        while (currentNbAction < MaxActionAllowed && crawler.FacingTile != Outside.Singleton)
        {
            Direction target = directionGenerator.NextDirection();

            bool hasTurned = false;

            while (target != crawler.Direction)
            {
                crawler.Direction.TurnRight();
                hasTurned = true;
            }

            if (hasTurned)
            {
                HandleCrawlerMoved();
            }

            if (crawler.FacingTile.IsTraversable)
            {
                crawler.Walk();
                currentNbAction++;
                HandleCrawlerMoved();
            }
        }
    }


    private void HandleCrawlerMoved()
    {
        Direction directionCopy = crawler.Direction.Clone();
        CrawlerMoved?.Invoke(this, new CrawlingEventArgs(crawler.Coord, directionCopy));
        Thread.Sleep(DelayBetweenMove);
    }
}