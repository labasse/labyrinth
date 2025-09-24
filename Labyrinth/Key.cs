using System;

namespace Labyrinth
{
    public record Key(Guid Id) : ICollectable
    {
        public Key() : this(Guid.NewGuid()) { }
    }
}
