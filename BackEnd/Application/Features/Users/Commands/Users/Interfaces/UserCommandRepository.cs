using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Companies.Mappers;
using Application.Features.Persons.Mappers;
using Application.Features.Users.Mappers;
using Domain.Features.User.Entities;
using Domain.Features.User.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
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
        private readonly ICompanyMapper _companyMapper;
        private readonly IPersonMapper _personMapper;
        private readonly DiplomaProjectContext _context;
        private readonly IProvider _provider;

        private static readonly string _dbTrue = new DatabaseBool(true).Code;
        private static readonly string _dbFalse = new DatabaseBool(false).Code;

        //Constructor
        public UserCommandRepository(
            IUserMapper mapper,
            ICompanyMapper companyMapper,
            IPersonMapper personMapper,
            DiplomaProjectContext context,
            IProvider provider
            )
        {
            _mapper = mapper;
            _companyMapper = companyMapper;
            _personMapper = personMapper;
            _context = context;
            _provider = provider;
        }



        //==========================================================================================================================================
        //==========================================================================================================================================
        //==========================================================================================================================================
        //Public Methods
        //DML
        public async Task<Guid> CreateUserAsync(
            DomainUser user,
            string salt,
            string password,
            string activationUrlSegment,
            CancellationToken cancellation)
        {
            try
            {
                var databaseUser = new Databases.Relational.Models.User
                {
                    Login = user.Login.Value,
                    Salt = salt,
                    Password = password,
                    CreatedProfileUrlSegment = activationUrlSegment,
                    //LastPasswordUpdate by DB Default Default_User_LastUpdatePassword
                };
                await _context.Users.AddAsync(databaseUser, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return databaseUser.Id;
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task UpdateAsync
            (
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
            )
        {
            try
            {
                var databaseUser = await GetDatabaseUserAsync(user.Id, cancellation);

                databaseUser.Login = user.Login.Value;
                databaseUser.Salt = salt;
                databaseUser.Password = password;
                databaseUser.CreatedProfileUrlSegment = activationUrlSegment;

                databaseUser.RefreshToken = refreshToken;
                databaseUser.ExpiredToken = expiredToken;

                databaseUser.LastLoginIn = user.LastLoginIn;
                databaseUser.LastPasswordUpdate = user.LastPasswordUpdate;

                databaseUser.ResetPasswordUrlSegment = resetPasswordUrlSegment;
                databaseUser.ResetPasswordInitiated = resetPasswordInitiated;

                databaseUser.IsHideProfile = isHideProfile ? _dbTrue : _dbFalse;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex);
            }
        }

        //DQL
        public async Task<(
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
            )> GetUserDataByLoginEmailAsync
             (
             Email login,
             CancellationToken cancellation
             )
        {
            var databaseUser = await GetDatabaseUserAsync(login, cancellation);
            var domainUser = _mapper.DomainUser(databaseUser);

            return (
                domainUser,
                databaseUser.Salt,
                databaseUser.Password,
                databaseUser.CreatedProfileUrlSegment,

                databaseUser.RefreshToken,
                databaseUser.ExpiredToken,

                databaseUser.ResetPasswordUrlSegment,
                databaseUser.ResetPasswordInitiated,

                databaseUser.IsHideProfile == _dbTrue,
                databaseUser.Person != null,
                databaseUser.Company != null
                );
        }

        public async Task<(
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
            )> GetUserDataByIdAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databaseUser = await GetDatabaseUserAsync(id, cancellation);
            var domainUser = _mapper.DomainUser(databaseUser);


            return (
                domainUser,
                databaseUser.Salt,
                databaseUser.Password,
                databaseUser.CreatedProfileUrlSegment,

                databaseUser.RefreshToken,
                databaseUser.ExpiredToken,

                databaseUser.ResetPasswordUrlSegment,
                databaseUser.ResetPasswordInitiated,

                databaseUser.IsHideProfile == _dbTrue,
                databaseUser.Person != null,
                databaseUser.Company != null
                );
        }

        public async Task DeleteAsync(
            UserId id,
            CancellationToken cancellation)
        {
            var user = await _context.Users
                .Include(x => x.Urls)
                .Include(x => x.Notifications)
                .Include(x => x.Person)
                .ThenInclude(x => x.PersonCharacteristics)
                .Include(x => x.Person)
                .ThenInclude(x => x.Recruitments)
                .ThenInclude(x => x.Internship)
                .ThenInclude(x => x.Comments)
                .Include(x => x.Company)
                .ThenInclude(x => x.Branches)
                .ThenInclude(x => x.BranchOffers)
                .ThenInclude(x => x.Recruitments)
                .ThenInclude(x => x.Internship)
                .ThenInclude(x => x.Comments)
                .Include(x => x.Company)
                .ThenInclude(x => x.Branches)
                .ThenInclude(x => x.BranchOffers)
                .ThenInclude(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)
                .Where(x => x.Id == id.Value)
                .FirstAsync(cancellation);


            if (user.Person != null)
            {

                foreach (var personRecruitment in user.Person.Recruitments)
                {
                    if (personRecruitment.Internship != null)
                    {
                        _context.Comments.RemoveRange(personRecruitment.Internship.Comments);
                        _context.Internships.Remove(personRecruitment.Internship);
                    }
                }
                _context.Recruitments.RemoveRange(user.Person.Recruitments);
                _context.PersonCharacteristics.RemoveRange(user.Person.PersonCharacteristics);
                _context.People.Remove(user.Person);
            }
            var offers = new List<Offer>();
            if (user.Company != null)
            {
                foreach (var branch in user.Company.Branches)
                {
                    foreach (var branchOffer in branch.BranchOffers)
                    {
                        foreach (var recruitment in branchOffer.Recruitments)
                        {
                            if (recruitment.Internship != null)
                            {
                                _context.Comments.RemoveRange(recruitment.Internship.Comments);
                                _context.Internships.Remove(recruitment.Internship);
                            }
                        }
                        _context.Recruitments.RemoveRange(branchOffer.Recruitments);
                        _context.OfferCharacteristics.RemoveRange(branchOffer.Offer.OfferCharacteristics);
                        offers.Add(branchOffer.Offer);
                    }
                    _context.BranchOffers.RemoveRange(branch.BranchOffers);
                }
                _context.Branches.RemoveRange(user.Company.Branches);
                _context.Companies.Remove(user.Company);
            }


            _context.Offers.RemoveRange(offers);
            _context.Urls.RemoveRange(user.Urls);
            _context.Notifications.RemoveRange(user.Notifications);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
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
                .Include(x => x.Person)
                .Include(x => x.Company)
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
