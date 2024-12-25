using Application.Features.Companies.Queries.PublicCompany.DTOs;
using Application.Shared.DTOs.Features.Users.Urls;
using Application.Shared.DTOs.Response;

namespace Application.Features.Companies.Queries.PublicCompany.Services
{
    public interface ICompanyQuerySvc
    {
        Task<ResponseItems<GetCompanyItemResp>> GetCompaniesAsync(
           CancellationToken cancellation,
           IEnumerable<int> characteristics,
           string? companyName = null,
           string? regon = null,
           string? searchText = null,
           string orderBy = "name", // characteristics
           bool ascending = true,
           bool hasActiveOffers = false,
           int maxItems = 100,
           int page = 1);
        /*
                Task<ResponseItems<UrlResp>> GetUrlsAsync(
                    Guid? companyId,
                    string? companyUrlSegment,
                    CancellationToken cancellation,
                    string? searchText = null,
                    string orderBy = "created", //typeId, name
                    bool ascending = true,
                    int maxItems = 100,
                    int page = 1);
        */
        Task<ResponseItems<UrlResp>> GetUrlsAsync(
            Guid companyId,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int maxItems = 100,
            int page = 1);

        Task<ResponseItems<GetOfferQueryResp>> GetOffersAsync(
           Guid companyId,
           CancellationToken cancellation,
           IEnumerable<int> characteristics,
           string? searchText = null,
           decimal? minSalary = null,
           decimal? maxSalary = null,
           bool? isForStudents = null,
           bool? isNegotiatedSalary = null,
           bool? hasActive = null,
           string orderBy = "publishStart",
           bool ascending = true,
           int maxItems = 100,
           int page = 1);

        Task<ResponseItems<GetBranchQueryResp>> GetBranchesAsync(
            Guid companyId,
            CancellationToken cancellation,
            string? wojewodztwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            string orderBy = "hierarchy",
            bool ascending = true,
            int maxItems = 100,
            int page = 1);
    }
}
