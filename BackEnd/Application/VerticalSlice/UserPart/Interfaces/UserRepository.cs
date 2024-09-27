using Application.Database;
using Application.Database.Models;
using Application.Shared.Exceptions.UserExceptions;
using Domain.Entities.UserPart;
using Domain.Factories;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;
using Microsoft.EntityFrameworkCore;

namespace Application.VerticalSlice.UserPart.Interfaces
{
    public class UserRepository : IUserRepository
    {
        //Values
        private readonly DiplomaProjectContext _context;
        private readonly IDomainFactory _domainFactory;

        //Constructor
        public UserRepository
            (
            DiplomaProjectContext context,
            IDomainFactory domainFactory
            )
        {
            _context = context;
            _domainFactory = domainFactory;
        }

        //Methods
        public async Task<bool> IsExistLoginAsync
            (
            Email loginEmail,
            CancellationToken cancellation
            )
        {
            var result = await _context.Users.Where(x => x.LoginEmail == loginEmail.Value)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellation);
            return result != null;
        }
        //==========================================================================================================================================
        //DDL
        public async Task CreateUserAsync
            (
            DomainUser user,
            string password,
            string salt,
            CancellationToken cancellation
            )
        {
            await _context.Users.AddAsync(new User
            {
                LoginEmail = user.Login.Value,
                Password = password,
                Salt = salt,
                LastUpdatePassword = user.LastUpdatePassword,
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateRefreshTokenAsync
            (
            DomainUser user,
            string refresh,
            DateTime validTo,
            DateTime lastLoginIn,
            CancellationToken cancellation
            )
        {
            var databaseUser = await GetDatabaseUserAsync(user.Id, cancellation);
            databaseUser.RefreshToken = refresh;
            databaseUser.ExpiredToken = validTo;
            databaseUser.LastLoginIn = lastLoginIn;

            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteRefreshTokenDataAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databaseUser = await GetDatabaseUserAsync(id, cancellation);

            databaseUser.RefreshToken = null;
            databaseUser.ExpiredToken = null;

            await _context.SaveChangesAsync(cancellation);
        }

        //==========================================================================================================================================
        //DQL
        public async Task<(DomainUser User, string Salt, string Password)> GetUserDataByLoginEmailAsync
            (
            Email login,
            CancellationToken cancellation
            )
        {
            var databaseUser = await GetDatabaseUserAsync(login, cancellation);

            var domainUser = _domainFactory.CreateDomainUser
                (
                databaseUser.Id,
                databaseUser.LoginEmail,
                databaseUser.LastLoginIn,
                databaseUser.LastUpdatePassword
                );

            return (domainUser, databaseUser.Salt, databaseUser.Password);
        }
        public async Task<(DomainUser User, string? RefreshToken, DateTime? ExpiredToken)> GetUserDataByIdAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databaseUser = await GetDatabaseUserAsync(id, cancellation);

            var domainUser = _domainFactory.CreateDomainUser
                (
                databaseUser.Id,
                databaseUser.LoginEmail,
                databaseUser.LastLoginIn,
                databaseUser.LastUpdatePassword
                );

            return (domainUser, databaseUser.RefreshToken, databaseUser.ExpiredToken);
        }
        //==========================================================================================================================================
        //==========================================================================================================================================
        //==========================================================================================================================================
        //Private Methods

        private async Task<User> GetDatabaseUserAsync
            (
            UserId id,
            CancellationToken cancellation
        )
        {
            var databaseUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id.Value, cancellation);
            if (databaseUser == null)
            {
                throw new UnauthorizedUserException();
            }
            return databaseUser;
        }

        private async Task<User> GetDatabaseUserAsync
            (
            Email email,
            CancellationToken cancellation
        )
        {
            var databaseUser = await _context.Users.Where(x => x.LoginEmail == email.Value)
                .Include(x => x.Person)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(cancellation);
            if (databaseUser == null)
            {
                throw new UnauthorizedUserException();
            }
            return databaseUser;
        }
    }
}
