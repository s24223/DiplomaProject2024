using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.VerticalSlice.OfferPart.ValueObjects.Identificators
{
    public record OfferId : GuidId
    {
        public OfferId(Guid? value) : base(value)
        {
        }
    }
}
