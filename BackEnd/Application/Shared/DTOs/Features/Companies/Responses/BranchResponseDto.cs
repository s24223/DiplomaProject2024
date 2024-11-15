using Application.Shared.DTOs.Features.Addresses;
using Domain.Features.Branch.Entities;

namespace Application.Shared.DTOs.Features.Companies.Responses
{
    public class BranchResponseDto
    {
        //Values
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? AddressId { get; set; }
        public string? UrlSegment { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public AddressResponseDto Address { get; set; }


        //Constructor
        public BranchResponseDto(DomainBranch domain)
        {
            Id = domain.Id.Value;
            CompanyId = domain.CompanyId.Value;
            AddressId = domain.AddressId.Value;
            UrlSegment = domain.UrlSegment;
            Name = domain.Name;
            Description = domain.Description;
            Address = new AddressResponseDto(domain.Address);
        }
    }
}
