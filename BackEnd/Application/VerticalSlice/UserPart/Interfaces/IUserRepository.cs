using Domain.Entities.UserPart;
using Domain.ValueObjects.ValueEmail;

namespace Application.VerticalSlice.UserPart.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsExistLoginEmailAsync
            (
            Email loginEmail,
            CancellationToken cancellation
            );

        Task SetUserAsync
           (
           User domainUser,
           string password,
           string salt,
           DateTime now,
           CancellationToken cancellation
           );

        Task<Application.Database.Models.User?> GetUserByLoginEmailAsync
            (
            string LoginEmail,
            CancellationToken cancellation
            );

        Task<Application.Database.Models.User?> GetUserByIdAsync
           (
           Guid id,
           CancellationToken cancellation
           );

        Task SetRefreshTokenDataLoginInAsync
            (
            Application.Database.Models.User user,
            string refresh,
            DateTime validTo,
            DateTime now
            );
        Task SetRefreshTokenDataLogOutAsync
            (
            Application.Database.Models.User user,
            CancellationToken cancellation
            );
    }
}
