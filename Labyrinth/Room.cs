using Labyrinth.Models.Collectables;

namespace Labyrinth.Models.Tiles

{
    public class Room : Tile
    {
        //Une pièce est par définition traversable, pass ne fait rien, il peut y avoir un item
        public override bool IsTraversable => true;

        public override void Pass(){}

        public ICollectable? Item { get; set; }
    
    }
}