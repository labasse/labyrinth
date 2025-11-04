using Labyrinth.Crawl;
using Labyrinth.Explorer;
using Labyrinth.Models;

namespace Labyrinth;

public class Screen(Labyrinth lab)
{
    private Coord? _lastCoord;
    private AbstractExplorer? _subscribedExplorer;

    public void DrawLabyrinth()
    {
        Console.Clear();
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine(lab);
    }

    public void SubscribeTo(AbstractExplorer explorer)
    {
        _subscribedExplorer = explorer;

        explorer.CrawlerMoved += Explorer_CrawlerMoved;
    }

    private void Explorer_CrawlerMoved(object? sender, CrawlingEventArgs e)
    {
        if (_lastCoord != null)
        {
            Console.SetCursorPosition(_lastCoord.X, _lastCoord.Y);
            Console.Write(" ");
        }

        Console.SetCursorPosition(e.Coord.X, e.Coord.Y);
        Console.Write(GetDirectionSymbol(e.Direction));

        _lastCoord = e.Coord;

        Console.SetCursorPosition(0, lab.Height);
    }

    public void Dispose()
    {
        if (_subscribedExplorer != null)
        {
            _subscribedExplorer.CrawlerMoved -= Explorer_CrawlerMoved;
            _subscribedExplorer = null;
        }
    }

    private char GetDirectionSymbol(Direction dir)
    {
        if (dir == Direction.North) return '↑';
        if (dir == Direction.South) return '↓';
        if (dir == Direction.East) return '→';
        if (dir == Direction.West) return '←';
        return '?';
    }
}