using Application.Features.Companies.Queries.QueriesUser.DTOs;
using Application.Features.Companies.Queries.QueriesUser.DTOs.BranchResponse;
using Application.Features.Companies.Queries.QueriesUser.DTOs.CompanyResponse;
using Application.Features.Companies.Queries.QueriesUser.DTOs.OfferResponse;
using Application.Features.Companies.Queries.QueriesUser.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using System.Security.Claims;

namespace Application.Features.Companies.Queries.QueriesUser.Services
{
    public class UserCompanyQuerySvc : IUserCompanyQuerySvc
    {
        //Values
        private readonly IUserCompanyQueryRepo _userCompanyRepo;
        private readonly IAuthJwtSvc _authentication;

        //Constructor
        public UserCompanyQuerySvc
            (
            IUserCompanyQueryRepo userCompanyRepo,
            IAuthJwtSvc authentication
            )
        {
            _userCompanyRepo = userCompanyRepo;
            _authentication = authentication;
        }


        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Public Methods 
        public async Task<ResponseItem<GetCoreBranchesResp>> GetCoreBranchesAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var id = GetId(claims);
            var data = await _userCompanyRepo.GetCoreBranchesAsync
                (
                id,
                cancellation,
                divisionId,
                streetId,
                ascending,
                itemsCount,
                page
                );

            return new ResponseItem<GetCoreBranchesResp>
            {
                Item = new GetCoreBranchesResp(data.TotalCount, data.Items)
            };

        }


        public async Task<ResponseItem<GetCoreOffersResp>> GetCoreOffersAsync
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
            )
        {
            var id = GetId(claims);
            var data = await _userCompanyRepo.GetCoreOffersAsync
                (
                id,
                characteristics,
                cancellation,
                searchText,
                isNegotiatedSalary,
                isForStudents,
                minSalary,
                maxSalary,
                orderBy,
                ascending,
                itemsCount,
                page
                );
            return new ResponseItem<GetCoreOffersResp>
            {
                Item = new GetCoreOffersResp(data.Items, data.TotalCount),
            };
        }

        public async Task<ResponseItem<CompanyWithDetailsResp>> GetCompanyAsync(
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
            int pageOffer = 1)
        {
            try
            {

                var id = GetId(claims);
                var data = await _userCompanyRepo.GetCompanyAsync
                (
                id,
                characteristics,
                cancellation,
                divisionId,
                streetId,
                ascendingBranch,
                itemsCountBranch,
                pageBranch,
                searchText,
                isNegotiatedSalary,
                isForStudents,
                minSalary,
                maxSalary,
                orderByOffer,
                ascendingOffer,
                itemsCountOffer,
                pageOffer);


                return new ResponseItem<CompanyWithDetailsResp>
                {
                    Item = data,
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<ResponseItems<GetBranchCompanyResp>> GetBranchesWithDetailsAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1)
        {
            var id = GetId(claims);
            var result = await _userCompanyRepo.GetBranchesWithDetailsAsync(
                id,
                cancellation,
                divisionId,
                streetId,
                ascending,
                itemsCount,
                page);

            return new ResponseItems<GetBranchCompanyResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }

        public async Task<ResponseItems<GetOfferCompanyResp>> GetOfferWithDetailsAsync(
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
            int page = 1)
        {
            var id = GetId(claims);
            var result = await _userCompanyRepo.GetOfferWithDetailsAsync(
                id,
                characteristics,
                cancellation,
                searchText,
                isNegotiatedSalary,
                isForStudents,
                minSalary,
                maxSalary,
                orderBy,
                ascending,
                itemsCount,
                page);

            return new ResponseItems<GetOfferCompanyResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }


        public async Task<ResponseItem<GetOfferResp>> GetOfferAsync(
            IEnumerable<Claim> claims,
            Guid offerId,
            CancellationToken cancellation,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "publishstart",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1)
        {
            var id = GetId(claims);
            var result = await _userCompanyRepo.GetOfferAsync(
                id,
                new OfferId(offerId),
                cancellation,
                from,
                to,
                orderBy,
                ascending,
                itemsCount,
                page);

            return new ResponseItem<GetOfferResp>
            {
                Item = result,
            };
        }

        public async Task<ResponseItem<GetBranchResp>> GetBranchAsync(
           IEnumerable<Claim> claims,
           Guid branchId,
           CancellationToken cancellation,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "publishstart",
           bool ascending = true,
           int itemsCount = 100,
           int page = 1)
        {
            var id = GetId(claims);
            var result = await _userCompanyRepo.GetBranchAsync(
                id,
                new BranchId(branchId),
                cancellation,
                from,
                to,
                orderBy,
                ascending,
                itemsCount,
                page);

            return new ResponseItem<GetBranchResp>
            {
                Item = result,
            };
        }

        public async Task<ResponseItems<GetBranchOfferResp>> GetBranchOffersAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "publishstart",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1)
        {
            var id = GetId(claims);
            var result = await _userCompanyRepo.GetBranchOffersAsync(id, cancellation,
                from, to, orderBy, ascending, itemsCount, page);

            return new ResponseItems<GetBranchOfferResp>
            {
                Items = result.Items.ToList(),
                TotalCount = result.TotalCount,
            };
        }
        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Private Methods 
        private UserId GetId(IEnumerable<Claim> claims)
        {
            return _authentication.GetIdNameFromClaims(claims);
        }

    }
}
