using Domain.Entities.UserPart;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;

namespace Application.VerticalSlice.UserPart.Interfaces
{
    public interface IUserRepository
    {
        //==========================================================================================================================================
        //DML
        //Data Part
        Task CreateUserAsync
            (
            DomainUser user,
            string password,
            string salt,
            CancellationToken cancellation
        );

        Task UpdateAsync
            (
            DomainUser user,
            string password,
            string salt,
            string? refreshToken,
            DateTime? expiredToken,
            CancellationToken cancellation
            );

        //Authentication Part
        Task LogOutAndDeleteRefreshTokenDataAsync
            (
            UserId id,
            CancellationToken cancellation
            );

        //==========================================================================================================================================
        //DQL
        Task<(DomainUser User, string Password, string Salt, string? RefreshToken, DateTime? ExpiredToken)> GetUserDataByLoginEmailAsync
             (
             Email login,
             CancellationToken cancellation
        );

        Task<(DomainUser User, string Password, string Salt, string? RefreshToken, DateTime? ExpiredToken)> GetUserDataByIdAsync
            (
            UserId id,
            CancellationToken cancellation
            );
    }
}
