using Application.Features.Companies.Queries.QueriesUser.DTOs;
using Application.Features.Companies.Queries.QueriesUser.DTOs.CompanyResponse;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Queries.QueriesUser.Services
{
    public interface IUserCompanyQuerySvc
    {
        Task<ResponseItem<GetCoreBranchesResp>> GetCoreBranchesAsync
           (
           IEnumerable<Claim> claims,
           CancellationToken cancellation,
           int? divisionId = null,
           int? streetId = null,
           bool ascending = true,
           int itemsCount = 100,
           int page = 1
           );

        Task<ResponseItem<GetCoreOffersResp>> GetCoreOffersAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            );

        Task<ResponseItem<CompanyWithDetailsResp>> GetCompanyAsync(
            IEnumerable<Claim> claims,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascendingBranch = true,
            int itemsCountBranch = 100,
            int pageBranch = 1,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderByOffer = "created",
            bool ascendingOffer = true,
            int itemsCountOffer = 100,
            int pageOffer = 1);

        Task<ResponseItems<BranchWithDeatilsToCompanyResp>> GetBranchesWithDetailsAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1);

        Task<ResponseItems<OfferWithDetailsToCompanyResp>> GetOfferWithDetailsAsync(
            IEnumerable<Claim> claims,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1);
    }
}
