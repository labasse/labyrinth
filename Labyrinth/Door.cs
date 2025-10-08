using Labyrinth.Models.Collectables;

namespace Labyrinth.Models.Tiles

{
    public class Door : Tile
    {
        //On instancie une clé unique pour chaque porte
        public Key Key { get; } = new();
        //On met un membre isOpen pour ne pas masquer la propriété IsTraversable héritée de tile
        private bool isOpen;
        //IsTraversable prend la valeur de isOpen
        public override bool IsTraversable => isOpen;

        //Pass jette une exception que si la porte est fermée
        public override void Pass()
        {
            if (!isOpen)
                throw new NotImplementedException();
        }

        //On ajoute une méthode UseKey pour changer l'état isOpen de la porte si la clé est bonne
        //La méthode jette une exception si l'item est null, si ce n'est pas une clé ou si la clé n'est pas la bonne
        public bool UseKey(ICollectable? item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item), "Tu ne possèdes aucun objet.");

            if (item is not Key key)
                throw new InvalidOperationException("Cet objet ne peut pas être utilisé sur une porte.");

            if (key.Id != Key.Id)
                throw new InvalidOperationException("Ce n’est pas la bonne clé pour cette porte.");

            isOpen = !isOpen;
            return isOpen;
        }
    
    }
}