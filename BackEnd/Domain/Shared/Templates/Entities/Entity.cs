using Domain.Shared.Providers;

namespace Domain.Shared.Templates.Entities
{
    public abstract class Entity<TId>
    {
        protected readonly IProvider _provider;

        public TId Id { get; private set; }

        public Entity(TId id, IProvider provider)
        {
            Id = id;
            _provider = provider;
        }
    }
}
