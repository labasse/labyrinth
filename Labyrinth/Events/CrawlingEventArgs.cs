using Labyrinth.Crawl;

namespace Labyrinth.Events
{
    /// <summary>
    /// Représente les arguments pour les événements liés au déplacement dans un crawler.
    /// </summary>
    public class CrawlingEventArgs : EventArgs
    {
        /// <summary>
        /// Obtient la position X du crawler.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Obtient la position Y du crawler.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Obtient la direction actuelle du crawler.
        /// </summary>
        public Direction Direction { get; private set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CrawlingEventArgs"/>.
        /// </summary>
        /// <param name="x">Position en X.</param>
        /// <param name="y">Position en Y.</param>
        /// <param name="direction">Direction actuelle.</param>
        public CrawlingEventArgs(int x, int y, Direction direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }
    }
}