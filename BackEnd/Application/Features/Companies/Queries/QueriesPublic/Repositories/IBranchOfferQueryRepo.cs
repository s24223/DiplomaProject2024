using Application.Features.Companies.Queries.QueriesPublic.DTOs;
using Application.Features.Companies.Queries.QueriesPublic.DTOs.BranchPart;
using Application.Features.Companies.Queries.QueriesPublic.DTOs.OffersPart;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Company.ValueObjects;
using Domain.Features.Offer.ValueObjects;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.ValueObjects;

namespace Application.Features.Companies.Queries.QueriesPublic.Repositories
{
    public interface IBranchOfferQueryRepo
    {
        Task<(IEnumerable<GetBranchOfferResp> Items, int TotalCount)> GetBranchOffersAsync(
            CancellationToken cancellation,
            IEnumerable<int> characteristics,
            UserId? userId = null,
            string? companyName = null,
            Regon? regon = null,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            Money? minSalary = null,
            Money? maxSalary = null,
            bool? isForStudents = null,
            bool? isNegotiatedSalary = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1);

        Task<GetSingleBranchOfferResp> GetBranchOfferAsync(
            BranchOfferId id,
            CancellationToken cancellation,
            UserId? userId = null);

        Task<GetOfferResp> GetOfferAsync(
            OfferId offerId,
            CancellationToken cancellation,
            UserId? userId = null,
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

        Task<(IEnumerable<GetOfferBranchOfferResp> Items, int TotalCount)>
           GetOfferBranchOffersAsync(
           OfferId offerId,
           CancellationToken cancellation,
           UserId? userId = null,
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

        Task<GetBranchResp> GetBranchAsync(
               UserId? companyId,
               BranchId? branchId,
               UrlSegment? companyUrlSegment,
               UrlSegment? branchUrlSegment,
               CancellationToken cancellation,
               IEnumerable<int> characteristics,
               UserId? userId = null,
               string? searchText = null,
               DateTime? publishFrom = null,
               DateTime? publishTo = null,
               DateTime? workFrom = null,
               DateTime? workTo = null,
               Money? minSalary = null,
               Money? maxSalary = null,
               bool? isForStudents = null,
               bool? isNegotiatedSalary = null,
               string orderBy = "publishStart",
               bool ascending = true,
               int maxItems = 100,
               int page = 1);

        Task<(IEnumerable<GetBranchBranchOfferResp> Items, int TotalCount)>
            GetBranchBranchOffersAsync(
              UserId? companyId,
              BranchId? branchId,
              UrlSegment? companyUrlSegment,
              UrlSegment? branchUrlSegment,
              CancellationToken cancellation,
              IEnumerable<int> characteristics,
              UserId? userId = null,
              string? searchText = null,
              DateTime? publishFrom = null,
              DateTime? publishTo = null,
              DateTime? workFrom = null,
              DateTime? workTo = null,
              Money? minSalary = null,
              Money? maxSalary = null,
              bool? isForStudents = null,
              bool? isNegotiatedSalary = null,
              string orderBy = "publishStart",
              bool ascending = true,
              int maxItems = 100,
              int page = 1);
    }
}
