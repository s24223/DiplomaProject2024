using Domain.Features.Company.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.CompanyPart.Interfaces
{
    public interface ICompanyRepository
    {
        //DML
        Task<Guid> CreateAsync(DomainCompany company, CancellationToken cancellation);
        Task UpdateAsync(DomainCompany company, CancellationToken cancellation);

        //DQL
        Task<DomainCompany> GetCompanyAsync
           (
           UserId id,
           CancellationToken cancellation
           );
    }
}
