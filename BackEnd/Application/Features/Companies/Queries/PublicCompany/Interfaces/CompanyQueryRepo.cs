using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Companies.ExtensionMethods;
using Application.Features.Companies.Mappers;
using Application.Features.Companies.Queries.PublicCompany.DTOs;
using Application.Features.Users.ExtensionMethods;
using Application.Features.Users.Mappers;
using Application.Shared.DTOs.Features.Characteristics.Responses;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Features.Users.Urls;
using Application.Shared.ExtensionMethods;
using Application.Shared.Interfaces.SqlClient;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Characteristic.Repositories;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.Company.ValueObjects;
using Domain.Features.Offer.ValueObjects;
using Domain.Features.Url.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Queries.PublicCompany.Interfaces
{
    public class CompanyQueryRepo : ICompanyQueryRepo
    {
        //Values
        private readonly DateTime _now;
        private readonly IProvider _provider;
        private readonly DiplomaProjectContext _context;
        private readonly ICompanyMapper _companyMapper;
        private readonly IUserMapper _userMapper;
        private readonly ISqlClientRepo _sqlClientRepo;
        private readonly ICharacteristicQueryRepository _characteristic;


        //Constructor
        public CompanyQueryRepo(
            DiplomaProjectContext context,
            IProvider provider,
            ICompanyMapper companyMapper,
            IUserMapper userMapper,
            ISqlClientRepo sqlClientRepo,
            ICharacteristicQueryRepository characteristic)
        {
            _context = context;
            _provider = provider;
            _companyMapper = companyMapper;
            _userMapper = userMapper;
            _sqlClientRepo = sqlClientRepo;
            _characteristic = characteristic;

            _now = _provider.TimeProvider().GetDateTimeNow();
        }


        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Public Methods
        public void GetBranchOffersAsync(
           CancellationToken cancellation,
           IEnumerable<int> characteristics,
           UserId? userId = null,
           string? companyName = null,
           Regon? regon = null,
           string? wojewodstwo = null,
           string? divisionName = null,
           string? streetName = null,
           string? searchText = null,
           DateTime? publishFrom = null,
           DateTime? publishTo = null,
           DateTime? workFrom = null,
           DateTime? workTo = null,
           Money? minSalary = null,
           Money? maxSalary = null,
           bool? isForStudents = null,
           bool? isNegotiatedSalary = null,
           string orderBy = "publishStart",
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        { }

        public async Task<(IEnumerable<GetCompanyItemResp> Items, int TotalCount)> GetCompaniesAsync(
           CancellationToken cancellation,
           IEnumerable<int> characteristics,
           string? companyName = null,
           Regon? regon = null,
           string? searchText = null,
           string orderBy = "name", // characteristics
           bool ascending = true,
           bool hasActiveOffers = false,
           int maxItems = 100,
           int page = 1)
        {
            var branchOfferQuery = PrepareBranchOfferQuery();
            var query = PrepareCompanyQuery()
                .Filter(characteristics, companyName, regon, searchText);

            if (hasActiveOffers)
            {
                query = query.Where(c => c.Branches.Any(b => b.BranchOffers.Where(x =>
                            x.PublishStart <= _now &&
                            (x.PublishEnd == null || (x.PublishEnd != null && x.PublishEnd >= _now))
                        ).Any()
                    ));
            }

            var queryResult = await query
                .Pagination(maxItems, page)
                .AsQueryable()
                .Select(c => new
                {
                    Item = c,
                    TotalCount = query
                        .Count(),
                    TotalActiveBranchOffers = branchOfferQuery
                        .Where(x => x.Branch.CompanyId == c.UserId)
                        .Count(),
                    TotalBranches = _context
                        .Branches
                        .Where(x => x.CompanyId == c.UserId)
                        .Count(),
                    TotalOffers = _context.Offers
                        .Include(x => x.BranchOffers)
                        .ThenInclude(x => x.Branch)
                        .Where(x => x.BranchOffers.Any(b => b.Branch.CompanyId == c.UserId))
                        .Count(),
                    CharacteristicIds = branchOfferQuery
                        .Where(x => x.Branch.CompanyId == c.UserId)
                        .SelectMany(x => x.Offer.OfferCharacteristics)
                        .Select(x => x.CharacteristicId)
                        .ToHashSet(),
                })
                .ToListAsync(cancellation);


            var respTtems = new List<GetCompanyItemResp>();
            var totalCount = -1;
            foreach (var item in queryResult)
            {
                if (totalCount < 0)
                {
                    totalCount = item.TotalCount;
                }
                var itemCharacteristics = _characteristic.GetCharList(
                    item.CharacteristicIds.Select(x => new CharacteristicId(x))
                    );

                var respItem = new GetCompanyItemResp
                {
                    Company = new CompanyResp(item.Item),
                    TotalBranches = item.TotalBranches,
                    TotalOffers = item.TotalOffers,
                    TotalActiveBranchOffers = item.TotalActiveBranchOffers,
                    Characteristics = itemCharacteristics.Select(x => new CharResp(x))
                };
                respTtems.Add(respItem);
            }

            totalCount = totalCount < 0 ? 0 : totalCount;
            return (respTtems, totalCount);
        }
        //Company
        public async Task GetCompanyAsync(
            UserId? companyId,
            UrlSegment? companyUrlSegment,
            CancellationToken cancellation,
            IEnumerable<int> characteristics,
            UserId? userId = null,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            Money? minSalary = null,
            Money? maxSalary = null,
            bool? isForStudents = null,
            bool? isNegotiatedSalary = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        { }
        //Company/urls
        /*
                public async Task<(IEnumerable<UrlResp> Items, int TotalCount)> GetUrlsAsync(
                    UserId? companyId,
                    UrlSegment? companyUrlSegment,
                    CancellationToken cancellation,
                    string? searchText = null,
                    string orderBy = "created", //typeId, name
                    bool ascending = true,
                    int maxItems = 100,
                    int page = 1)
                {
                    var companyQuery = PrepareCompanyQuery(companyId, companyUrlSegment);
                    var urlsQuery = _context.Urls
                        .UrlOrderBy(orderBy, ascending)
                        .UrlFilter(searchText)
                        .AsQueryable();

                    var resultQuery = await companyQuery
                        .Select(c => new
                        {
                            Comapny = c,
                            Totalcount = urlsQuery
                                .Where(x => x.UserId == c.UserId)
                                .Count(),
                            Items = urlsQuery
                                .Where(x => x.UserId == c.UserId)
                                //.Pagination(maxItems, page)
                                .Skip(maxItems * (page - 1))
                                .Take(maxItems)
                                .ToArray(),

                        })
                        .FirstOrDefaultAsync(cancellation) ?? throw new CompanyException(
                            Messages.Company_Query_NotFound,
                            DomainExceptionTypeEnum.NotFound);

                    var returnItems = resultQuery.Items.Select(x => new UrlResp(
                        _userMapper.DomainUrl(x)));

                    return (returnItems, resultQuery.Totalcount);
                }
        */

        public async Task<(IEnumerable<UrlResp> Items, int TotalCount)> GetUrlsAsync(
            UserId companyId,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var companyQuery = _context.Companies
                .Where(c => c.UserId == companyId.Value);
            var urlsQuery = _context.Urls
                .Where(url => url.UserId == companyId.Value)
                .UrlOrderBy(orderBy, ascending)
                .UrlFilter(searchText)
                .AsQueryable();

            var resultQuery = await companyQuery
                .Select(c => new
                {
                    Comapny = c,
                    Totalcount = urlsQuery.Count(),
                    Items = urlsQuery
                        .Pagination(maxItems, page)
                        .ToArray(),

                })
                .FirstOrDefaultAsync(cancellation) ?? throw new CompanyException(
                    Messages.Company_Query_NotFound,
                    DomainExceptionTypeEnum.NotFound);

            var returnItems = resultQuery.Items.Select(x => new UrlResp(
                _userMapper.DomainUrl(x)));

            return (returnItems, resultQuery.Totalcount);
        }

        //Company/Offers
        public async Task<(IEnumerable<GetOfferQueryResp> Items, int TotalCount)> GetOffersAsync(
           UserId companyId,
           CancellationToken cancellation,
           IEnumerable<int> characteristics,
           string? searchText = null,
           Money? minSalary = null,
           Money? maxSalary = null,
           bool? isForStudents = null,
           bool? isNegotiatedSalary = null,
           bool? hasActive = null,
           string orderBy = "publishStart",
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        {
            var companyQuery = _context.Companies
                .Where(c => c.UserId == companyId.Value);

            var branchOfferQuery = PrepareBranchOfferQuery();
            var offerTotalQuery = _context.Offers
                .Include(x => x.OfferCharacteristics)
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Branch)
                .Where(o =>
                    o.BranchOffers
                    .OrderBy(bo => bo.Created)
                    .First()
                    .Branch.CompanyId == companyId.Value);

            var offerQuery = offerTotalQuery
                .OfferFilter(characteristics, searchText, isNegotiatedSalary, isForStudents,
                    minSalary, maxSalary)
                .OfferOrderBy(characteristics, orderBy, ascending);
            if (hasActive != null && hasActive.Value)
            {
                offerQuery = offerQuery
                    .Where(o => o.BranchOffers.Any(x =>
                        x.PublishStart <= _now &&
                        (x.PublishEnd == null || (x.PublishEnd != null && x.PublishEnd >= _now)))
                    );
            }

            var resultQuery = await companyQuery
                .Select(c => new
                {
                    Item = c,
                    TotalCount = offerQuery.Count(),
                    Offers = offerQuery
                        .Pagination(maxItems, page)
                        .Select(o => new
                        {
                            Item = o,
                            BranchOffersCount = branchOfferQuery
                                .Where(bo => bo.OfferId == o.Id)
                                .Count()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellation) ?? throw new CompanyException(
                    Messages.Company_Query_NotFound,
                    DomainExceptionTypeEnum.NotFound);

            var resultItems = resultQuery.Offers.Select(x => new GetOfferQueryResp
            {
                Offer = new OfferResp(_companyMapper.DomainOffer(x.Item)),
                ActiveBranchOfferCount = x.BranchOffersCount,
            });

            return (resultItems, resultQuery.TotalCount);
        }


        //Company/Branches
        public async Task<(IEnumerable<GetBranchQueryResp> Items, int TotalCount)> GetBranchesAsync(
            UserId companyId,
            CancellationToken cancellation,
            string? wojewodztwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            string orderBy = "hierarchy",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var branchOfferQuery = PrepareBranchOfferQuery();
            int? wojId = null;
            IEnumerable<int> divisionIds = [];

            if (!string.IsNullOrWhiteSpace(wojewodztwo))
            {
                var procedureResult = await _sqlClientRepo.GetChildDivisionIdsAsync(
                    wojewodztwo, divisionName, cancellation);
                wojId = procedureResult.WojId ?? throw new AddressException(
                    $"{Messages.Address_Query_WojName_NotFound}: {wojewodztwo}",
                    DomainExceptionTypeEnum.NotFound);
                divisionIds = procedureResult.DivisionIds;

                if (!string.IsNullOrWhiteSpace(divisionName) && !divisionIds.Any())
                {
                    throw new AddressException(
                    $"{Messages.Address_Query_DivisionName_NotFound}: {divisionName}",
                    DomainExceptionTypeEnum.NotFound);
                }

            }

            var companyQuery = _context.Companies
                .Where(c => c.UserId == companyId.Value);
            var branchesQuery = _context.Branches
                .Include(x => x.Address)
                .ThenInclude(x => x.Division)
                .Where(b => b.CompanyId == companyId.Value);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                branchesQuery = branchesQuery.Where(x =>
                     (x.Name != null && searchTerms.Any(st => x.Name.Contains(st))) ||
                     (
                        x.Description != null &&
                        searchTerms.Any(st => x.Description.Contains(st))
                )
                );
            }
            if (divisionIds.Any())
            {
                branchesQuery = branchesQuery.Where(x => divisionIds.Any(y =>
                    x.Address != null &&
                    x.Address.Division.Id == y
                ));

                if (streetName != null)
                {
                    var searchTerms = streetName
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    branchesQuery = branchesQuery.Where(x =>
                     x.Address != null &&
                     x.Address.Street != null &&
                     searchTerms.Any(st => x.Address.Street.Name.Contains(st))
                     );
                }
            }
            else
            {
                if (wojId.HasValue)
                {
                    branchesQuery = branchesQuery.Where(x =>
                        x.Address != null &&
                        x.Address.Division.PathIds != null &&
                        x.Address.Division.PathIds.Contains($"{wojId.Value}-"));
                }
            }

            branchesQuery = branchesQuery.BranchOrderBy(orderBy, ascending);

            var resultQuery = await companyQuery
                .Select(c => new
                {
                    Item = c,
                    TotalCount = branchesQuery.Count(),
                    Branches = branchesQuery
                        .Pagination(maxItems, page)
                        .Select(b => new
                        {
                            Item = b,
                            ActiveBranchOffers = branchOfferQuery
                                .Where(bo => bo.BranchId == b.Id)
                                .Count(),
                            Charachteristics = branchOfferQuery
                                .Where(bo => bo.BranchId == b.Id)
                                .SelectMany(bo => bo.Offer.OfferCharacteristics)
                                .Select(oc => oc.CharacteristicId)
                                .ToHashSet()
                        })
                        .ToList(),
                })
                .FirstOrDefaultAsync(cancellation) ?? throw new CompanyException(
                    Messages.Company_Query_NotFound,
                    DomainExceptionTypeEnum.NotFound);

            var branches = resultQuery.Branches.Select(x => x.Item);
            var domainBranches = await _companyMapper.DomainBranchesAsync(
                branches, cancellation);

            var items = resultQuery.Branches.Select(b => new GetBranchQueryResp
            {
                Branch = new BranchResp(domainBranches[new BranchId(b.Item.Id)]),
                BranchOffersCount = b.ActiveBranchOffers,
                Characteristics = _characteristic.GetCharList(
                    b.Charachteristics.Select(x => new CharacteristicId(x)))
                    .Select(x => new CharResp(x)),
            });
            return (items, resultQuery.TotalCount);
        }
        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Private Methods
        private IQueryable<Company> PrepareCompanyQuery()
        {
            return _context.Companies
                .Include(x => x.Branches)
                .ThenInclude(x => x.BranchOffers)
                .ThenInclude(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)
                .AsQueryable();
        }

        private IQueryable<Company> PrepareCompanyQuery(
            UserId? companyId,
            UrlSegment? companyUrlSegment)
        {
            var query = _context.Companies
                .AsQueryable();

            Console.WriteLine(companyId == null);
            Console.WriteLine(companyUrlSegment == null);

            if (companyId != null)
            {
                return query.Where(c => c.UserId == companyId.Value);
            }
            else if (companyUrlSegment != null)
            {
                return query.Where(c => c.UrlSegment == companyUrlSegment.Value);
            }
            else
            {
                throw new UrlException(Messages.Url_Query_CompanyIdOrUrl_Requered,
                    DomainExceptionTypeEnum.NotFound);
            }
        }

        private IQueryable<BranchOffer> PrepareBranchOfferQuery()
        {
            return _context.BranchOffers
                .Include(x => x.Branch)
                .ThenInclude(x => x.Company)
                .Include(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)
                .Where(x =>
                    x.PublishStart <= _now &&
                    (x.PublishEnd == null || (x.PublishEnd != null && x.PublishEnd >= _now)));
        }

    }
}
