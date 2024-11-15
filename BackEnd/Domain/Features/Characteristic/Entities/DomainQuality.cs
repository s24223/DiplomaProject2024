using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.Characteristic.Entities
{
    public class DomainQuality : Entity<QualityId>
    {
        //Values
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;


        //Constructor
        public DomainQuality
            (
            int id,
            string name,
            string description,
            IProvider provider
            ) : base(new QualityId(id), provider)
        {
            Name = name;
            Description = description;
        }
    }
}
