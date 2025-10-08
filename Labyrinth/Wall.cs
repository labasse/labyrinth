namespace Labyrinth.Models.Tiles

{
    public class Wall : Tile
    {
        //Un mur n'est pas traversable donc Pass jettera une exception
        public override bool IsTraversable => false;

        public override void Pass() {
            throw new NotImplementedException();
        }

    }
}
