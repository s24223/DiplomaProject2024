using Application.Features.Companies.Queries.QueriesUser.DTOs;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Queries.QueriesUser.Services
{
    public interface IUserCompanySvc
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
    }
}
