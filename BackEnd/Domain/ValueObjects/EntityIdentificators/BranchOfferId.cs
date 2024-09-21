namespace Domain.ValueObjects.EntityIdentificators
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
