namespace Domain.ValueObjects.EntityIdentificators
{
    public record RecrutmentId
    {
        public BranchOfferId BranchOfferId { get; private set; }
        public UserId PersonId { get; private set; }

        public RecrutmentId(BranchOfferId branchOfferId, UserId personId)
        {
            BranchOfferId = branchOfferId;
            PersonId = personId;
        }
    }
}
