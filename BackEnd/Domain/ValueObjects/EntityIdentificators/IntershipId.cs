using Domain.Templates.ValueObjects.EntityIdentificators;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record IntershipId : GuidId
    {
        public IntershipId(Guid? value) : base(value)
        {
        }
    }
}
