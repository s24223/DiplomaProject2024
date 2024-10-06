using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Offer.ValueObjects.Identificators;

namespace Domain.Features.BranchOffer.ValueObjects.Identificators
{
    public record BranchOfferId
    {
        public BranchId BranchId { get; private set; }
        public OfferId OfferId { get; private set; }
        public DateTime Created { get; private set; }

        public BranchOfferId(BranchId branchId, OfferId offerId, DateTime created)
        {
            BranchId = branchId;
            OfferId = offerId;
            Created = created;
        }
    }
}
