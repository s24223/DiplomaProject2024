using Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer;
using Application.Shared.DTOs.Response;

namespace Application.Features.Companies.Services.QueriesCompany
{
    public interface ICompanyQueryService
    {
        Task<ResponseItems<OfferQueryResponseDto>> GetOffersAsync
            (
            string orderBy = "publishStart",
            bool ascending = true,
            bool? isPayed = null,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
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
