using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Address.ValueObjects.Identificators
{
    public record AddressId : GuidId
    {
        public AddressId(Guid? value) : base(value)
        {
        }
    }
}
