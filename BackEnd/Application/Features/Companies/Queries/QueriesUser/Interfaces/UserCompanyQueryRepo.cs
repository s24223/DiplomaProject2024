using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Addresses.Queries.Interfaces;
using Application.Features.Companies.ExtensionMethods;
using Application.Features.Companies.Mappers;
using Application.Features.Companies.Queries.QueriesUser.DTOs.CompanyResponse;
using Application.Shared.DTOs.Features.Characteristics.Responses;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.ExtensionMethods;
using Application.Shared.Interfaces.SqlClient;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.Entities;
using Domain.Features.Characteristic.Repositories;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Queries.QueriesUser.Interfaces
{
    public class UserCompanyQueryRepo : IUserCompanyQueryRepo
    {
        //Values
        private readonly IProvider _provider;
        private readonly ICompanyMapper _companyMapper;
        private readonly ISqlClientRepo _sqlClientRepo;
        private readonly IAddressQueryRepo _addressQueryRepo;
        private readonly ICharacteristicQueryRepository _charRepo;
        private readonly DiplomaProjectContext _context;

        private readonly DateTime _now;
        private readonly string _dbTrue = new DatabaseBool(true).Code;
        private readonly string _dbFalse = new DatabaseBool(false).Code;

        //Constructor 
        public UserCompanyQueryRepo
            (
            IProvider provider,
            ICompanyMapper companyMapper,
            ISqlClientRepo sqlClientRepo,
            IAddressQueryRepo addressQueryRepo,
            ICharacteristicQueryRepository charRepository,
            DiplomaProjectContext context
            )
        {
            _provider = provider;
            _companyMapper = companyMapper;
            _sqlClientRepo = sqlClientRepo;
            _addressQueryRepo = addressQueryRepo;
            _context = context;
            _charRepo = charRepository;
            _now = _provider.TimeProvider().GetDateTimeNow();
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        public async Task<(int TotalCount, IEnumerable<DomainBranch> Items)> GetCoreBranchesAsync
            (
            UserId companyId,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {

            var query = PrepareQueryBranchCore(companyId, divisionId, streetId, ascending);
            var totalCount = await query.CountAsync(cancellation);
            var databaseBranches = await query
                .Pagination(itemsCount, page)
                .AsNoTracking()
                .ToListAsync(cancellation);

            var addressIds = databaseBranches
                .Select(x => new AddressId(x.AddressId))
                .ToHashSet();
            var domainAddresses = await _addressQueryRepo.GetAddressDictionaryAsync(addressIds, cancellation);

            var domainBranches = new List<DomainBranch>();
            foreach (var branch in databaseBranches)
            {
                var addressId = new AddressId(branch.AddressId);
                var domainAddress = domainAddresses[addressId];

                var domainBranch = _companyMapper.DomainBranch(branch);
                domainBranch.Address = domainAddress;
                domainBranches.Add(domainBranch);
            }

            return (totalCount, domainBranches);
        }

        public async Task<(int TotalCount, IEnumerable<DomainOffer> Items)> GetCoreOffersAsync
            (
            UserId companyId,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            //Adapt data 
            var query = PrepareQueryOffersCore(companyId, characteristics, searchText,
                isNegotiatedSalary, isForStudents, minSalary, maxSalary, orderBy,
                ascending, itemsCount, page);

            var items = await query
                .Pagination(itemsCount, page)
                .AsNoTracking()
                .ToListAsync(cancellation);
            var totalCount = await query.CountAsync(cancellation);
            return (totalCount, items.Select(x => _companyMapper.DomainOffer(x)));
        }


        public async Task<CompanyWithDetailsResp> GetCompanyAsync(
            UserId companyId,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascendingBranch = true,
            int itemsCountBranch = 100,
            int pageBranch = 1,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderByOffer = "created",
            bool ascendingOffer = true,
            int itemsCountOffer = 100,
            int pageOffer = 1
            )
        {
            var branchesQuery = PrepareBranchesQuery(companyId);
            var branchOffersQuery = PrepareBranchOffersQuery(companyId);
            var recruitmentQuery = PrepareRecruitmentQuery(companyId);
            var internshipsQuery = PrepareInternshipsQuery(companyId);
            var branchOffersForCharacteristicsQuery =
                PrepareBranchOffersWithCharacteristicsQuery(companyId);
            var queryOffer = PrepareQueryOffersCore(companyId, characteristics, searchText, isNegotiatedSalary,
                isForStudents, minSalary, maxSalary, orderByOffer, ascendingOffer);

            var queryResult = await _context.Companies
                .Where(x => x.UserId == companyId.Value)
                .Select(x => new
                {
                    Item = x,
                    BranchesCount = branchesQuery.Count(),
                    OffersCount = branchOffersQuery
                        .GroupBy(x => x.OfferId)
                        .Count(),
                    BranchOfferPastCount = branchOffersQuery
                        .Where(x =>
                            x.PublishStart < _now &&
                            (x.PublishEnd != null && x.PublishEnd < _now) &&
                            (x.PublishStart != x.PublishEnd))
                        .Count(),
                    BranchOfferActiveCount = branchOffersQuery
                        .Where(x =>
                            x.PublishStart <= _now && (
                                x.PublishEnd == null ||
                                (x.PublishEnd != null && x.PublishEnd > _now)))
                        .Count(),
                    BranchOfferFutureCount = branchOffersQuery
                        .Where(x => x.PublishStart > _now)
                        .Count(),
                    RcruitmentAcceptedCount = recruitmentQuery
                        .Where(x => x.IsAccepted == _dbTrue)
                        .Count(),
                    RcruitmentDeniedCount = recruitmentQuery
                        .Where(x => x.IsAccepted == _dbFalse)
                        .Count(),
                    RcruitmentWaitingCount = recruitmentQuery
                        .Where(x => x.IsAccepted == null)
                        .Count(),
                    InternshipCount = internshipsQuery.Count(),
                    CharacteristicIds = branchOffersForCharacteristicsQuery
                        .SelectMany(x => x.Offer.OfferCharacteristics)
                        .Select(x => x.CharacteristicId)
                        .ToList(),
                    Branches = branchesQuery
                        .BranchFilter(divisionId, streetId)
                        .BranchOrderBy("", ascendingBranch)
                        .Pagination(itemsCountBranch, pageBranch)
                        .Select(b => new
                        {
                            Item = b,
                            BranchOfferPastCount = branchOffersQuery
                                .Where(x =>
                                    x.PublishStart < _now &&
                                    (x.PublishEnd != null && x.PublishEnd < _now) &&
                                    (x.PublishStart != x.PublishEnd))
                                .Where(x => x.BranchId == b.Id)
                                .Count(),
                            BranchOfferActiveCount = branchOffersQuery
                                .Where(x =>
                                    x.PublishStart <= _now && (
                                        x.PublishEnd == null ||
                                        (x.PublishEnd != null && x.PublishEnd > _now)))
                                .Where(x => x.BranchId == b.Id)
                                .Count(),
                            BranchOfferFutureCount = branchOffersQuery
                                .Where(x => x.PublishStart > _now)
                                .Where(x => x.BranchId == b.Id)
                                .Count(),
                            RcruitmentAcceptedCount = recruitmentQuery
                                .Where(x => x.IsAccepted == _dbTrue)
                                .Where(x => x.BranchOffer.BranchId == b.Id)
                                .Count(),
                            RcruitmentDeniedCount = recruitmentQuery
                                .Where(x => x.IsAccepted == _dbFalse)
                                .Where(x => x.BranchOffer.BranchId == b.Id)
                                .Count(),
                            RcruitmentWaitingCount = recruitmentQuery
                                .Where(x => x.IsAccepted == null)
                                .Where(x => x.BranchOffer.BranchId == b.Id)
                                .Count(),
                            InternshipCount = internshipsQuery
                                .Where(x => x.Recruitment.BranchOffer.BranchId == b.Id)
                                .Count(),
                            CharacteristicIds = branchOffersForCharacteristicsQuery
                                .Where(x => x.BranchId == b.Id)
                                .SelectMany(x => x.Offer.OfferCharacteristics)
                                .Select(x => x.CharacteristicId)
                                .ToList(),
                        }).ToList(),
                    Offers = queryOffer
                        .Pagination(itemsCountOffer, pageOffer)
                        .Select(o => new
                        {
                            Item = o,
                            BranchOfferPastCount = branchOffersQuery
                                        .Where(x =>
                                            x.PublishStart < _now &&
                                            (x.PublishEnd != null && x.PublishEnd < _now) &&
                                            (x.PublishStart != x.PublishEnd))
                                        .Where(x => x.OfferId == o.Id)
                                        .Count(),
                            BranchOfferActiveCount = branchOffersQuery
                                        .Where(x =>
                                            x.PublishStart <= _now && (
                                                x.PublishEnd == null ||
                                                (x.PublishEnd != null && x.PublishEnd > _now)))
                                        .Where(x => x.OfferId == o.Id)
                                        .Count(),
                            BranchOfferFutureCount = branchOffersQuery
                                        .Where(x => x.PublishStart > _now)
                                        .Where(x => x.OfferId == o.Id)
                                        .Count(),
                            RcruitmentAcceptedCount = recruitmentQuery
                                        .Where(x => x.IsAccepted == _dbTrue)
                                        .Where(x => x.BranchOffer.OfferId == o.Id)
                                        .Count(),
                            RcruitmentDeniedCount = recruitmentQuery
                                        .Where(x => x.IsAccepted == _dbFalse)
                                        .Where(x => x.BranchOffer.OfferId == o.Id)
                                        .Count(),
                            RcruitmentWaitingCount = recruitmentQuery
                                        .Where(x => x.IsAccepted == null)
                                        .Where(x => x.BranchOffer.OfferId == o.Id)
                                        .Count(),
                            InternshipCount = internshipsQuery
                                        .Where(x => x.Recruitment.BranchOffer.OfferId == o.Id)
                                        .Count(),
                        })
                        .ToList()
                }).FirstOrDefaultAsync(cancellation) ??
                throw new CompanyException(
                    Messages.Company_Cmd_Id_NotFound,
                    Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.NotFound);


            var addressesIds = queryResult.Branches
                .Select(x => new AddressId(x.Item.AddressId));
            var addressesDictionary = await _addressQueryRepo
                .GetAddressDictionaryAsync(addressesIds, cancellation);


            return new CompanyWithDetailsResp
            {
                Company = new CompanyResp(queryResult.Item),
                BranchesTotalCount = queryResult.BranchesCount,
                OffersTotalCount = queryResult.OffersCount,
                BranchOfferPastCount = queryResult.BranchOfferPastCount,
                BranchOfferActiveCount = queryResult.BranchOfferActiveCount,
                BranchOfferFutureCount = queryResult.BranchOfferFutureCount,
                RecruitmentDeniedCount = queryResult.RcruitmentDeniedCount,
                RecruitmentAcceptedCount = queryResult.RcruitmentAcceptedCount,
                RecruitmentWaitingCount = queryResult.RcruitmentWaitingCount,
                InternshipCount = queryResult.InternshipCount,
                CompanyCharacteristics = ConvertCharacteristics(queryResult.CharacteristicIds),
                Branches = queryResult.Branches.Select(x => new BranchWithDeatilsToCompanyResp
                {
                    Branch = new BranchResp(
                        x.Item,
                        (x.Item.AddressId == null ? null : addressesDictionary[new AddressId(x.Item.AddressId)])
                        ),
                    BranchOfferPastCount = x.BranchOfferPastCount,
                    BranchOfferActiveCount = x.BranchOfferActiveCount,
                    BranchOfferFutureCount = x.BranchOfferFutureCount,
                    RecruitmentDeniedCount = x.RcruitmentDeniedCount,
                    RecruitmentAcceptedCount = x.RcruitmentAcceptedCount,
                    RecruitmentWaitingCount = x.RcruitmentWaitingCount,
                    InternshipCount = x.InternshipCount,
                    BranchCharacteristics = ConvertCharacteristics(x.CharacteristicIds),

                }).ToList(),
                Offers = queryResult.Offers.Select(x => new OfferWithDetailsToCompanyResp
                {
                    Offer = new OfferResp(_companyMapper.DomainOffer(x.Item)),
                    BranchOfferPastCount = x.BranchOfferPastCount,
                    BranchOfferActiveCount = x.BranchOfferActiveCount,
                    BranchOfferFutureCount = x.BranchOfferFutureCount,
                    RecruitmentAcceptedCount = x.RcruitmentAcceptedCount,
                    RecruitmentDeniedCount = x.RcruitmentDeniedCount,
                    RecruitmentWaitingCount = x.RcruitmentWaitingCount,
                    InternshipCount = x.InternshipCount,
                }).ToList(),
            };
        }


        public async Task<(IEnumerable<BranchWithDeatilsToCompanyResp> Items, int TotalCount)> GetBranchesWithDetailsAsync(
            UserId companyId,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1)
        {
            var branchesQuery = PrepareBranchesQuery(companyId);
            var branchOffersQuery = PrepareBranchOffersQuery(companyId);
            var recruitmentQuery = PrepareRecruitmentQuery(companyId);
            var internshipsQuery = PrepareInternshipsQuery(companyId);
            var branchOffersForCharacteristicsQuery =
                PrepareBranchOffersWithCharacteristicsQuery(companyId);

            var totalCount = await branchesQuery
                                .BranchFilter(divisionId, streetId)
                                .CountAsync(cancellation);
            var queryResult = await branchesQuery
                        .BranchFilter(divisionId, streetId)
                        .BranchOrderBy("", ascending)
                        .Pagination(itemsCount, page)
                        .Select(b => new
                        {
                            Item = b,
                            BranchOfferPastCount = branchOffersQuery
                                .Where(x =>
                                    x.PublishStart < _now &&
                                    (x.PublishEnd != null && x.PublishEnd < _now) &&
                                    (x.PublishStart != x.PublishEnd))
                                .Where(x => x.BranchId == b.Id)
                                .Count(),
                            BranchOfferActiveCount = branchOffersQuery
                                .Where(x =>
                                    x.PublishStart <= _now && (
                                        x.PublishEnd == null ||
                                        (x.PublishEnd != null && x.PublishEnd > _now)))
                                .Where(x => x.BranchId == b.Id)
                                .Count(),
                            BranchOfferFutureCount = branchOffersQuery
                                .Where(x => x.PublishStart > _now)
                                .Where(x => x.BranchId == b.Id)
                                .Count(),
                            RcruitmentAcceptedCount = recruitmentQuery
                                .Where(x => x.IsAccepted == _dbTrue)
                                .Where(x => x.BranchOffer.BranchId == b.Id)
                                .Count(),
                            RcruitmentDeniedCount = recruitmentQuery
                                .Where(x => x.IsAccepted == _dbFalse)
                                .Where(x => x.BranchOffer.BranchId == b.Id)
                                .Count(),
                            RcruitmentWaitingCount = recruitmentQuery
                                .Where(x => x.IsAccepted == null)
                                .Where(x => x.BranchOffer.BranchId == b.Id)
                                .Count(),
                            InternshipCount = internshipsQuery
                                .Where(x => x.Recruitment.BranchOffer.BranchId == b.Id)
                                .Count(),
                            CharacteristicIds = branchOffersForCharacteristicsQuery
                                .Where(x => x.BranchId == b.Id)
                                .SelectMany(x => x.Offer.OfferCharacteristics)
                                .Select(x => x.CharacteristicId)
                                .ToList(),
                        }).ToListAsync(cancellation);

            var addressesIds = queryResult
                .Select(x => new AddressId(x.Item.AddressId));
            var addressesDictionary = await _addressQueryRepo
                .GetAddressDictionaryAsync(addressesIds, cancellation);

            var items = queryResult.Select(x => new BranchWithDeatilsToCompanyResp
            {
                Branch = new BranchResp(
                        x.Item,
                        (x.Item.AddressId == null ? null : addressesDictionary[new AddressId(x.Item.AddressId)])
                        ),
                BranchOfferPastCount = x.BranchOfferPastCount,
                BranchOfferActiveCount = x.BranchOfferActiveCount,
                BranchOfferFutureCount = x.BranchOfferFutureCount,
                RecruitmentDeniedCount = x.RcruitmentDeniedCount,
                RecruitmentAcceptedCount = x.RcruitmentAcceptedCount,
                RecruitmentWaitingCount = x.RcruitmentWaitingCount,
                InternshipCount = x.InternshipCount,
                BranchCharacteristics = ConvertCharacteristics(x.CharacteristicIds),
            }).ToList();
            return (items, totalCount);
        }


        public async Task<(IEnumerable<OfferWithDetailsToCompanyResp> Items, int TotalCount)> GetOfferWithDetailsAsync(
            UserId companyId,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var query = PrepareQueryOffersCore(companyId, characteristics, searchText, isNegotiatedSalary,
                isForStudents, minSalary, maxSalary, orderBy, ascending);
            var branchOffersQuery = PrepareBranchOffersQuery(companyId);
            var recruitmentQuery = PrepareRecruitmentQuery(companyId);
            var internshipsQuery = PrepareInternshipsQuery(companyId);
            var branchOffersForCharacteristicsQuery =
                PrepareBranchOffersWithCharacteristicsQuery(companyId);

            var totalCount = await query.CountAsync(cancellation);
            var queryResult = await query
                .Pagination(itemsCount, page)
                .Select(o => new
                {
                    Item = o,
                    BranchOfferPastCount = branchOffersQuery
                                .Where(x =>
                                    x.PublishStart < _now &&
                                    (x.PublishEnd != null && x.PublishEnd < _now) &&
                                    (x.PublishStart != x.PublishEnd))
                                .Where(x => x.OfferId == o.Id)
                                .Count(),
                    BranchOfferActiveCount = branchOffersQuery
                                .Where(x =>
                                    x.PublishStart <= _now && (
                                        x.PublishEnd == null ||
                                        (x.PublishEnd != null && x.PublishEnd > _now)))
                                .Where(x => x.OfferId == o.Id)
                                .Count(),
                    BranchOfferFutureCount = branchOffersQuery
                                .Where(x => x.PublishStart > _now)
                                .Where(x => x.OfferId == o.Id)
                                .Count(),
                    RcruitmentAcceptedCount = recruitmentQuery
                                .Where(x => x.IsAccepted == _dbTrue)
                                .Where(x => x.BranchOffer.OfferId == o.Id)
                                .Count(),
                    RcruitmentDeniedCount = recruitmentQuery
                                .Where(x => x.IsAccepted == _dbFalse)
                                .Where(x => x.BranchOffer.OfferId == o.Id)
                                .Count(),
                    RcruitmentWaitingCount = recruitmentQuery
                                .Where(x => x.IsAccepted == null)
                                .Where(x => x.BranchOffer.OfferId == o.Id)
                                .Count(),
                    InternshipCount = internshipsQuery
                                .Where(x => x.Recruitment.BranchOffer.OfferId == o.Id)
                                .Count(),
                })
                .ToListAsync(cancellation);

            var items = queryResult.Select(x => new OfferWithDetailsToCompanyResp
            {
                Offer = new OfferResp(_companyMapper.DomainOffer(x.Item)),
                BranchOfferPastCount = x.BranchOfferPastCount,
                BranchOfferActiveCount = x.BranchOfferActiveCount,
                BranchOfferFutureCount = x.BranchOfferFutureCount,
                RecruitmentAcceptedCount = x.RcruitmentAcceptedCount,
                RecruitmentDeniedCount = x.RcruitmentDeniedCount,
                RecruitmentWaitingCount = x.RcruitmentWaitingCount,
                InternshipCount = x.InternshipCount,
            });
            return (items, totalCount);
        }
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods 
        private IEnumerable<CharAndCharTypeResp> ConvertCharacteristics(IEnumerable<int> ids)
        {
            if (ids is null || !ids.Any())
            {
                return [];
            }
            var domainIds = ids.Select(x => new CharacteristicId(x));
            Console.WriteLine(domainIds.Count());
            var listDomainCharacteristics = _charRepo.GetCharList(domainIds);
            return listDomainCharacteristics.Select(x => new CharAndCharTypeResp(x));
        }


        private IQueryable<Offer> PrepareQueryOffersCore(
            UserId companyId,
            IEnumerable<int> characteristics,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            return _context.Offers
                .Include(x => x.OfferCharacteristics)
                .Include(x => x.BranchOffers)
                    .ThenInclude(x => x.Branch)
                .Where(x =>
                    x.BranchOffers
                    .OrderBy(x => x.Created)
                    .First()
                    .Branch.CompanyId == companyId.Value)
                .OfferFilter(characteristics, searchText, isNegotiatedSalary,
                    isForStudents, minSalary, maxSalary)
                .OfferOrderBy(characteristics, orderBy, ascending);
        }

        private IQueryable<Branch> PrepareQueryBranchCore(
            UserId companyId,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true)
        {
            return _context.Branches
                .Include(b => b.Address)
                    .ThenInclude(b => b.Division)
                .Include(x => x.BranchOffers)
                    .ThenInclude(x => x.Offer)
                    .ThenInclude(x => x.OfferCharacteristics)
                .Where(x => x.CompanyId == companyId.Value)
                .BranchFilter(divisionId, streetId)
                .BranchOrderBy(ascending: ascending);
        }

        private IQueryable<Branch> PrepareBranchesQuery(UserId companyId)
        {
            return _context.Branches
                .Include(x => x.Address)
                .ThenInclude(x => x.Division)
                .Where(x => x.CompanyId == companyId.Value)
            .AsQueryable();
        }

        private IQueryable<BranchOffer> PrepareBranchOffersQuery(UserId companyId)
        {
            return _context.BranchOffers
                .Include(x => x.Branch)
                .Where(x => x.Branch.CompanyId == companyId.Value)
                .AsQueryable();
        }

        private IQueryable<Recruitment> PrepareRecruitmentQuery(UserId companyId)
        {
            return _context.Recruitments
                .Include(x => x.BranchOffer)
                .ThenInclude(x => x.Branch)
                .Where(x => x.BranchOffer.Branch.CompanyId == companyId.Value)
                .AsQueryable();
        }

        private IQueryable<Internship> PrepareInternshipsQuery(UserId companyId)
        {
            return _context.Internships
               .Include(x => x.Recruitment)
               .ThenInclude(x => x.BranchOffer)
               .ThenInclude(x => x.Branch)
               .Where(x => x.Recruitment.BranchOffer.Branch.CompanyId == companyId.Value)
               .AsQueryable();
        }
        private IQueryable<BranchOffer> PrepareBranchOffersWithCharacteristicsQuery(UserId companyId)
        {
            return _context.BranchOffers
                .Include(x => x.Branch)
                .Include(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)
                .Where(x => x.Branch.CompanyId == companyId.Value)
                .Where(x =>
                    x.PublishStart <= _now && (
                        x.PublishEnd == null ||
                        (x.PublishEnd != null && x.PublishEnd > _now)))
                .AsQueryable();
        }

    }
}
