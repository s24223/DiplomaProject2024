using Application.Database;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.User.Entities;
using Domain.Features.User.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.UserPart.Interfaces
{
    public class UserRepository : IUserRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public UserRepository
            (
            IEntityToDomainMapper mapper,
            IExceptionsRepository exceptionRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _exceptionRepository = exceptionRepository;
            _context = context;
        }



        //==========================================================================================================================================
        //==========================================================================================================================================
        //==========================================================================================================================================
        //Public Methods
        //DML
        public async Task CreateUserAsync
            (
            DomainUser user,
            string password,
            string salt,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUser = new Application.Database.Models.User
                {
                    Login = user.Login.Value,
                    Password = password,
                    Salt = salt,
                    //LastPasswordUpdate by DB Default Default_User_LastUpdatePassword
                };
                await _context.Users.AddAsync(databaseUser, cancellation);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }


        public async Task UpdateAsync
            (
            DomainUser user,
            string password,
            string salt,
            string? refreshToken,
            DateTime? expiredToken,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUser = await GetDatabaseUserAsync(user.Id, cancellation);

                databaseUser.Login = user.Login.Value;
                databaseUser.Password = password;
                databaseUser.Salt = salt;
                databaseUser.RefreshToken = refreshToken;
                databaseUser.ExpiredToken = expiredToken;
                databaseUser.LastLoginIn = user.LastLoginIn;
                databaseUser.LastPasswordUpdate = user.LastPasswordUpdate;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }


        //Authentication Part
        public async Task LogOutAndDeleteRefreshTokenDataAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUser = await GetDatabaseUserAsync(id, cancellation);

                databaseUser.RefreshToken = null;
                databaseUser.ExpiredToken = null;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        //DQL
        public async Task<(DomainUser User, string Password, string Salt, string? RefreshToken, DateTime? ExpiredToken)> GetUserDataByLoginEmailAsync
             (
             Email login,
             CancellationToken cancellation
             )
        {
            var databaseUser = await GetDatabaseUserAsync(login, cancellation);
            var domainUser = _mapper.ToDomainUser(databaseUser);

            return (domainUser, databaseUser.Password, databaseUser.Salt, databaseUser.RefreshToken, databaseUser.ExpiredToken);
        }

        public async Task<(DomainUser User, string Password, string Salt, string? RefreshToken, DateTime? ExpiredToken)> GetUserDataByIdAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databaseUser = await GetDatabaseUserAsync(id, cancellation);
            var domainUser = _mapper.ToDomainUser(databaseUser);

            return (domainUser, databaseUser.Password, databaseUser.Salt, databaseUser.RefreshToken, databaseUser.ExpiredToken);
        }

        //==========================================================================================================================================
        //==========================================================================================================================================
        //==========================================================================================================================================
        //Private Methods
        private async Task<Application.Database.Models.User> GetDatabaseUserAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databaseUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id.Value, cancellation);
            if (databaseUser == null)
            {
                throw new UserException
                    (
                    Messages.User_Ids_NotFound,
                    DomainExceptionTypeEnum.AppProblem
                    );
            }
            return databaseUser;
        }

        private async Task<Application.Database.Models.User> GetDatabaseUserAsync
            (
            Email login,
            CancellationToken cancellation
            )
        {
            var databaseUser = await _context.Users.Where(x => x.Login == login.Value)
                .Include(x => x.Person)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(cancellation);
            if (databaseUser == null)
            {
                throw new UserException
                    (
                    Messages.User_Login_NotFound,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }
            return databaseUser;
        }
    }
}
