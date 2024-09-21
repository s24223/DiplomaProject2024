using Domain.Entities.RecrutmentPart;
using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects.EntityIdentificators;

namespace Domain.Entities.CompanyPart
{
    public class DomainBranchOffer : Entity<BranchOfferId>
    {
        //Values
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnly? WorkStart { get; set; }
        public DateOnly? WorkEnd { get; set; }
        public DateTime LastUpdate { get; set; }


        //References
        public DomainBranch Branch { get; set; } = null!;
        public DomainOffer Offer { get; set; } = null!;
        public Dictionary<RecrutmentId, DomainRecrutment> Recrutments { get; private set; } = new();


        //Constructor
        public DomainBranchOffer
            (
            Guid branchId,
            Guid offerId,
            DateTime created,
            DateTime publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            DateTime lastUpdate,
            IDomainProvider provider
            ) : base(new BranchOfferId
                (
                new BranchId(branchId),
                new OfferId(offerId),
                created
                ), provider)
        {
            PublishStart = publishStart;
            PublishEnd = publishEnd;
            WorkStart = workStart;
            WorkEnd = workEnd;
            LastUpdate = lastUpdate;
        }
    }
}
