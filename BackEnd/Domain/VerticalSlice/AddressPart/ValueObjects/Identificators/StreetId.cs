using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.VerticalSlice.AddressPart.ValueObjects.Identificators
{
    public record StreetId : IntId
    {
        public StreetId(int value) : base(value)
        {
        }
    }
}
