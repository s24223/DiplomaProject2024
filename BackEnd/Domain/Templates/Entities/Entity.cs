using Domain.Providers;

namespace Domain.Templates.Entities
{
    public abstract class Entity<TId>
    {
        protected readonly IDomainProvider _provider;

        public TId Id { get; private set; }

        public Entity(TId id, IDomainProvider provider)
        {
            Id = id;
            _provider = provider;
        }
    }
}
