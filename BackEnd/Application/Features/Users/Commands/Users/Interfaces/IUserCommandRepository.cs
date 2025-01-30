using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.ValueObjects;

namespace Application.Features.Users.Commands.Users.Interfaces
{
    public interface IUserCommandRepository
    {
        //==========================================================================================================================================
        //DML
        //Data Part
        Task<Guid> CreateUserAsync(
             DomainUser user,
             string salt,
             string password,
             string activationUrlSegment,
             CancellationToken cancellation);

        Task UpdateAsync(
            DomainUser user,
            string salt,
            string password,
            string? activationUrlSegment,

            string? refreshToken,
            DateTime? expiredToken,

            string? resetPasswordUrlSegment,
            DateTime? resetPasswordInitiated,

            bool isHideProfile,
            CancellationToken cancellation
            );

        //==========================================================================================================================================
        //DQL
        Task<(
            DomainUser User,
            string Salt,
            string Password,
            string? ActivationUrlSegment,

            string? RefreshToken,
            DateTime? ExpiredToken,

            string? ResetPasswordUrlSegment,
            DateTime? ResetPasswordInitiated,

            bool IsHideProfile,
            bool HasPersonProfile,
            bool HasCompanyProfile
            )> GetUserDataByLoginEmailAsync(
             Email login,
             CancellationToken cancellation
             );

        Task<(
            DomainUser User,
            string Salt,
            string Password,
            string? ActivationUrlSegment,

            string? RefreshToken,
            DateTime? ExpiredToken,

            string? ResetPasswordUrlSegment,
            DateTime? ResetPasswordInitiated,

            bool IsHideProfile,
            bool HasPersonProfile,
            bool HasCompanyProfile
            )> GetUserDataByIdAsync(
            UserId id,
            CancellationToken cancellation
            );
    }
}
