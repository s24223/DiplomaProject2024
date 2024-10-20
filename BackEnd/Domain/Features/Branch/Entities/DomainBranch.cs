using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Company.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
namespace Domain.Features.Branch.Entities
{
    public class DomainBranch : Entity<BranchId>
    {
        //Values
        public UrlSegment? UrlSegment { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }


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

        //BranchOffer 
        private Dictionary<BranchOfferId, DomainBranchOffer> _branchOffers = new();
        public IReadOnlyDictionary<BranchOfferId, DomainBranchOffer> BranchOffers => _branchOffers;

        //Adress
        public AddressId? AddressId { get; private set; } = null!;
        private DomainAddress _address = null!;
        public DomainAddress Address
        {
            get { return _address; }
            set
            {
                if (value.Id != AddressId)
                {
                    throw new AddressException
                        (
                        Messages.Branch_Address_NotSameAddressId,
                        DomainExceptionTypeEnum.AppProblem
                        );
                }
                _address = value;
            }
        }


        //Constructor
        public DomainBranch
            (
            Guid? id,
            Guid companyId,
            Guid? addressId,
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
            AddressId = addressId == null ? null : new AddressId(addressId);
            Description = description;
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public void AddBranchOffer(DomainBranchOffer domainBranchOffer)
        {
            if (
                domainBranchOffer.BranchId == Id &&
                !_branchOffers.ContainsKey(domainBranchOffer.Id)
                )
            {
                _branchOffers.Add(domainBranchOffer.Id, domainBranchOffer);
                domainBranchOffer.Branch = this;
            }
        }

        public void Update
            (
            Guid addressId,
            string? urlSegment,
            string name,
            string? description
            )
        {
            AddressId = new AddressId(addressId);
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ?
                null : new UrlSegment(urlSegment);
            Name = name;
            Description = description;
        }
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
    }
}
