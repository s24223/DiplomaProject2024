using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Features.UserProblem.Entities;
using Domain.Features.UserProblem.Exceptions.Entities;
using Domain.Features.UserProblem.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.UserProblemPart.Interfaces
{
    public class UserProblemRepository : IUserProblemRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public UserProblemRepository
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


        //==============================================================================================
        //==============================================================================================
        //==============================================================================================
        //Public Methods
        //DML
        public async Task<Guid> CreateAndReturnIdAsync
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
                    PreviousProblemId = userProblem.PreviousProblemId == null ?
                    null : userProblem.PreviousProblemId.Value,
                    UserId = userProblem.UserId == null ?
                    null : userProblem.UserId.Value,
                    Email = userProblem.Email == null ?
                    null : userProblem.Email.Value,
                    Status = userProblem.Status.Code,
                };
                await _context.UserProblems.AddAsync(databaseUserProblem, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return databaseUserProblem.Id;
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task SetNewStatusForAuthorizedAsync
            (
            UserId userId,
            DomainUserProblem userProblem,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUserProblem = await GetUserProblemAsync
                    (
                    (userProblem.UserId ?? userId),
                    userProblem.Id,
                    cancellation
                    );

                databaseUserProblem.Status = userProblem.Status.Code;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }


        //==============================================================================================
        //DQL
        public async Task<DomainUserProblem> GetProblemAsync
            (
            UserId userId,
            UserProblemId userProblemId,
            CancellationToken cancellation
            )
        {
            var databaseUserProblem = await GetUserProblemAsync(userId, userProblemId, cancellation);
            return _mapper.ToDomainUserProblem(databaseUserProblem);
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
                .Where(x =>
                    x.UserId == userId.Value &&
                    x.Id == userProblemId.Value
                ).FirstOrDefaultAsync(cancellation);
            if (databaseUserProblem == null)
            {
                throw new UserProblemException
                    (
                    Messages.UserProblem_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseUserProblem;
        }
    }
}
