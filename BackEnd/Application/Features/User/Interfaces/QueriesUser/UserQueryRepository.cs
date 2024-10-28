using Application.Databases.Relational;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Domain.Features.User.Entities;
using Domain.Features.User.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.Interfaces.QueriesUser
{
    public class UserQueryRepository : IUserQueryRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public UserQueryRepository
            (
            IEntityToDomainMapper mapper,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        public async Task<DomainUser> GetUserDataAsync(UserId id, CancellationToken cancellation)
        {
            var databaseUser = await _context.Users
                .Include(x => x.Urls)
                .Include(x => x.Notifications)
                .Where(x => x.Id == id.Value).FirstOrDefaultAsync(cancellation);

            if (databaseUser == null)
            {
                throw new UserException
                    (
                    Messages.User_Ids_NotFound,
                    Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.Unauthorized
                    );
            }

            var domainUser = _mapper.ToDomainUser(databaseUser);
            domainUser.AddUrls(databaseUser.Urls.Select(x => _mapper.ToDomainUrl(x)));
            domainUser.AddNotifications(databaseUser.Notifications.Select(x =>
                    _mapper.ToDomainNotification(x)
                ));
            return domainUser;
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
    }
}
