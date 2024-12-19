using Application.Features.Companies.Queries.QueriesPublic.DTOs;
using Application.Features.Companies.Queries.QueriesPublic.DTOs.BranchPart;
using Application.Features.Companies.Queries.QueriesPublic.DTOs.OffersPart;
using Application.Shared.DTOs.Response;

namespace Application.Features.Companies.Queries.QueriesPublic.Services
{
    public interface IBranchOfferQuerySvc
    {
        Task<ResponseItems<GetBranchOfferResp>> GetBranchOffersAsync(
            CancellationToken cancellation,
            IEnumerable<int> characteristics,
            string? jwt = null,
            string? companyName = null,
            string? regon = null,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            bool? isForStudents = null,
            bool? isNegotiatedSalary = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1);

        Task<ResponseItem<GetSingleBranchOfferResp>> GetBranchOfferAsync(
            Guid id,
            CancellationToken cancellation,
            string? jwt = null);

        Task<ResponseItem<GetOfferResp>> GetOfferAsync(
            Guid offerId,
            CancellationToken cancellation,
            string? jwt = null,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1);

        Task<ResponseItems<GetOfferBranchOfferResp>> GetOfferBranchOffersAsync(
            Guid offerId,
            CancellationToken cancellation,
            string? jwt = null,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1);

        Task<ResponseItem<GetBranchResp>> GetBranchAsync(
               Guid? companyId,
               Guid? branchId,
               string? companyUrlSegment,
               string? branchUrlSegment,
               CancellationToken cancellation,
               IEnumerable<int> characteristics,
               string? jwt = null,
               string? searchText = null,
               DateTime? publishFrom = null,
               DateTime? publishTo = null,
               DateTime? workFrom = null,
               DateTime? workTo = null,
               decimal? minSalary = null,
               decimal? maxSalary = null,
               bool? isForStudents = null,
               bool? isNegotiatedSalary = null,
               string orderBy = "publishStart",
               bool ascending = true,
               int maxItems = 100,
               int page = 1);

        Task<ResponseItems<GetBranchBranchOfferResp>> GetBranchBranchOffersAsync(
               Guid? companyId,
               Guid? branchId,
               string? companyUrlSegment,
               string? branchUrlSegment,
               CancellationToken cancellation,
               IEnumerable<int> characteristics,
               string? jwt = null,
               string? searchText = null,
               DateTime? publishFrom = null,
               DateTime? publishTo = null,
               DateTime? workFrom = null,
               DateTime? workTo = null,
               decimal? minSalary = null,
               decimal? maxSalary = null,
               bool? isForStudents = null,
               bool? isNegotiatedSalary = null,
               string orderBy = "publishStart",
               bool ascending = true,
               int maxItems = 100,
               int page = 1);
    }
}
