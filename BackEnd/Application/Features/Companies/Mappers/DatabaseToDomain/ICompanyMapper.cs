using Application.Databases.Relational.Models;
using Domain.Features.Branch.Entities;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Company.Entities;
using Domain.Features.Offer.Entities;

namespace Application.Features.Companies.Mappers.DatabaseToDomain
{
    public interface ICompanyMapper
    {
        DomainCompany DomainCompany(Company databaseCompany);
        DomainBranch DomainBranch(Branch databaseBranch);
        DomainOffer DomainOffer(Offer databaseOffer);
        DomainBranchOffer DomainBranchOffer(BranchOffer databaseBranchOffer);
    }
}
