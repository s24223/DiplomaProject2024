using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects.EntityIdentificators;

namespace Domain.Entities.RecrutmentPart
{
    public class DomainIntership : Entity<IntershipId>
    {
        //Values
        public string ContractNumber { get; set; } = null!;


        //Refrences
        public Dictionary<CommentId, DomainComment> Comments { get; private set; } = new();
        //Recrutment Refrences
        public RecrutmentId RecrutmentId { get; private set; }
        public DomainRecrutment Recrutment { get; set; } = null!;


        //Cosntructor
        public DomainIntership
            (
            Guid? id,
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created,
            string contractNumber,
            IDomainProvider provider
            ) : base(new IntershipId(id), provider)
        {
            ContractNumber = contractNumber;

            RecrutmentId = new RecrutmentId(
                new BranchOfferId(
                    new BranchId(branchId),
                    new OfferId(offerId),
                    created),
                new UserId(personId)
                );
        }
    }
}
