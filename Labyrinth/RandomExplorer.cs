using Labyrinth.Crawl;
using Labyrinth.Explorer;
using Labyrinth.Random;
using Labyrinth.Tiles;

namespace Labyrinth;

public class RandomExplorer : IExplorer
{
    private readonly ICrawler _crawler;
    private readonly IDirectionGenerator _directionGenerator;

    public RandomExplorer(ICrawler crawler, IDirectionGenerator directionGenerator)
    {
        _crawler = crawler;
        _directionGenerator = directionGenerator;
    }

    public void Explore()
    {
        GetOut();
    }

    private void GetOut(int nbMaxAction = 100)
    {
        int currentNbAction = 0;
        
        while (currentNbAction < nbMaxAction && _crawler.FacingTile != Outside.Singleton)
        {
            Direction target = _directionGenerator.NextDirection();
            currentNbAction += TurnTo(target);
            
            if (_crawler.FacingTile.IsTraversable)
            {
                _crawler.Walk();
                currentNbAction++;
            }
        }
    }

    /// <summary>
    /// Fait tourner le crawler vers la direction cible en choisissant le sens le plus court.
    /// Retourne le nombre de rotations effectuées.
    /// </summary>
    /// <param name="target">La direction vers laquelle tourner.</param>
    /// <returns>Nombre de rotations effectuées.</returns>
    /// <example>
    /// Si le crawler est North et que target = West  → on tourne 1 fois à gauche.
    /// Si le crawler est North et target = East      → on tourne 1 fois à droite.
    /// Si le crawler est North et target = North     → on ne tourne pas.
    /// Si le crawler est North et target = South     → on tourne 2 fois à droite.
    /// </example>
    private int TurnTo(Direction target)
    {
        Direction[] directions = { Direction.North, Direction.East, Direction.South, Direction.West };
        int currentDirectionIndex = Array.FindIndex(directions, d => d == _crawler.Direction);
        int targetDirectionIndex = Array.FindIndex(directions, d => d == target);

        int delta = (targetDirectionIndex - currentDirectionIndex + 4) % 4;
        int turns = (delta == 3) ? 1 : delta;

        if (delta == 3)
            _crawler.Direction.TurnLeft();
        else
            for (int i = 0; i < turns; i++)
                _crawler.Direction.TurnRight();

        return turns;
    }
}