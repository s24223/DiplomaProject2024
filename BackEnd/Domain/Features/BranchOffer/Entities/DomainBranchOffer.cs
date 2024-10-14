using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Exceptions.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Offer.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.BranchOffer.Entities
{
    public class DomainBranchOffer : Entity<BranchOfferId>
    {
        //Values
        public DateTime PublishStart { get; private set; }
        public DateTime? PublishEnd { get; private set; }
        public DateOnly? WorkStart { get; private set; }
        public DateOnly? WorkEnd { get; private set; }
        public DateTime LastUpdate { get; private set; }


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
            DateTime? lastUpdate,
            IProvider provider
            ) : base(new BranchOfferId
                (
                new BranchId(branchId),
                new OfferId(offerId),
                created
                ), provider)
        {
            LastUpdate = lastUpdate ?? _provider.TimeProvider().GetDateTimeNow();
            PublishStart = publishStart;
            PublishEnd = publishEnd;
            WorkStart = workStart;
            WorkEnd = workEnd;

            //ThrowExceptionIfIsNotValid();
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
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

        public void Update
            (
            DateTime publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd
            )
        {
            LastUpdate = _provider.TimeProvider().GetDateTimeNow();
            PublishStart = publishStart;
            PublishEnd = publishEnd;
            WorkStart = workStart;
            WorkEnd = workEnd;

            ThrowExceptionIfIsNotValid();
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
        private void ThrowExceptionIfIsNotValid()
        {
            if (
                PublishEnd is not null &&
                PublishEnd < PublishStart
                )
            {
                //Context: Data konca publikacji nie może byc mniejsza a niz data obecna
                throw new BranchOfferException(Messages.BranchOffer_PublishEnd_Invalid);
            }
            if (
                PublishEnd is not null &&
                WorkStart is not null &&
                _provider.TimeProvider().ConvertToDateTime(WorkStart.Value) < PublishEnd
                )
            {
                //Context: Data Początku pracy powinna być najwczesniij kolejnego dnia po ukonczeniu rekrutacji
                throw new BranchOfferException(Messages.BranchOffer_WorkStart_Invalid);
            }
            if (
                WorkStart is not null &&
                WorkEnd is not null &&
                WorkEnd < WorkStart
                )
            {
                //Context: Data pocztku pracy nie może byc wieksza od konca
                throw new BranchOfferException(Messages.BranchOffer_WorkEnd_Invalid);
            }
        }
    }
}
