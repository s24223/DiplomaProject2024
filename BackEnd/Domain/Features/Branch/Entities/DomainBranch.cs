﻿using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Company.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;
namespace Domain.Features.Branch.Entities
{
    public class DomainBranch : Entity<BranchId>
    {
        //Values
        public UrlSegment? UrlSegment { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }


        //References
        //DomainCompany
        public UserId CompanyId { get; private set; } = null!;
        private DomainCompany _company = null!;
        public DomainCompany Company
        {
            get { return _company; }
            set
            {
                if (_company == null && value != null && value.Id == CompanyId)
                {
                    _company = value;
                    _company.AddBranch(this);
                }
            }
        }
        //BarnachOffer 
        private Dictionary<BranchOfferId, DomainBranchOffer> _branchOffers = new();
        public IReadOnlyDictionary<BranchOfferId, DomainBranchOffer> BranchOffers => _branchOffers;
        //Adress
#warning Add adress
        public AddressId AddressId { get; private set; } = null!;


        //Constructor
        public DomainBranch
            (
            Guid? id,
            Guid companyId,
            Guid addressId,
            string? urlSegment,
            string name,
            string? description,
            IProvider provider
            ) : base(new BranchId(id), provider)
        {
            //Values with exeptions
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ?
                null : new UrlSegment(urlSegment);

            //Values with no exeptions
            Name = name;
            CompanyId = new UserId(companyId);
            AddressId = new AddressId(addressId);
            Description = description;
        }


        //Methods
        public void AddBranchOffer(DomainBranchOffer domainBranchOffer)
        {
            if (
                domainBranchOffer.Id.BranchId == Id &&
                !_branchOffers.ContainsKey(domainBranchOffer.Id)
                )
            {
                _branchOffers.Add(domainBranchOffer.Id, domainBranchOffer);
                domainBranchOffer.Branch = this;
            }
        }
    }
}