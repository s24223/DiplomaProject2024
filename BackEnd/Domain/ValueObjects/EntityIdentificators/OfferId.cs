using Domain.Templates.ValueObjects.EntityIdentificators;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record OfferId : GuidId
    {
        public OfferId(Guid? value) : base(value)
        {
        }
    }
}
