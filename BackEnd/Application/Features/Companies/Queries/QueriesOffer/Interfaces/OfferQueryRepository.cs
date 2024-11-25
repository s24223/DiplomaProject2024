using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Addresses.Queries.Interfaces;
using Application.Features.Companies.Mappers;
using Application.Features.Users.Mappers;
using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.Offer.Exceptions.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Queries.QueriesOffer.Interfaces
{
    public class OfferQueryRepository : IOfferQueryRepository
    {
        //Values 
        private readonly IProvider _provider;
        private readonly ICompanyMapper _mapper;
        private readonly IUserMapper _entitiesMapper;
        private readonly IAddressQueryRepo _addressQueryRepository;
        private readonly DiplomaProjectContext _context;

        private readonly string _trueDatabaseBool = new DatabaseBool(true).Code;
        private readonly string _falseDatabaseBool = new DatabaseBool(false).Code;
        private readonly DateTime _now;


        //Constructor
        public OfferQueryRepository
            (
            IProvider provider,
            ICompanyMapper mapper,
            IUserMapper entitiesMapper,
            IAddressQueryRepo addressQueryRepository,
            DiplomaProjectContext context
            )
        {
            _provider = provider;

            _mapper = mapper;
            _entitiesMapper = entitiesMapper;
            _addressQueryRepository = addressQueryRepository;
            _context = context;

            _now = _provider.TimeProvider().GetDateTimeNow();
        }


        //=======================================================================================================
        //=======================================================================================================
        //=======================================================================================================
        //Public Methods
        public async Task<DomainOffer> GetOfferAsync
            (
            OfferId id,
            CancellationToken cancellation,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            string orderBy,
            bool ascending,
            int maxItems = 100,
            int page = 1
            )
        {
            var query = BuildBasicQuery();
            query = BuildFilters
                (
                query,
                id,
                publishStart,
                publishEnd,
                workStart,
                workEnd
                );
            query = ApplySorting(query, orderBy, ascending);

            //Get Data From Databse
            var branchOffers = await GetDomainBranchOffersAsync
                (
                query,
                maxItems,
                page,
                cancellation
                );

            if (!branchOffers.Any())
            {
                //not found Active Offer by ID
                throw new OfferException
                    (
                    $"{Messages2.Offer_Ids_NotFound}: {id.Value}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }

            //var domainCompany = branchOffers.First().Branch.Company;
            var domainOffer = branchOffers.First().Offer;
            domainOffer.SetBranchOffers(branchOffers);
            return domainOffer;
        }

        public async Task<IEnumerable<DomainBranchOffer>> GetOffersAsync
            (
            UserId? companyId,
            DivisionId? divisionId,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            decimal? salaryMin,
            decimal? salaryMax,
            bool? isNegotietedSalary,
            string orderBy,
            bool ascending,
            int maxItems = 100,
            int page = 1
            )
        {
            var query = BuildBasicQuery();
            query = BuildFilters
                (
                query,
                companyId,
                divisionId,
                characteristics,
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                salaryMin,
                salaryMax,
                isNegotietedSalary
                );
            query = ApplySorting(query, orderBy, ascending);

            //Return Domain List
            return await GetDomainBranchOffersAsync
                (
                query,
                maxItems,
                page,
                cancellation
                );
        }


        public async Task<DomainBranch> GetOffersByBranchAsync
           (
            UserId? companyId,
            string? companyUrlsegment,
            BranchId? branchId,
            string? branchUrlSegment,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            decimal? salaryMin,
            decimal? salaryMax,
            bool? isNegotietedSalary,
            string orderBy,
            bool ascending,
            int maxItems = 100,
            int page = 1
           )
        {
            var query = BuildBasicQuery();
            query = BuildFilters
                (
                query,
                companyId,
                companyUrlsegment,
                branchId,
                branchUrlSegment,
                characteristics,
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                salaryMin,
                salaryMax,
                isNegotietedSalary
                );
            query = ApplySorting(query, orderBy, ascending);

            //Get Data From Databse
            var domainBranchOffers = await GetDomainBranchOffersAsync
                (
                query,
                maxItems,
                page,
                cancellation
                );

            if (!domainBranchOffers.Any())
            {
                //By Paramiters not found any Offers by this Branch
                throw new BranchException
                    (
                    $"{Messages2.BranchOffer_Ids_NotFound} ",
                    DomainExceptionTypeEnum.NotFound
                    );
            }

            //Return Domain Branch
            var domainBranch = domainBranchOffers.First().Branch;
            domainBranch.AddBranchOffers(domainBranchOffers);

            return domainBranch;
        }

        //=======================================================================================================
        //=======================================================================================================
        //=======================================================================================================
        //Pivate Methods
        private DomainBranchOffer CreateDomainBranchOffer
            (
            BranchOffer database,
            DomainAddress domainAddress
            )
        {
            var domain = _mapper.DomainBranchOffer(database);
            domain.Offer = _mapper.DomainOffer(database.Offer);
            domain.Branch = _mapper.DomainBranch(database.Branch);
            domain.Branch.Company = _mapper.DomainCompany(database.Branch.Company);
            domain.Branch.Company.User = _entitiesMapper.DomainUser(database.Branch.Company.User);
            var domainUrls = database.Branch.Company.User.Urls
                .Select(x => _entitiesMapper.DomainUrl(x));
            domain.Branch.Company.User.AddUrls(domainUrls);
            domain.Branch.Address = domainAddress;

            return domain;
        }


        //BranchOffer Part 
        private IQueryable<BranchOffer> BuildBasicQuery()
        {
            var query = _context.BranchOffers
                .Include(x => x.Offer)
                .Include(x => x.Branch)
                    .ThenInclude(x => x.Company)
                        .ThenInclude(x => x.User)
                            .ThenInclude(x => x.Urls)
                .Where(x => x.Branch.AddressId.HasValue)
                .AsQueryable();

            return query.Where
                            (x =>
                            x.PublishStart <= _now &&
                            (x.PublishEnd >= _now || x.PublishEnd == null)
                            );
        }

        private IQueryable<BranchOffer> BuildBasicFilters
            (
            IQueryable<BranchOffer> query,
            IEnumerable<int> characteristics,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            decimal? salaryMin,
            decimal? salaryMax,
            bool? isNegotietedSalary
            )
        {
            //Payed
            if (isPayed)
            {
                query = query.Where(x => x.Offer.MinSalary > 0);
            }

            //Time
            if (publishStart.HasValue)
            {
                query = query.Where(x => x.PublishStart >= publishStart);
            }
            if (publishEnd.HasValue)
            {
                query = query.Where(x => x.PublishEnd >= publishEnd.Value);
            }
            if (workStart.HasValue)
            {
                query = query.Where(x => x.WorkStart >= workStart.Value);
            }
            if (workEnd.HasValue)
            {
                query = query.Where(x => x.WorkEnd >= workEnd.Value);
            }

            //Salary
            if (salaryMin.HasValue)
            {
                query = query.Where(x => x.Offer.MinSalary >= salaryMin.Value);
            }
            if (salaryMax.HasValue)
            {
                query = query.Where(x => x.Offer.MaxSalary <= salaryMax.Value);
            }
            if (isNegotietedSalary.HasValue && isNegotietedSalary == true)
            {
                query = query.Where(x => x.Offer.IsNegotiatedSalary == _trueDatabaseBool);
            }

            return query;
        }

        private IQueryable<BranchOffer> BuildFilters
            (
            IQueryable<BranchOffer> query,
            UserId? companyId,
            DivisionId? divisionId,
            IEnumerable<int> characteristics,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            decimal? salaryMin,
            decimal? salaryMax,
            bool? isNegotietedSalary
            )
        {
            query = BuildBasicFilters
                (
                query,
                characteristics,
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                salaryMin,
                salaryMax,
                isNegotietedSalary
                );

            if (companyId != null)
            {
                query = query.Where(x => x.Branch.CompanyId == companyId.Value);
            }

            //Implement Logic with  divisionId
            return query;
        }

        private IQueryable<BranchOffer> BuildFilters
            (
            IQueryable<BranchOffer> query,
            UserId? companyId,
            string? companyUrlsegment,
            BranchId? branchId,
            string? branchUrlSegment,
            IEnumerable<int> characteristics,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            decimal? salaryMin,
            decimal? salaryMax,
            bool? isNegotietedSalary
            )
        {
            query = BuildBasicFilters
                (
                query,
                characteristics,
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                salaryMin,
                salaryMax,
                isNegotietedSalary
                );

            //Chckin is Id or Urlsegmet exist
            if (companyId != null)
            {
                query = query.Where(x => x.Branch.CompanyId == companyId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(companyUrlsegment))
            {
                query = query.Where(x => x.Branch.Company.UrlSegment == companyUrlsegment);

            }
            else
            {
                //Not Given Id Or Urlsegmet
                throw new CompanyException($"{Messages2.Company_Ids_NotFound}: Not Given Id Or Urlsegmet");
            }

            //Chckin is Id or Urlsegmet exist
            if (branchId != null)
            {
                query = query.Where(x => x.Branch.Id == branchId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(branchUrlSegment))
            {
                query = query.Where(x => x.Branch.UrlSegment == branchUrlSegment);

            }
            else
            {
                //Not Given Id Or Urlsegmet
                throw new
                    ($"{Messages2.BranchOffer_Ids_NotFound}: Not Given Id Or Urlsegmet");
            }
            return query;
        }

        private IQueryable<BranchOffer> BuildFilters
            (
            IQueryable<BranchOffer> query,
            OfferId id,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd
            )
        {
            query = query.Where(x => x.OfferId == id.Value);

            //Time
            if (publishStart.HasValue)
            {
                query = query.Where(x => x.PublishStart >= publishStart);
            }
            if (publishEnd.HasValue)
            {
                query = query.Where(x => x.PublishEnd >= publishEnd.Value);
            }
            if (workStart.HasValue)
            {
                query = query.Where(x => x.WorkStart >= workStart.Value);
            }
            if (workEnd.HasValue)
            {
                query = query.Where(x => x.WorkEnd >= workEnd.Value);
            }
            return query;
        }

        private IQueryable<BranchOffer> ApplySorting
            (
            IQueryable<BranchOffer> query,
            string orderBy,
            bool ascending
            )
        {
            return orderBy switch
            {
                // "publishStart"
                _ => ascending ?
                    query.OrderBy(x => x.PublishStart) :
                    query.OrderByDescending(x => x.PublishStart)
            };
        }

        private async Task<IEnumerable<DomainBranchOffer>> GetDomainBranchOffersAsync
                (
                IQueryable<BranchOffer> query,
                int maxItems,
                int page,
                CancellationToken cancellation
                )
        {
            query = query.Skip((page - 1) * maxItems).Take(maxItems);
            var list = await query.ToListAsync(cancellation);

            //Address Part
            var addresIdsSet = list
                .Select(x => new AddressId(x.Branch.AddressId))
                .ToHashSet();
            var addressesDictionary = await _addressQueryRepository
                .GetAddressDictionaryAsync(addresIdsSet, cancellation);

            return list.Select(x =>
            {
                var addressId = new AddressId(x.Branch.AddressId);
                var domainAddress = addressesDictionary[addressId];
                return CreateDomainBranchOffer(x, domainAddress);
            }).ToList();
        }

    }
}

