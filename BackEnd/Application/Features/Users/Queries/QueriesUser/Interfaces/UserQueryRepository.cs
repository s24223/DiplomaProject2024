using Application.Databases.Relational;
using Application.Features.Companies.Mappers.DatabaseToDomain;
using Application.Features.Persons.Mappers;
using Application.Features.Users.Mappers;
using Domain.Features.Notification.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;
using Domain.Features.User.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.QueriesUser.Interfaces
{
    public class UserQueryRepository : IUserQueryRepository
    {
        //Values
        private readonly IProvider _provider;
        private readonly IUserMapper _userMapper;
        private readonly IPersonMapper _personMapper;
        private readonly ICompanyMapper _companyMapper;
        private readonly DiplomaProjectContext _context;
        private readonly DateTime _now;

        //Constructor
        public UserQueryRepository
            (
            IProvider provider,
            IUserMapper userMapper,
            IPersonMapper personMapper,
            ICompanyMapper companyMapper,
            DiplomaProjectContext context
            )
        {
            _provider = provider;
            _userMapper = userMapper;
            _personMapper = personMapper;
            _companyMapper = companyMapper;
            _context = context;

            _now = _provider.TimeProvider().GetDateTimeNow();
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        public async Task
            <(
            DomainUser User,
            int BranchCount,
            int ActiveOffersCount,
            IEnumerable<int> ActiveOffersCharacteristicIds
            )>
            GetUserDataAsync(UserId id, CancellationToken cancellation)
        {
            var data = await _context.Users
                .Where(x => x.Id == id.Value)
                .Select(x => new
                {
                    User = x,
                    Person = x.Person,
                    Company = x.Company,
                    BranchCount = (x.Company != null) ? x.Company.Branches.Count() : 0,
                    ActiveOffersCount = (x.Company != null) ?
                        x.Company.Branches
                            .SelectMany(branch => branch.BranchOffers)
                            .Where(offer => offer.PublishStart <= _now && offer.PublishEnd >= _now)
                            .Count()
                        : 0,
                    // Liczba aktywnych ofert
                    ActiveOffersCharacteristicIds = (x.Company != null) ?
                        x.Company.Branches
                            .SelectMany(branch => branch.BranchOffers)
                            .Where(offer => offer.PublishStart <= _now && offer.PublishEnd >= _now)
                            .SelectMany(offer => offer.Offer.OfferCharacteristics)
                            .Select(characteristic => characteristic.CharacteristicId)
                            .Distinct()
                        : Enumerable.Empty<int>()
                    // Unikalne ID charakterystyk, jeśli Company == null, zwraca pustą kolekcję
                })
                .FirstOrDefaultAsync(cancellation);

            //Imposible but 
            if (data == null)
            {
                throw new UserException
                    (
                    Messages.User_Ids_NotFound,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }

            //Map
            var domainUser = _userMapper.DomainUser(data.User);
            if (data.Company != null)
            {
                domainUser.Company = _companyMapper.DomainCompany(data.Company);
            }
            if (data.Person != null)
            {
                domainUser.Person = await _personMapper.DomainPerson(data.Person, cancellation);
            }

            return (domainUser, data.BranchCount, data.ActiveOffersCount, data.ActiveOffersCharacteristicIds);
        }

        public async Task<(int TotalCount, IEnumerable<DomainUrl> Items)> GetUrlsAsync
            (
            UserId id,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            //Adapt data 
            itemsCount = (itemsCount < 10) ? 10 : itemsCount;
            itemsCount = (itemsCount > 100) ? 100 : itemsCount;
            page = (page < 1) ? 1 : page;
            orderBy = orderBy.Trim().ToLower();


            //Prepare Query
            var query = _context.Urls.AsQueryable();
            query = query.Where(x => x.UserId == id.Value);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.Name != null && searchTerms.Any(st => x.Name.Contains(st))) ||
                     searchTerms.Any(st => x.Path.Contains(st))
                     );
            }

            switch (orderBy)
            {
                case "typeId":
                    query = ascending ?
                        query.OrderBy(x => x.UrlTypeId).ThenBy(x => x.Created) :
                    query.OrderByDescending(x => x.UrlTypeId).ThenByDescending(x => x.Created);
                    break;
                case "path":
                    query = ascending ?
                        query.OrderBy(x => x.Path).ThenBy(x => x.Created) :
                    query.OrderByDescending(x => x.Path).ThenByDescending(x => x.Created);
                    break;
                default:
                    query = ascending ?
                        query.OrderBy(x => x.Created) :
                    query.OrderByDescending(x => x.Created);
                    break;
            };

            //Databse Select
            var totalCount = await query.CountAsync(cancellation);
            var items = await query
                .Skip((page - 1) * itemsCount)
                .Take(itemsCount)
                .ToListAsync(cancellation);


            return (
                totalCount,
                items.Select(x => _userMapper.DomainUrl(x))
            );
        }


        public async Task<(int TotalCount, IEnumerable<DomainNotification> Items)> GetNotificationsAsync
            (
            UserId id,
            CancellationToken cancellation,
            string? searchText = null,
            bool? hasReaded = null,
            int? senderId = null,
            int? statusId = null,
            DateTime? createdStart = null,
            DateTime? createdEnd = null,
            DateTime? completedStart = null,
            DateTime? completedEnd = null,
            string orderBy = "created", //completed
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var databaseBoolTrue = new DatabaseBool(true).Code;
            var databaseBoolFalse = new DatabaseBool(false).Code;

            //Adapt data 
            itemsCount = (itemsCount < 10) ? 10 : itemsCount;
            itemsCount = (itemsCount > 100) ? 100 : itemsCount;
            page = (page < 1) ? 1 : page;
            orderBy = orderBy.Trim().ToLower();
            //Swap If Time is invalid
            if (createdStart.HasValue && createdEnd.HasValue && createdStart > createdEnd)
            {
                var start = createdStart.Value;
                createdStart = createdEnd.Value;
                createdEnd = start;
            }
            if (completedStart.HasValue && completedEnd.HasValue && completedStart > completedEnd)
            {
                var start = completedStart.Value;
                completedStart = completedEnd.Value;
                completedEnd = start;
            }


            //Prepare Query
            var query = _context.Notifications.AsQueryable();
            query = query.Where(x => x.UserId == id.Value);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.UserMessage != null && searchTerms.Any(st => x.UserMessage.Contains(st))) ||
                     (x.Response != null && searchTerms.Any(st => x.Response.Contains(st)))
                     );
            }

            if (hasReaded.HasValue)
            {
                query = hasReaded.Value ?
                    query.Where(x => x.IsReadedByUser == databaseBoolTrue) :
                    query.Where(x => x.IsReadedByUser == databaseBoolFalse);
            }
            if (senderId.HasValue)
            {
                query = query.Where(x => x.NotificationSenderId == senderId.Value);
            }
            if (statusId.HasValue)
            {
                query = query.Where(x => x.NotificationStatusId == statusId.Value);
            }
            //Time Vlaidation
            if (createdStart.HasValue)
            {
                query = query.Where(x => x.Created >= createdStart.Value);
            }
            if (createdEnd.HasValue)
            {
                query = query.Where(x => x.Created <= createdEnd.Value);
            }
            if (completedStart.HasValue)
            {
                query = query.Where(x => x.Completed >= completedStart.Value);
            }
            if (completedEnd.HasValue)
            {
                query = query.Where(x => x.Completed <= completedEnd.Value);
            }

            switch (orderBy)
            {
                case "completed":
                    query = ascending ?
                        query.OrderBy(x => x.Completed) :
                        query.OrderByDescending(x => x.Completed);
                    break;
                default:
                    //Created
                    query = ascending ?
                        query.OrderBy(x => x.Created) :
                        query.OrderByDescending(x => x.Created);
                    break;
            }

            //Prepare Query
            var totalCount = await query.CountAsync(cancellation);
            var items = await query
                .Skip((page - 1) * itemsCount)
                .Take(itemsCount)
                .ToListAsync(cancellation);


            return
                (
                totalCount,
                items.Select(x => _userMapper.DomainNotification(x))
                );
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
    }
}
