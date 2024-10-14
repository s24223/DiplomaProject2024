using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Domain.Features.Recruitment.ValueObjects.Identificators
{
    public record RecrutmentId
    {
        //Values
        public BranchOfferId BranchOfferId { get; private set; }
        public UserId PersonId { get; private set; }

        //Constructor
        public RecrutmentId(BranchOfferId branchOfferId, UserId personId)
        {
            BranchOfferId = branchOfferId;
            PersonId = personId;
        }
    }
}
