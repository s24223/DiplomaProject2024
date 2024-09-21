using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;
namespace Domain.Entities.CompanyPart
{
    public class DomainBranch : Entity<BranchId>
    {
        //Values
        public SegementUrl? UrlSegment { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }


        //References
        //Company References
        public UserId CompanyId { get; private set; } = null!;
        public DomainCompany Company { get; set; } = null!;
        //BarnachOffer References
        public Dictionary<BranchOfferId, DomainBranchOffer> BranchOffers { get; private set; } = new();
        //Adress References
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
            IDomainProvider provider
            ) : base(new BranchId(id), provider)
        {
            CompanyId = new UserId(companyId);
            AddressId = new AddressId(addressId);
            UrlSegment = (string.IsNullOrWhiteSpace(urlSegment)) ? null : new SegementUrl(urlSegment);
            Name = name;
            Description = description;
        }
    }
}
