using Application.Features.Companies.Queries.PublicCompany.DTOs;
using Application.Features.Companies.Queries.PublicCompany.Interfaces;
using Application.Shared.DTOs.Features.Users.Urls;
using Application.Shared.DTOs.Response;
using Domain.Features.Company.ValueObjects;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Queries.PublicCompany.Services
{
    public class CompanyQuerySvc : ICompanyQuerySvc
    {
        //Values 
        private readonly ICompanyQueryRepo _repo;


        //Constructor
        public CompanyQuerySvc(ICompanyQueryRepo repo)
        {
            _repo = repo;
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
        public async Task<ResponseItems<GetCompanyItemResp>> GetCompaniesAsync(
           CancellationToken cancellation,
           IEnumerable<int> characteristics,
           string? companyName = null,
           string? regon = null,
           string? searchText = null,
           string orderBy = "name", // characteristics
           bool ascending = true,
           bool hasActiveOffers = false,
           int maxItems = 100,
           int page = 1)
        {
            var domainRegon = string.IsNullOrWhiteSpace(regon) ? null : new Regon(regon);

            var result = await _repo.GetCompaniesAsync(
                cancellation, characteristics, companyName, domainRegon, searchText,
                orderBy, ascending, hasActiveOffers, maxItems, page);

            return new ResponseItems<GetCompanyItemResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }
        /*
                public async Task<ResponseItems<UrlResp>> GetUrlsAsync(
                    Guid? companyId,
                    string? companyUrlSegment,
                    CancellationToken cancellation,
                    string? searchText = null,
                    string orderBy = "created", //typeId, name
                    bool ascending = true,
                    int maxItems = 100,
                    int page = 1)
                {
                    page = page < 1 ? 1 : page;
                    maxItems = maxItems < 10 ? 10 : maxItems;
                    maxItems = maxItems > 100 ? 100 : maxItems;
                    var id = companyId is null ? null : new UserId(companyId);

                    var result = await _repo.GetUrlsAsync(id,
                        (UrlSegment?)companyUrlSegment, cancellation,
                        searchText, orderBy, ascending, maxItems, page);

                    return new ResponseItems<UrlResp>
                    {
                        Items = result.Items.ToList(),
                        TotalCount = result.TotalCount,
                    };
                }*/
        public async Task<ResponseItems<UrlResp>> GetUrlsAsync(
            Guid companyId,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            page = page < 1 ? 1 : page;
            maxItems = maxItems < 10 ? 10 : maxItems;
            maxItems = maxItems > 100 ? 100 : maxItems;
            var id = new UserId(companyId);

            var result = await _repo.GetUrlsAsync(id, cancellation,
                searchText, orderBy, ascending, maxItems, page);

            return new ResponseItems<UrlResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };

        }

        public async Task<ResponseItems<GetOfferQueryResp>> GetOffersAsync(
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
           int page = 1)
        {
            page = page < 1 ? 1 : page;
            maxItems = maxItems < 10 ? 10 : maxItems;
            maxItems = maxItems > 100 ? 100 : maxItems;
            var id = new UserId(companyId);

            var result = await _repo.GetOffersAsync(id, cancellation, characteristics, searchText,
                minSalary, maxSalary, isForStudents, isNegotiatedSalary, hasActive, orderBy, ascending,
                maxItems, page);

            return new ResponseItems<GetOfferQueryResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }

        public async Task<ResponseItems<GetBranchQueryResp>> GetBranchesAsync(
            Guid companyId,
            CancellationToken cancellation,
            string? wojewodztwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            string orderBy = "hierarchy",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var result = await _repo.GetBranchesAsync(new UserId(companyId), cancellation,
                wojewodztwo, divisionName, streetName, searchText, orderBy, ascending, maxItems, page);

            return new ResponseItems<GetBranchQueryResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }
        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
    }
}
