using Application.Features.Companies.Queries.QueriesPublic.DTOs;
using Application.Features.Companies.Queries.QueriesPublic.DTOs.BranchPart;
using Application.Features.Companies.Queries.QueriesPublic.DTOs.OffersPart;
using Application.Features.Companies.Queries.QueriesPublic.Repositories;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Company.ValueObjects;
using Domain.Features.Offer.ValueObjects;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.ValueObjects;

namespace Application.Features.Companies.Queries.QueriesPublic.Services
{
    public class BranchOfferQuerySvc : IBranchOfferQuerySvc
    {
        //Values
        private readonly IAuthJwtSvc _jwtSvc;
        private readonly IBranchOfferQueryRepo _branchOfferRepo;


        //Constructor
        public BranchOfferQuerySvc(
            IAuthJwtSvc jwtSvc,
            IBranchOfferQueryRepo branchOfferRepo)
        {
            _jwtSvc = jwtSvc;
            _branchOfferRepo = branchOfferRepo;
        }


        //=====================================================================================
        //=====================================================================================
        //=====================================================================================
        //Public Methods
        public async Task<ResponseItems<GetBranchOfferResp>> GetBranchOffersAsync(
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
            int page = 1)
        {
            var userId = GetUserIdFromJwt(jwt);
            var domainRegon = string.IsNullOrWhiteSpace(regon) ?
                null : new Regon(regon);

            var result = await _branchOfferRepo.GetBranchOffersAsync(cancellation, characteristics,
              userId, companyName, domainRegon, wojewodstwo, divisionName, streetName,
              searchText, publishFrom, publishTo, workFrom, workTo,
              (Money?)minSalary, (Money?)maxSalary, isForStudents,
              isNegotiatedSalary, orderBy, ascending, maxItems, page);

            return new ResponseItems<GetBranchOfferResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }


        public async Task<ResponseItem<GetSingleBranchOfferResp>> GetBranchOfferAsync(
            Guid id,
            CancellationToken cancellation,
            string? jwt = null)
        {
            var userId = GetUserIdFromJwt(jwt);
            var result = await _branchOfferRepo.GetBranchOfferAsync(
                new Domain.Features.BranchOffer.ValueObjects.Identificators.BranchOfferId(id),
                cancellation,
                userId
                );

            return new ResponseItem<GetSingleBranchOfferResp>
            {
                Item = result,
            };
        }


        public async Task<ResponseItem<GetOfferResp>> GetOfferAsync(
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
            int page = 1)
        {
            var userId = GetUserIdFromJwt(jwt);
            var result = await _branchOfferRepo.GetOfferAsync(new OfferId(offerId),
                cancellation, userId, wojewodstwo, divisionName, streetName,
                searchText, publishFrom, publishTo, workFrom, workTo, orderBy, ascending, maxItems, page);

            return new ResponseItem<GetOfferResp>
            {
                Item = result,
            };
        }


        public async Task<ResponseItems<GetOfferBranchOfferResp>> GetOfferBranchOffersAsync(
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
            int page = 1)
        {
            var userId = GetUserIdFromJwt(jwt);
            var result = await _branchOfferRepo.GetOfferBranchOffersAsync(new OfferId(offerId),
                cancellation, userId, wojewodstwo, divisionName, streetName,
                searchText, publishFrom, publishTo, workFrom, workTo, orderBy, ascending, maxItems, page);

            return new ResponseItems<GetOfferBranchOfferResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }

        public async Task<ResponseItem<GetBranchResp>> GetBranchAsync(
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
               int page = 1)
        {
            var userId = GetUserIdFromJwt(jwt);
            var companyUrl = string.IsNullOrWhiteSpace(companyUrlSegment) ?
                null : new UrlSegment(companyUrlSegment);
            var branchUrl = string.IsNullOrWhiteSpace(branchUrlSegment) ?
                null : new UrlSegment(branchUrlSegment);

            var result = await _branchOfferRepo.GetBranchAsync(
            new UserId(companyId), new BranchId(branchId),
            companyUrl, branchUrl, cancellation, characteristics,
                userId, searchText, publishFrom, publishTo, workFrom, workTo, minSalary,
                maxSalary, isForStudents, isNegotiatedSalary, orderBy, ascending,
                maxItems, page);

            return new ResponseItem<GetBranchResp>
            {
                Item = result,
            };
        }

        public async Task<ResponseItems<GetBranchBranchOfferResp>> GetBranchBranchOffersAsync(
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
               int page = 1)
        {
            var userId = GetUserIdFromJwt(jwt);
            var companyUrl = string.IsNullOrWhiteSpace(companyUrlSegment) ?
                null : new UrlSegment(companyUrlSegment);
            var branchUrl = string.IsNullOrWhiteSpace(branchUrlSegment) ?
                null : new UrlSegment(branchUrlSegment);

            var result = await _branchOfferRepo.GetBranchBranchOffersAsync(
            new UserId(companyId), new BranchId(branchId),
            companyUrl, branchUrl, cancellation, characteristics,
                userId, searchText, publishFrom, publishTo, workFrom, workTo, minSalary,
                maxSalary, isForStudents, isNegotiatedSalary, orderBy, ascending,
                maxItems, page);

            return new ResponseItems<GetBranchBranchOfferResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }


        //=====================================================================================
        //=====================================================================================
        //=====================================================================================
        //Private Methods
        private UserId? GetUserIdFromJwt(string? jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
            {
                return null;
            }
            if (!_jwtSvc.IsJwtGeneratedByThisServerAndNotExpired(jwt))
            {
                return null;
            }
            return _jwtSvc.GetIdNameFromJwt(jwt);
        }
    }
}
