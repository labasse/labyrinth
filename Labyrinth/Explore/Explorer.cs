using Labyrinth.Crawl;

namespace Labyrinth.Explore;

/// <summary>
/// Fait avancer un crawler aléatoirement jusqu'à sortir ou après n actions.
/// </summary>
public class Explorer
{
    private readonly ICrawler _crawler;
    private readonly Random _rnd;

    public Explorer(ICrawler crawler, Random? rnd = null)
    {
        _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
        _rnd = rnd ?? new Random();
    }

    /// <summary>
    /// Effectue au plus n actions (turn left, turn right, walk).
    /// Retourne true si l'explorateur a atteint une tuile Outside (avant ou après un déplacement).
    /// </summary>
    public bool GetOut(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));

        for (int i = 0; i < n; i++)
        {
            if (_crawler.FacingTile.GetType().Name == "Outside") return true;

            int action = _rnd.Next(3);

            switch (action)
            {
                case 0:
                    _crawler.Direction.TurnLeft();
                    break;
                case 1:
                    _crawler.Direction.TurnRight();
                    break;
                case 2:
                    _crawler.Walk();
                    if (_crawler.FacingTile.GetType().Name == "Outside") return true;
                    break;
            }
        }
        return false;
    }
}
