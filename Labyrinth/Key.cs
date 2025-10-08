namespace Labyrinth.Models.Collectables

{
    public record Key(Guid Id) : ICollectable

    //Les clées ont un Id unique pour la gestion des portes
    {
        public Key() : this(Guid.NewGuid()) { }

    }

}