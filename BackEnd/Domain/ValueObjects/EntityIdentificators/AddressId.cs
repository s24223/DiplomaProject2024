using Domain.Templates.ValueObjects.EntityIdentificators;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record AddressId : GuidId
    {
        public AddressId(Guid? value) : base(value)
        {
        }
    }
}
