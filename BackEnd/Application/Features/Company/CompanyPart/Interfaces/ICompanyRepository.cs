using Domain.Features.Company.Entities;

namespace Application.Features.Company.CompanyPart.Interfaces
{
    public interface ICompanyRepository
    {
        Task CreateCompanyProfileAsync(DomainCompany company, CancellationToken cancellation);
    }
}
