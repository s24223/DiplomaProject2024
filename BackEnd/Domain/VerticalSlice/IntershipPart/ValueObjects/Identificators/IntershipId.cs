using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.VerticalSlice.IntershipPart.ValueObjects.Identificators
{
    public record IntershipId : GuidId
    {
        public IntershipId(Guid? value) : base(value)
        {
        }
    }
}
