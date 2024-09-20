using Domain.Entities.CompanyPart;

namespace Application.VerticalSlice.CompanyPart.Interfaces
{
    public interface ICompanyRepository
    {
        Task CreateCompanyProfileAsync(DomainCompany company, CancellationToken cancellation);
    }
}
