namespace Domain.Entities.Template
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }

        public Entity(TId id)
        {
            Id = id;
        }
    }
}
