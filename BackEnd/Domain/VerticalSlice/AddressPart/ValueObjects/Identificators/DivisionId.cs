using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.VerticalSlice.AddressPart.ValueObjects.Identificators
{
    public record DivisionId : IntId
    {
        public DivisionId(int value) : base(value)
        {
        }
    }
}
