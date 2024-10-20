using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.BranchOffer.ValueObjects.Identificators
{
    public record BranchOfferId : GuidId
    {
        public BranchOfferId(Guid? value) : base(value)
        {
        }
    }
}
