using Domain.VerticalSlice.CompanyPart.Entities;

namespace Application.VerticalSlice.CompanyPart.Interfaces
{
    public interface ICompanyRepository
    {
        Task CreateCompanyProfileAsync(DomainCompany company, CancellationToken cancellation);
    }
}
