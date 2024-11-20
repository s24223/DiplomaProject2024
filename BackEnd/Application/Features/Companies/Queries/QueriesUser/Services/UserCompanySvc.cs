using Application.Features.Companies.Queries.QueriesUser.DTOs;
using Application.Features.Companies.Queries.QueriesUser.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.User.ValueObjects.Identificators;
using System.Security.Claims;

namespace Application.Features.Companies.Queries.QueriesUser.Services
{
    public class UserCompanySvc : IUserCompanySvc
    {
        //Values
        private readonly IUserCompanyRepo _userCompanyRepo;
        private readonly IAuthenticationSvc _authentication;

        //Constructor
        public UserCompanySvc
            (
            IUserCompanyRepo userCompanyRepo,
            IAuthenticationSvc authentication
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
