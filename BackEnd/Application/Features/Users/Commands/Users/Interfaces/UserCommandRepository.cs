using Application.Databases.Relational;
using Application.Features.Users.Mappers;
using Domain.Features.User.Entities;
using Domain.Features.User.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.Users.Interfaces
{
    public class UserCommandRepository : IUserCommandRepository
    {
        //Values
        private readonly IUserMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public UserCommandRepository
            (
            IUserMapper mapper,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
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
                var databaseUser = new Databases.Relational.Models.User
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
                throw HandleException(ex);
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
                throw HandleException(ex);
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
                throw HandleException(ex);
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
            var domainUser = _mapper.DomainUser(databaseUser);

            return (domainUser, databaseUser.Password, databaseUser.Salt, databaseUser.RefreshToken, databaseUser.ExpiredToken);
        }

        public async Task<(DomainUser User, string Password, string Salt, string? RefreshToken, DateTime? ExpiredToken)> GetUserDataByIdAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databaseUser = await GetDatabaseUserAsync(id, cancellation);
            var domainUser = _mapper.DomainUser(databaseUser);

            return (domainUser, databaseUser.Password, databaseUser.Salt, databaseUser.RefreshToken, databaseUser.ExpiredToken);
        }

        //==========================================================================================================================================
        //==========================================================================================================================================
        //==========================================================================================================================================
        //Private Methods
        private async Task<Databases.Relational.Models.User> GetDatabaseUserAsync
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
                    $"{Messages.User_Cmd_Id_NotFound}: {id.Value}",
                    DomainExceptionTypeEnum.AppProblem
                    );
            }
            return databaseUser;
        }

        private async Task<Databases.Relational.Models.User> GetDatabaseUserAsync
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
                    Messages.User_Cmd_Unautorized,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }
            return databaseUser;
        }

        private System.Exception HandleException(System.Exception ex)
        {
            if (ex is DbUpdateException dbEx && dbEx.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;

                if (number == 2627)
                {
                    if (message.Contains("User_UNIQUE_Login"))
                    {
                        return new UserException(Messages.User_Cmd_Login_NotUnique);
                    }
                }
                if (number == 547)
                {
                    if (message.Contains("User_CHECK_IsHideProfile"))
                    {
                        return new UserException(
                            Messages.User_Cmd_Invalid_IsHideProfile,
                            DomainExceptionTypeEnum.AppProblem
                            );
                    }
                }
            }
            return ex;
        }
    }
}
