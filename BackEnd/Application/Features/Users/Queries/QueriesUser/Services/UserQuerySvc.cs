using Application.Features.Users.Queries.QueriesUser.DTOs;
using Application.Features.Users.Queries.QueriesUser.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Characteristic.Repositories;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using System.Security.Claims;

namespace Application.Features.Users.Queries.QueriesUser.Services
{
    public class UserQuerySvc : IUserQuerySvc
    {
        //Values
        private readonly IUserQueryRepo _queriesRepository;
        private readonly ICharacteristicQueryRepository _charRepository;
        private readonly IAuthenticationSvc _authenticationService;

        //Controller
        public UserQuerySvc
            (
            IUserQueryRepo queriesRepository,
            ICharacteristicQueryRepository charRepository,
            IAuthenticationSvc authenticationService
            )
        {
            _queriesRepository = queriesRepository;
            _charRepository = charRepository;
            _authenticationService = authenticationService;
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
        public async Task<ResponseItem<GetUserDataResp>> GetUserDataAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            )
        {
            var id = GetId(claims);
            var domainUser = await _queriesRepository.GetUserDataAsync(id, cancellation);
            var ids = domainUser.ActiveOffersCharacteristicIds
                .Select(x => new CharacteristicId(x));
            var characteristics = _charRepository.GetCharList(ids);

            return new ResponseItem<GetUserDataResp>
            {
                Item = new GetUserDataResp(
                    domainUser.User,
                    domainUser.BranchCount,
                    domainUser.ActiveOffersCount,
                    characteristics
                    ),
            };
        }

        public async Task<ResponseItem<GetUrlsResp>> GetUrlsAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var id = GetId(claims);
            var data = await _queriesRepository
                .GetUrlsAsync(id, cancellation, searchText, orderBy, ascending, itemsCount, page);

            return new ResponseItem<GetUrlsResp>
            {
                Item = new GetUrlsResp(data.TotalCount, data.Items),
            };
        }

        public async Task<ResponseItem<GetNotificationsResp>> GetNotificationsAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            string? searchText = null,
            bool? hasReaded = null,
            int? senderId = null,
            int? statusId = null,
            DateTime? createdStart = null,
            DateTime? createdEnd = null,
            DateTime? completedStart = null,
            DateTime? completedEnd = null,
            string orderBy = "created", //completed
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var id = GetId(claims);
            var data = await _queriesRepository.GetNotificationsAsync
                (
                id,
                cancellation,
                searchText,
                hasReaded,
                senderId,
                statusId,
                createdStart,
                createdEnd,
                completedStart,
                completedEnd,
                orderBy,
                ascending,
                itemsCount,
                page
                );

            return new ResponseItem<GetNotificationsResp>
            {
                Item = new GetNotificationsResp(data.TotalCount, data.Items),
            };
        }

        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
        private UserId GetId(IEnumerable<Claim> claims)
        {
            return _authenticationService.GetIdNameFromClaims(claims);
        }
    }
}
