using Domain.Entities.UserPart;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;

namespace Application.VerticalSlice.UserPart.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsExistLoginAsync
            (
            Email loginEmail,
            CancellationToken cancellation
            );

        Task CreateUserAsync
            (
            DomainUser user,
            string password,
            string salt,
            CancellationToken cancellation
        );
        Task UpdateRefreshTokenAsync
            (
            DomainUser user,
            string refresh,
            DateTime validTo,
            DateTime lastLoginIn,
            CancellationToken cancellation
            );

        Task DeleteRefreshTokenDataAsync
            (
            UserId id,
            CancellationToken cancellation
            );

        Task<(DomainUser User, string Salt, string Password)> GetUserDataByLoginEmailAsync
             (
             Email login,
             CancellationToken cancellation
             );

        Task<(DomainUser User, string? RefreshToken, DateTime? ExpiredToken)> GetUserDataByIdAsync
            (
            UserId id,
            CancellationToken cancellation
            );
    }
}
