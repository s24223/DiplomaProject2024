using Domain.Features.Notification.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Users.Queries.QueriesUser.Interfaces
{
    public interface IUserQueryRepo
    {
        Task<(
            DomainUser User,
            int BranchCount,
            int ActiveOffersCount,
            IEnumerable<int> ActiveOffersCharacteristicIds
            )>
            GetUserDataAsync(UserId id, CancellationToken cancellation);

        Task<(int TotalCount, IEnumerable<DomainUrl> Items)> GetUrlsAsync
            (
            UserId id,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            );

        Task<(int TotalCount, IEnumerable<DomainNotification> Items)> GetNotificationsAsync
            (
            UserId id,
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
