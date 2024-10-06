using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Offer.ValueObjects.Identificators
{
    public record OfferId : GuidId
    {
        public OfferId(Guid? value) : base(value)
        {
        }
    }
}
