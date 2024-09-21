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
        //DomainBranch
        private DomainBranch _branch = null!;
        public DomainBranch Branch
        {
            get { return _branch; }
            set
            {
                if (_branch == null && value != null && value.Id == Id.BranchId)
                {
                    _branch = value;
                    _branch.AddBranchOffer(this);
                }
            }
        }
        //DomainOffer
        private DomainOffer _offer = null!;
        public DomainOffer Offer
        {
            get { return _offer; }
            set
            {
                if (_offer == null && value != null && value.Id == Id.OfferId)
                {
                    _offer = value;
                    _branch.AddBranchOffer(this);
                }
            }
        }
        //DomainRecrutment
        private Dictionary<RecrutmentId, DomainRecrutment> _recrutments = new();
        public IReadOnlyDictionary<RecrutmentId, DomainRecrutment> Recrutments => _recrutments;


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


        //Methods
        public void AddRecrutment(DomainRecrutment domainRecrutment)
        {
            if (
                domainRecrutment.Id.BranchOfferId == this.Id &&
                !_recrutments.ContainsKey(domainRecrutment.Id)
                )
            {
                _recrutments.Add(domainRecrutment.Id, domainRecrutment);
                domainRecrutment.BranchOffer = this;
            }
        }
    }
}
