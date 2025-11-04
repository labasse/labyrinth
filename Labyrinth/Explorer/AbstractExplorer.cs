using Labyrinth.Models;

namespace Labyrinth.Explorer;

public abstract class AbstractExplorer
{
    public abstract void Explore();
    
    public abstract event EventHandler<CrawlingEventArgs>? CrawlerMoved;
}