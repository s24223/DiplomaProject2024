using Application.Databases.Relational;
using Application.Features.Companies.Mappers;
using Application.Features.Persons.Mappers;
using Application.Features.Users.ExtensionMethods;
using Application.Features.Users.Mappers;
using Application.Shared.ExtensionMethods;
using Domain.Features.Notification.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;
using Domain.Features.User.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.QueriesUser.Interfaces
{
    public class UserQueryRepo : IUserQueryRepo
    {
        //Values
        private readonly IProvider _provider;
        private readonly IUserMapper _userMapper;
        private readonly IPersonMapper _personMapper;
        private readonly ICompanyMapper _companyMapper;
        private readonly DiplomaProjectContext _context;
        private readonly DateTime _now;

        //Constructor
        public UserQueryRepo
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
        public async Task<(
            DomainUser User,
            int BranchCount,
            int ActiveOffersCount,
            IEnumerable<int> ActiveOffersCharacteristicIds)>
            GetUserDataAsync(UserId id, CancellationToken cancellation)
        {
            var data = await _context.Users
                .Include(user => user.Person)
                .ThenInclude(user => user.PersonCharacteristics)
                .Where(x => x.Id == id.Value)
                .Select(x => new
                {
                    User = x,
                    Person = x.Person,//Add x.Person.chr ...
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
                    Messages.User_Cmd_Unautorized,
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
            //Prepare Query
            var query = _context.Urls
                .Where(x => x.UserId == id.Value)
                .UrlFilter(searchText)
                .UrlOrderBy(orderBy, ascending);

            //Database Select
            var totalCount = await query.CountAsync(cancellation);
            var items = await query
                .Pagination(itemsCount, page)
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
            //Prepare Query
            var query = _context.Notifications.AsQueryable();
            query = query.Where(x => x.UserId == id.Value)
                .NotificationFilter(searchText, hasReaded, senderId, statusId,
                    createdStart, createdEnd, completedStart, completedEnd)
                .NotificationOrderBy(orderBy, ascending);

            //Prepare Query
            var totalCount = await query.CountAsync(cancellation);
            var items = await query
                .Pagination(itemsCount, page)
                .ToListAsync(cancellation);

            return (
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
