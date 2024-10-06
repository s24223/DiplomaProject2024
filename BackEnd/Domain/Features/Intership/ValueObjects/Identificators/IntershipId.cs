using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Intership.ValueObjects.Identificators
{
    public record IntershipId : GuidId
    {
        public IntershipId(Guid? value) : base(value)
        {
        }
    }
}
