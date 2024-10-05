using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.VerticalSlice.BranchOfferPart.ValueObjects.Identificators;
using Domain.VerticalSlice.BranchPart.Entities;
using Domain.VerticalSlice.BranchPart.ValueObjects.Identificators;
using Domain.VerticalSlice.OfferPart.Entities;
using Domain.VerticalSlice.OfferPart.ValueObjects.Identificators;
using Domain.VerticalSlice.RecruitmentPart.Entities;
using Domain.VerticalSlice.RecruitmentPart.ValueObjects.Identificators;

namespace Domain.VerticalSlice.BranchOfferPart.Entities
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
        private Dictionary<RecrutmentId, DomainRecruitment> _recrutments = new();
        public IReadOnlyDictionary<RecrutmentId, DomainRecruitment> Recrutments => _recrutments;


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
            IProvider provider
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
        public void AddRecrutment(DomainRecruitment domainRecrutment)
        {
            if (
                domainRecrutment.Id.BranchOfferId == Id &&
                !_recrutments.ContainsKey(domainRecrutment.Id)
                )
            {
                _recrutments.Add(domainRecrutment.Id, domainRecrutment);
                domainRecrutment.BranchOffer = this;
            }
        }
    }
}
