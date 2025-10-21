using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Models;
using Labyrinth.Tiles;

namespace Labyrinth;

public partial class Labyrinth
{
    private class Crawler : ICrawler
    {
        public Coord Coord { get; private set; }
        public Direction Direction { get; }

        public Tile FacingTile => _attachedLabyrinth.GetFacingTile(Coord, Direction);

        private readonly Labyrinth _attachedLabyrinth;

        public Crawler(Labyrinth attached_labyrinth, Coord coord, Direction initial_direction)
        {
            Coord = coord;
            Direction = initial_direction;
            _attachedLabyrinth = attached_labyrinth;
        }

        public Inventory Walk()
        {
            var inventory = FacingTile.Pass();
            Coord = new Coord(Coord.X + Direction.DeltaX, Coord.Y + Direction.DeltaY);
            return inventory;
        }
    }
}