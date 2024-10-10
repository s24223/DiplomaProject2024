﻿using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
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
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Money? MinSalary { get; set; }
        public Money? MaxSalary { get; set; }
        public DatabaseBool? IsNegotiatedSalary { get; set; }
        public DatabaseBool IsForStudents { get; set; }


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
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public void AddAddBranchOffer(DomainBranchOffer domainBranchOffer)
        {
            if (
                domainBranchOffer.Id.OfferId == Id &&
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
        }
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
    }
}
