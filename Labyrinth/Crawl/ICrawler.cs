using Labyrinth.Items;
using Labyrinth.Models;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    /// <summary>
    /// Labyrinth crawler interface.
    /// </summary>
    public interface ICrawler
    {
        /// <summary>
        /// Gets the current X,Y position.
        /// </summary>
        Coord Coord { get; }

        /// <summary>
        /// Gets the current direction.
        /// </summary>
        Direction Direction { get; }

        /// <summary>
        /// Gets the tile in front of the crawler.
        /// </summary>
        Tile FacingTile { get; }

        /// <summary>
        /// Pass the tile in front of the crawler and move into it.
        /// </summary>
        /// <returns>An inventory of the collectable items in the place reached.</returns>
        Inventory Walk();
    }
}
