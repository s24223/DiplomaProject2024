using Application.Features.Companies.Queries.QueriesUser.DTOs;
using Application.Features.Companies.Queries.QueriesUser.DTOs.BranchResponse;
using Application.Features.Companies.Queries.QueriesUser.DTOs.CompanyResponse;
using Application.Features.Companies.Queries.QueriesUser.DTOs.OfferResponse;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Offer.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Queries.QueriesUser.Interfaces
{
    public interface IUserCompanyQueryRepo
    {
        Task<(int TotalCount, IEnumerable<DomainBranch> Items)> GetCoreBranchesAsync
            (
            UserId companyId,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            );

        Task<(int TotalCount, IEnumerable<DomainOffer> Items)> GetCoreOffersAsync
            (
            UserId comapnyId,
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

        Task<CompanyWithDetailsResp> GetCompanyAsync(
            UserId companyId,
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

        Task<(IEnumerable<GetBranchCompanyResp> Items, int TotalCount)> GetBranchesWithDetailsAsync(
            UserId companyId,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1);

        Task<(IEnumerable<GetOfferCompanyResp> Items, int TotalCount)> GetOfferWithDetailsAsync(
            UserId companyId,
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

        Task<GetOfferResp> GetOfferAsync(
            UserId companyId,
            OfferId offerId,
            CancellationToken cancellation,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "publishstart",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1);

        Task<GetBranchResp> GetBranchAsync(
            UserId companyId,
            BranchId branchId,
            CancellationToken cancellation,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "publishstart",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1);

        Task<(IEnumerable<GetBranchOfferResp> Items, int TotalCount)> GetBranchOffersAsync(
            UserId companyId,
            CancellationToken cancellation,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "publishstart",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1);
    }
}
