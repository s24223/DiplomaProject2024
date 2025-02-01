using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Characteristic.Entities;
using Domain.Features.Characteristic.Repositories;
using Domain.Features.Characteristic.ValueObjects.Identificators;
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
        private readonly ICharacteristicQueryRepository _characteristicRepository;
        //Values
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public Money? MinSalary { get; private set; } = null;
        public Money? MaxSalary { get; private set; } = null;
        public DatabaseBool? IsNegotiatedSalary { get; private set; } = null;
        public DatabaseBool IsForStudents { get; private set; } = null!;
        //Pochodne
        public bool IsPaid { get; private set; }


        //References
        //DomainBranchOffer
        private Dictionary<BranchOfferId, DomainBranchOffer> _branchOffers = [];
        public IReadOnlyDictionary<BranchOfferId, DomainBranchOffer> BranchOffers => _branchOffers;
        //Characteristic
        private Dictionary<CharacteristicId, (DomainCharacteristic, DomainQuality?)>
            _characteristics = [];
        public IReadOnlyDictionary<CharacteristicId, (DomainCharacteristic, DomainQuality?)>
            Characteristics => _characteristics;


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
            ICharacteristicQueryRepository characteristicRepository,
            IProvider provider
            ) : base(new OfferId(id), provider)
        {
            _characteristicRepository = characteristicRepository;
            SetValues
                (
                name,
                description,
                minSalary,
                maxSalary,
                (isNegotiatedSalary is null ? null : (DatabaseBool)isNegotiatedSalary),
                (DatabaseBool)isForStudents
                );
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public void SetBranchOffers(IEnumerable<DomainBranchOffer> branchOffers)
        {
            foreach (DomainBranchOffer offer in branchOffers)
            {
                SetBranchOffer(offer);
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
            SetValues(name, description, minSalary, maxSalary, isNegotiatedSalary, isForStudents);
        }

        public void SetCharacteristics(IEnumerable<(CharacteristicId, QualityId?)> values)
        {
            _characteristics.Clear();

            var dictionary = _characteristicRepository.GetCollocations(values);
            foreach (var pair in dictionary)
            {
                _characteristics[pair.Key.CharacteristicId] = pair.Value;
            }
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
        private void SetValues
            (
            string name,
            string description,
            decimal? minSalary,
            decimal? maxSalary,
            DatabaseBool? isNegotiatedSalary,
            DatabaseBool isForStudents
            )
        {
            Name = name;
            Description = description;
            MinSalary = (Money?)minSalary;
            MaxSalary = (Money?)maxSalary;
            IsNegotiatedSalary = isNegotiatedSalary;
            IsForStudents = isForStudents;

            //Pochodne
            IsPaid = MinSalary is not null && MinSalary.Value > 0;

            ThrowExceptionIfNotValid();
        }


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

        private void SetBranchOffer(DomainBranchOffer domainBranchOffer)
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
    }
}
