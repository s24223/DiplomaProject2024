using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Offer.Exceptions.Entities;
using Domain.Features.Offer.Exceptions.ValueObjects;
using Domain.Features.Offer.ValueObjects;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;

namespace Domain.Features.Offer.Entities
{
    public class DomainOffer : Entity<OfferId>
    {
        //Values
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public Money? MinSalary { get; private set; }
        public Money? MaxSalary { get; private set; }
        public DatabaseBool? IsNegotiatedSalary { get; private set; }
        public DatabaseBool IsForStudents { get; private set; }


        //References
        //DomainBranchOffer
        private Dictionary<BranchOfferId, DomainBranchOffer> _branchOffers = new();
        public IReadOnlyDictionary<BranchOfferId, DomainBranchOffer> BranchOffers => _branchOffers;


        //Constructor
        public DomainOffer
            (
            Guid? id,
            string name,
            string description,
            decimal? minSalary,
            decimal? maxSalary,
            string? isNegotiatedSalary,
            string isForStudents,
            IProvider provider
            ) : base(new OfferId(id), provider)
        {
            //Values with exeptions
            MinSalary = minSalary == null ? null : new Money(minSalary.Value);
            MaxSalary = maxSalary == null ? null : new Money(maxSalary.Value);
            IsForStudents = new DatabaseBool(isForStudents);
            IsNegotiatedSalary = string.IsNullOrWhiteSpace(isNegotiatedSalary) ?
                null : new DatabaseBool(isNegotiatedSalary);

            //Values with  no exeptions
            Name = name;
            Description = description;

            //ThrowExceptionIfNotValid();
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public void AddAddBranchOffer(DomainBranchOffer domainBranchOffer)
        {
            if (
                domainBranchOffer.OfferId == Id &&
                !_branchOffers.ContainsKey(domainBranchOffer.Id)
                )
            {
                _branchOffers.Add(domainBranchOffer.Id, domainBranchOffer);
                domainBranchOffer.Offer = this;
            }
        }

        public void Update
            (
            string name,
            string description,
            decimal? minSalary,
            decimal? maxSalary,
            bool? isNegotiatedSalary,
            bool isForStudents
            )
        {

            //Values with exeptions
            MinSalary = minSalary == null ? null : new Money(minSalary.Value);
            MaxSalary = maxSalary == null ? null : new Money(maxSalary.Value);
            IsForStudents = new DatabaseBool(isForStudents);
            IsNegotiatedSalary = isNegotiatedSalary == null ?
                null : new DatabaseBool(isNegotiatedSalary.Value);

            //Values with  no exeptions
            Name = name;
            Description = description;

            ThrowExceptionIfNotValid();
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
        private void ThrowExceptionIfNotValid()
        {
            if (
                MaxSalary is not null &&
                MinSalary is not null &&
                MaxSalary < MinSalary
                )
            {
                throw new MoneyException(Messages.Offer_MaxSalary_Invalid);
            }
            if (
                IsNegotiatedSalary is null &&
                    (
                    MaxSalary is not null ||
                    MinSalary is not null
                    )
                )
            {
                throw new OfferException(Messages.Offer_IsNegotiatedSalary_Empty);
            }
        }
    }
}
