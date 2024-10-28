using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Interfaces.QueriesCompany
{
    public interface ICompanyQueryRepository
    {
        Task<IEnumerable<DomainBranchOffer>> GetOffersAsync
            (
            string orderBy,
            bool ascending,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd = null,
            DateOnly? workStart = null,
            DateOnly? workEnd = null,
            Guid? companyId = null,
            Guid? branchId = null,
            Guid? offerId = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            bool? isNegotietedSalary = null,
            int? divisionId = null,
            int maxItems = 100,
            int page = 1,
            CancellationToken cancellation = default
            );
    }
}
