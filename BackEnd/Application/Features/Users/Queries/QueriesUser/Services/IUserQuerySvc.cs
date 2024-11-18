using Application.Features.Users.Queries.QueriesUser.DTOs;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Users.Queries.QueriesUser.Services
{
    public interface IUserQuerySvc
    {
        Task<ResponseItem<GetUserDataResp>> GetUserDataAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            );

        Task<ResponseItem<GetUrlsResp>> GetUrlsAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            );

        Task<ResponseItem<GetNotificationsResp>> GetNotificationsAsync
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
            );
    }
}
