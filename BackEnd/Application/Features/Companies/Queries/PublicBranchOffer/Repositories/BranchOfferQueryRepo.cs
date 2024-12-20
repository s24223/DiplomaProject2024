using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Companies.ExtensionMethods;
using Application.Features.Companies.Mappers;
using Application.Features.Companies.Queries.PublicBranchOffer.DTOs;
using Application.Features.Companies.Queries.PublicBranchOffer.DTOs.BranchPart;
using Application.Features.Companies.Queries.PublicBranchOffer.DTOs.OffersPart;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.ExtensionMethods;
using Application.Shared.Interfaces.SqlClient;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Exceptions.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Company.ValueObjects;
using Domain.Features.Offer.ValueObjects;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Queries.PublicBranchOffer.Repositories
{
    public class BranchOfferQueryRepo : IBranchOfferQueryRepo
    {
        //Values
        private readonly DiplomaProjectContext _context;
        private readonly IProvider _provider;
        private readonly ISqlClientRepo _sql;
        private readonly ICompanyMapper _companyMapper;
        private readonly DateTime _now;


        //Constructor
        public BranchOfferQueryRepo(
            DiplomaProjectContext context,
            ICompanyMapper companyMapper,
            ISqlClientRepo sql,
            IProvider provider)
        {
            _sql = sql;
            _context = context;
            _provider = provider;
            _companyMapper = companyMapper;
            _now = _provider.TimeProvider().GetDateTimeNow();
        }


        //===================================================================================
        //===================================================================================
        //===================================================================================
        //Public Methods
        public async Task<(IEnumerable<GetBranchOfferResp> Items, int TotalCount)>
            GetBranchOffersAsync(
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
        {
            var divisionData = await GetChildDivisionIdsAsync(cancellation, wojewodstwo, divisionName);
            var query = PrepareBranchOfferQuery().Filter(characteristics, divisionData.DivisionIds,
                divisionData.WojId, streetName, userId, companyName, regon, searchText, publishFrom,
                publishTo, workFrom, workTo, minSalary, maxSalary, isForStudents, isNegotiatedSalary);


            var totalCount = await query.CountAsync(cancellation);
            var results = await query
                .BranchOfferOrderBy(characteristics, orderBy, ascending)
                .Pagination(maxItems, page)
                .AsNoTracking()
                .ToListAsync(cancellation);


            var branches = results.Select(x => x.Branch);
            var domainBranchesDictionary = await _companyMapper
                .DomainBranchesAsync(branches, cancellation);


            var items = results.Select(x => new GetBranchOfferResp
            {
                BranchOffer = new BranchOfferResp(_companyMapper
                    .DomainBranchOffer(x)),
                Branch = new BranchResp(
                    domainBranchesDictionary[new BranchId(x.BranchId)]),
                Offer = new OfferResp(_companyMapper.DomainOffer(x.Offer)),
                Company = new CompanyResp(_companyMapper
                    .DomainCompany(x.Branch.Company))
            });
            return (items, totalCount);
        }


        public async Task<GetSingleBranchOfferResp> GetBranchOfferAsync(
            BranchOfferId id,
            CancellationToken cancellation,
            UserId? userId = null)
        {
            var result = await PrepareBranchOfferQuery()
                .Where(x => x.Id == id.Value)
                .Select(x => new
                {
                    Item = x,
                    IsRecruited = userId == null ? (bool?)null : _context.Recruitments
                    .Where(r => r.BranchOfferId == x.Id && r.PersonId == userId.Value)
                    .Any(),
                })
                .FirstOrDefaultAsync(cancellation);

            if (result == null)
            {
                throw new BranchOfferException(
                    Messages.BranchOffer_Cmd_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound);
            }

            var branch = await _companyMapper.DomainBranchAsync(result.Item.Branch, cancellation);

            return new GetSingleBranchOfferResp
            {
                BranchOffer = new BranchOfferResp(_companyMapper
                    .DomainBranchOffer(result.Item)),
                Branch = new BranchResp(branch),
                Offer = new OfferResp(_companyMapper.DomainOffer(result.Item.Offer)),
                Company = new CompanyResp(_companyMapper
                    .DomainCompany(result.Item.Branch.Company)),
                IsRecruited = result.IsRecruited,
            };
        }


        public async Task<GetOfferResp> GetOfferAsync(
            OfferId offerId,
            CancellationToken cancellation,
            UserId? userId = null,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var divisionData = await GetChildDivisionIdsAsync(cancellation, wojewodstwo, divisionName);
            var branchOfferQuery = PrepareBranchOfferQuery()
                .Filter([], divisionData.DivisionIds, divisionData.WojId,
                streetName, userId, null, null, searchText, publishFrom, publishTo,
                workFrom, workTo, null, null, null, null)
            .Where(x => x.OfferId == offerId.Value)
            .BranchOfferOrderBy([], orderBy, ascending);


            var result = await _context.Offers
                .Include(x => x.OfferCharacteristics)
                .Where(x => x.Id == offerId.Value)
                .Select(o => new
                {
                    Item = o,
                    TotalCount = branchOfferQuery.Count(),
                    BranchOffers = branchOfferQuery
                        .Pagination(maxItems, page)
                        .ToList(),
                })
                .FirstOrDefaultAsync(cancellation) ?? throw new BranchOfferException(
                    Messages.Offer_Cmd_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound);


            var branches = result.BranchOffers.Select(x => x.Branch);
            var domainBranchesDictionary = await _companyMapper
                .DomainBranchesAsync(branches, cancellation);

            return new GetOfferResp
            {
                Offer = new OfferResp(_companyMapper.DomainOffer(result.Item)),
                TotalCount = result.TotalCount,
                BranchOffers = result.BranchOffers.Select(bo => new GetOfferBranchOfferResp
                {
                    BranchOffer = new BranchOfferResp(_companyMapper
                        .DomainBranchOffer(bo)),
                    Branch = new BranchResp(
                        domainBranchesDictionary[new BranchId(bo.BranchId)]),
                    Company = new CompanyResp(_companyMapper
                        .DomainCompany(bo.Branch.Company))
                })
            };
        }


        public async Task<(IEnumerable<GetOfferBranchOfferResp> Items, int TotalCount)>
            GetOfferBranchOffersAsync(
            OfferId offerId,
            CancellationToken cancellation,
            UserId? userId = null,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var divisionData = await GetChildDivisionIdsAsync(cancellation, wojewodstwo, divisionName);
            var branchOfferQuery = PrepareBranchOfferQuery()
                .Filter([], divisionData.DivisionIds, divisionData.WojId,
                streetName, userId, null, null, searchText, publishFrom, publishTo,
                workFrom, workTo, null, null, null, null)
                .Where(x => x.OfferId == offerId.Value);

            var totalCount = await branchOfferQuery.CountAsync(cancellation);
            var items = await branchOfferQuery
                .BranchOfferOrderBy([], orderBy, ascending)
                .Pagination(maxItems, page)
                .ToListAsync(cancellation);


            var branches = items.Select(x => x.Branch);
            var domainBranchesDictionary = await _companyMapper
                .DomainBranchesAsync(branches, cancellation);

            var resultItems = items.Select(bo => new GetOfferBranchOfferResp
            {
                BranchOffer = new BranchOfferResp(_companyMapper
                        .DomainBranchOffer(bo)),
                Branch = new BranchResp(
                        domainBranchesDictionary[new BranchId(bo.BranchId)]),
                Company = new CompanyResp(_companyMapper
                        .DomainCompany(bo.Branch.Company))
            });

            return (resultItems, totalCount);
        }


        public async Task<GetBranchResp> GetBranchAsync(
               UserId? companyId,
               BranchId? branchId,
               UrlSegment? companyUrlSegment,
               UrlSegment? branchUrlSegment,
               CancellationToken cancellation,
               IEnumerable<int> characteristics,
               UserId? userId = null,
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
        {
            var branchQuery = PrepareBranchQuery(companyId, branchId,
                companyUrlSegment, branchUrlSegment);

            var query = PrepareBranchOfferQuery()
                .Filter(characteristics, [],
                null, null, userId, null, null, searchText, publishFrom,
                publishTo, workFrom, workTo, minSalary, maxSalary, isForStudents, isNegotiatedSalary);

            var result = await branchQuery.Select(b => new
            {
                Item = b,
                TotalCount = query
                    .Where(x => x.BranchId == b.Id)
                    .AsQueryable()
                    .Count(),
                BranchOffers = query
                    .BranchOfferOrderBy(characteristics, orderBy, ascending)
                    .Pagination(maxItems, page)
                    .AsQueryable()
                    .Where(x => x.BranchId == b.Id)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellation) ?? throw new BranchOfferException(
                Messages.Branch_Query_Branch_NotFound,
                DomainExceptionTypeEnum.NotFound);

            var branch = await _companyMapper.DomainBranchAsync(result.Item, cancellation);

            return new GetBranchResp
            {
                Branch = new BranchResp(branch),
                Company = new CompanyResp(
                    _companyMapper.DomainCompany(result.Item.Company)),
                TotalCount = result.TotalCount,
                BranchOffers = result.BranchOffers.Select(x => new GetBranchBranchOfferResp
                {
                    Offer = new OfferResp(_companyMapper.DomainOffer(x.Offer)),
                    BranchOffer = new BranchOfferResp(_companyMapper
                        .DomainBranchOffer(x)),
                })
                .ToList(),
            };
        }

        public async Task<(IEnumerable<GetBranchBranchOfferResp> Items, int TotalCount)>
            GetBranchBranchOffersAsync(
              UserId? companyId,
              BranchId? branchId,
              UrlSegment? companyUrlSegment,
              UrlSegment? branchUrlSegment,
              CancellationToken cancellation,
              IEnumerable<int> characteristics,
              UserId? userId = null,
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
        {
            var branchQuery = PrepareBranchQuery(companyId, branchId,
                companyUrlSegment, branchUrlSegment);

            var query = PrepareBranchOfferQuery().Filter(characteristics, [],
                null, null, userId, null, null, searchText, publishFrom,
                publishTo, workFrom, workTo, minSalary, maxSalary, isForStudents, isNegotiatedSalary);

            var result = await branchQuery.Select(b => new
            {
                Item = b,
                TotalCount = query
                    .Where(x => x.BranchId == b.Id)
                    .AsQueryable()
                    .Count(),
                BranchOffers = query
                    .BranchOfferOrderBy(characteristics, orderBy, ascending)
                    .Pagination(maxItems, page)
                    .AsQueryable()
                    .Where(x => x.BranchId == b.Id)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellation) ?? throw new BranchOfferException(
                Messages.Branch_Query_Branch_NotFound,
                DomainExceptionTypeEnum.NotFound);


            var items = result.BranchOffers.Select(x => new GetBranchBranchOfferResp
            {
                Offer = new OfferResp(_companyMapper.DomainOffer(x.Offer)),
                BranchOffer = new BranchOfferResp(_companyMapper
                        .DomainBranchOffer(x)),
            })
            .ToList();

            return (items, result.TotalCount);
        }
        //Company
        //Company/Urls
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
        //===================================================================================
        //===================================================================================
        //===================================================================================
        //Private Methods
        private IQueryable<BranchOffer> PrepareBranchOfferQuery()
        {
            return _context.BranchOffers
                .Include(x => x.Recruitments)

                .Include(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)

                .Include(x => x.Branch)
                .ThenInclude(x => x.Company)

                .Include(x => x.Branch)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.Division)

                .Include(x => x.Branch)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.Street)

                .Where(x =>
                    x.PublishStart <= _now &&
                    (x.PublishEnd == null ||
                    x.PublishEnd != null && x.PublishEnd >= _now)
                );
        }

        private async Task<(int? WojId, IEnumerable<int> DivisionIds)> GetChildDivisionIdsAsync(
            CancellationToken cancellation,
            string? wojewodstwo = null,
            string? divisionName = null)
        {
            if (string.IsNullOrWhiteSpace(wojewodstwo))
            {
                return (null, []);
            }

            wojewodstwo = wojewodstwo.Trim();
            var divisionsData = await _sql.GetChildDivisionIdsAsync(
                    wojewodstwo, divisionName, cancellation);

            if (!divisionsData.WojId.HasValue)
            {
                throw new AddressException($"{Messages.Address_Query_WojName_NotFound}: {wojewodstwo}",
                    DomainExceptionTypeEnum.NotFound);
            }
            if (!string.IsNullOrWhiteSpace(divisionName) && !divisionsData.DivisionIds.Any())
            {
                throw new AddressException($"{Messages.Address_Query_DivisionName_NotFound}: {divisionName}",
                    DomainExceptionTypeEnum.NotFound);
            }
            return (divisionsData.WojId.Value, divisionsData.DivisionIds);
        }

        private IQueryable<Branch> PrepareBranchQuery(
               UserId? companyId,
               BranchId? branchId,
               UrlSegment? companyUrlSegment,
               UrlSegment? branchUrlSegment)
        {
            IQueryable<Branch> branchQuery = _context.Branches
                .Include(x => x.Address)
                .Include(x => x.Company);

            if (branchId != null)
            {
                branchQuery = branchQuery
                    .Where(x => x.Id == branchId.Value);
            }
            else if (branchUrlSegment != null && companyId != null)
            {
                branchQuery = branchQuery.Where(x =>
                    x.UrlSegment == branchUrlSegment.Value &&
                    x.Company.UserId == companyId.Value
                );
            }
            else if (branchUrlSegment != null && companyUrlSegment != null)
            {
                branchQuery = branchQuery.Where(x =>
                    x.UrlSegment == branchUrlSegment.Value &&
                    x.Company.UrlSegment == companyUrlSegment.Value
                );
            }
            else
            {
                throw new BranchOfferException(Messages.Branch_Query_UrlSegmentBranch_Invalid);
            }

            return branchQuery;
        }
    }
}
