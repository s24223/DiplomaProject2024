using Application.Features.Companies.Queries.PublicCompany.DTOs;
using Application.Shared.DTOs.Features.Users.Urls;
using Domain.Features.Company.ValueObjects;
using Domain.Features.Offer.ValueObjects;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Queries.PublicCompany.Interfaces
{
    public interface ICompanyQueryRepo
    {
        Task<(IEnumerable<GetCompanyItemResp> Items, int TotalCount)> GetCompaniesAsync(
           CancellationToken cancellation,
           IEnumerable<int> characteristics,
           string? companyName = null,
           Regon? regon = null,
           string? searchText = null,
           string orderBy = "name", // characteristics
           bool ascending = true,
           bool hasActiveOffers = false,
           int maxItems = 100,
           int page = 1);
        /*
                Task<(IEnumerable<UrlResp> Items, int TotalCount)> GetUrlsAsync(
                    UserId? companyId,
                    UrlSegment? companyUrlSegment,
                    CancellationToken cancellation,
                    string? searchText = null,
                    string orderBy = "created", //typeId, name
                    bool ascending = true,
                    int maxItems = 100,
                    int page = 1);
        */
        Task<(IEnumerable<UrlResp> Items, int TotalCount)> GetUrlsAsync(
            UserId companyId,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int maxItems = 100,
            int page = 1);

        Task<(IEnumerable<GetOfferQueryResp> Items, int TotalCount)> GetOffersAsync(
           UserId companyId,
           CancellationToken cancellation,
           IEnumerable<int> characteristics,
           string? searchText = null,
           Money? minSalary = null,
           Money? maxSalary = null,
           bool? isForStudents = null,
           bool? isNegotiatedSalary = null,
           bool? hasActive = null,
           string orderBy = "publishStart",
           bool ascending = true,
           int maxItems = 100,
           int page = 1);

        Task<(IEnumerable<GetBranchQueryResp> Items, int TotalCount)> GetBranchesAsync(
            UserId companyId,
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
