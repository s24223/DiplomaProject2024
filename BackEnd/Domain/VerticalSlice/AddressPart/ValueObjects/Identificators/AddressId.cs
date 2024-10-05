using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.VerticalSlice.AddressPart.ValueObjects.Identificators
{
    public record AddressId : GuidId
    {
        public AddressId(Guid? value) : base(value)
        {
        }
    }
}
