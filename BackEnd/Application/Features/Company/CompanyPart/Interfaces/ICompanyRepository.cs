using Domain.Features.Company.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Company.CompanyPart.Interfaces
{
    public interface ICompanyRepository
    {
        //DML
        Task CreateCompanyAsync(DomainCompany company, CancellationToken cancellation);
        Task UpdateCompanyAsync(DomainCompany company, CancellationToken cancellation);

        //DQL
        Task<DomainCompany> GetDomainCompanyAsync
           (
           UserId id,
           CancellationToken cancellation
           );
    }
}
