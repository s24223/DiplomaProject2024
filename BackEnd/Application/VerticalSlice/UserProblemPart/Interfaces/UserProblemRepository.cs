using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.Exceptions;
using Domain.Entities.UserPart;
using Domain.Exceptions.UserExceptions.EntitiesExceptions;
using Domain.Factories;
using Domain.ValueObjects.EntityIdentificators;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Application.VerticalSlice.UserProblemPart.Interfaces
{
    public class UserProblemRepository : IUserProblemRepository
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public UserProblemRepository
            (
            IDomainFactory domainFactory,
            IExceptionsRepository exceptionRepository,
            DiplomaProjectContext diplomaProjectContext
            )
        {
            _domainFactory = domainFactory;
            _exceptionRepository = exceptionRepository;
            _context = diplomaProjectContext;
        }


        //Methods
        //==============================================================================================
        //DML
        public async Task<Guid> CreateUserProblemAndReturnIdAsync
            (
            DomainUserProblem userProblem,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUserProblem = new UserProblem
                {
                    Created = userProblem.Created,
                    UserMessage = userProblem.UserMessage,
                    PreviousProblemId = (userProblem.PreviousProblemId == null) ?
                    null : userProblem.PreviousProblemId.Value,
                    UserId = (userProblem.UserId == null) ?
                    null : userProblem.UserId.Value,
                    Email = (userProblem.Email == null) ?
                    null : userProblem.Email.Value,
                    Status = userProblem.Status.Code,
                };
                await _context.UserProblems.AddAsync(databaseUserProblem, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return databaseUserProblem.Id;
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertDbException
                    (ex,
                    this.GetType(),
                    MethodBase.GetCurrentMethod()
                    );
            }
        }

        public async Task SetNewStatusUserProblemForAuthorizedAsync
            (
            DomainUserProblem userProblem,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUserProblem = await GetUserProblemAsync(userProblem.Id, cancellation);

                databaseUserProblem.Status = userProblem.Status.Code;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertDbException
                    (ex,
                    this.GetType(),
                    MethodBase.GetCurrentMethod()
                    );
            }
        }


        //==============================================================================================
        //DQL
        public async Task<DomainUserProblem> GetDomainUserProblemAsync
            (
            UserId userId,
            UserProblemId userProblemId,
            CancellationToken cancellation
            )
        {
            var databaseUserProblem = await GetUserProblemAsync(userId, userProblemId, cancellation);
            return _domainFactory.CreateDomainUserProblem
                (
                databaseUserProblem.Id,
                databaseUserProblem.Created,
                databaseUserProblem.UserMessage,
                databaseUserProblem.Response,
                databaseUserProblem.PreviousProblemId,
                databaseUserProblem.Email,
                databaseUserProblem.Status,
                databaseUserProblem.UserId
                );
        }
        //==============================================================================================
        //==============================================================================================
        //==============================================================================================
        //Private Methods
        private async Task<UserProblem> GetUserProblemAsync
            (
            UserId userId,
            UserProblemId userProblemId,
            CancellationToken cancellation
            )
        {
            var databaseUserProblem = await _context.UserProblems
                .Where(x => x.UserId == userId.Value && x.Id == userProblemId.Value)
                .FirstOrDefaultAsync(cancellation);
            if (databaseUserProblem == null)
            {
                throw new UserProblemException(Messages.NotExistUserProblem);
            }
            return databaseUserProblem;
        }

        private async Task<UserProblem> GetUserProblemAsync
            (
            UserProblemId userProblemId,
            CancellationToken cancellation
            )
        {
            var databaseUserProblem = await _context.UserProblems
                .Where(x => x.Id == userProblemId.Value)
                .FirstOrDefaultAsync(cancellation);
            if (databaseUserProblem == null)
            {
                throw new UserProblemException(Messages.NotExistUserProblem);
            }
            return databaseUserProblem;
        }
    }
}
