using Application.Databases.Relational;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.Interfaces.QueriesUser
{
    public class UserQueriesRepository : IUserQueriesRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public UserQueriesRepository
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
                throw new ArgumentException();
            }
            var domainUser = _mapper.ToDomainUser(databaseUser);
            foreach (var url in databaseUser.Urls)
            {
                domainUser.AddUrl(_mapper.ToDomainUrl(url));
            }
            foreach (var notification in databaseUser.Notifications)
            {
                domainUser.AddNotification(_mapper.ToDomainNotification(notification));
            }
            return domainUser;
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
    }
}
