using Application.Databases.Relational.Models;
using Application.Shared.DTOs.Features.Addresses;
using Domain.Features.Address.Entities;
using Domain.Features.Branch.Entities;

namespace Application.Shared.DTOs.Features.Companies.Responses
{
    public class BranchResp
    {
        //Values
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? AddressId { get; set; } = null;
        public string? UrlSegment { get; set; } = null;
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null;
        public AddressResponseDto Address { get; set; } = null!;


        //Constructor
        public BranchResp(DomainBranch domain)
        {
            Id = domain.Id.Value;
            CompanyId = domain.CompanyId.Value;
            AddressId = domain.AddressId.Value;
            UrlSegment = domain.UrlSegment;
            Name = domain.Name;
            Description = domain.Description;
            if (domain.Address != null)
            {
                Address = new AddressResponseDto(domain.Address);
            }
        }

        public BranchResp(Branch database, DomainAddress? address)
        {
            Id = database.Id;
            CompanyId = database.CompanyId;
            AddressId = database.AddressId;
            UrlSegment = database.UrlSegment;
            Name = database.Name;
            Description = database.Description;
            if (address != null)
            {
                Address = new AddressResponseDto(address);
            }

        }
    }
}
