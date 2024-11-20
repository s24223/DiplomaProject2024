using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Addresses.Queries.Interfaces;
using Application.Features.Companies.Mappers;
using Application.Shared.Interfaces.SqlClient;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Queries.QueriesUser.Interfaces
{
    public class UserCompanyRepo : IUserCompanyRepo
    {
        //Values
        private readonly IProvider _provider;
        private readonly ICompanyMapper _companyMapper;
        private readonly ISqlClientRepo _sqlClientRepo;
        private readonly IAddressQueryRepo _addressQueryRepo;
        private readonly DiplomaProjectContext _context;

        private readonly string databaseBoolTrue = new DatabaseBool(true).Code;
        private readonly string databaseBoolFalse = new DatabaseBool(false).Code;


        //Constructor 
        public UserCompanyRepo
            (
            IProvider provider,
            ICompanyMapper companyMapper,
            ISqlClientRepo sqlClientRepo,
            IAddressQueryRepo addressQueryRepo,
            DiplomaProjectContext context
            )
        {
            _provider = provider;
            _companyMapper = companyMapper;
            _sqlClientRepo = sqlClientRepo;
            _addressQueryRepo = addressQueryRepo;
            _context = context;
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
            //Adapt data 
            AdaptQueryParameters(ref itemsCount, ref page);

            //Get Prepared by Database
            var data = await _sqlClientRepo.GetBranchIdsSorted
                (
                companyId.Value,
                divisionId,
                streetId,
                itemsCount,
                page,
                ascending,
                cancellation
                );


            var databseBranches = await _context.Branches
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)
                .Where(x => data.Ids.Any(id => x.Id == id))
                .ToListAsync(cancellation);


            var addressIds = databseBranches
                .Select(x => new AddressId(x.AddressId))
                .ToHashSet();
            var doaminAddresses = await _addressQueryRepo
                .GetAddressDictionaryAsync(addressIds, cancellation);


            var domainBranches = new List<DomainBranch>();
            foreach (var branch in databseBranches)
            {
                var addressId = new AddressId(branch.AddressId);
                var doaminAddress = doaminAddresses[addressId];

                var domainBranch = _companyMapper.DomainBranch(branch);
                domainBranch.Address = doaminAddress;
                domainBranches.Add(domainBranch);
            }

            return (data.TotalCount, domainBranches);
        }


        public async Task<(int TotalCount, IEnumerable<DomainOffer> Items)> GetCoreOffersAsync
            (
            UserId comapnyId,
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
            AdaptQueryParameters(ref minSalary, ref maxSalary, ref orderBy, ref itemsCount, ref page);
            var query = PrepareQueryOffersCore
                (
                comapnyId,
                characteristics,
                searchText,
                isNegotiatedSalary,
                isForStudents,
                minSalary,
                maxSalary,
                orderBy,
                ascending,
                itemsCount,
                page
                );

            //Database Request
            var totalCount = await query.AsNoTracking().CountAsync(cancellation);
            var items = await query
                .Skip((page - 1) * itemsCount)
                .Take(itemsCount)
                .AsNoTracking()
                .ToListAsync(cancellation);

            //Map
            return (totalCount, items.Select(x => _companyMapper.DomainOffer(x)));
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
        private void AdaptQueryParameters
            (
            ref decimal? minSalary,
            ref decimal? maxSalary,
            ref string orderBy,
            ref int itemsCount,
            ref int page
            )
        {
            AdaptQueryParameters(ref orderBy, ref itemsCount, ref page);
            minSalary = (minSalary <= 0) ? null : minSalary;
            maxSalary = (
                (maxSalary <= 0) ||
                (minSalary.HasValue && maxSalary.HasValue && maxSalary < minSalary)
                ) ? null : maxSalary;
        }

        private void AdaptQueryParameters
            (
            ref string orderBy,
            ref int itemsCount,
            ref int page
            )
        {
            AdaptQueryParameters(ref itemsCount, ref page);
            orderBy = orderBy.Trim().ToLower();
        }

        private void AdaptQueryParameters
            (
            ref int itemsCount,
            ref int page
            )
        {
            itemsCount = (itemsCount < 10) ? 10 : itemsCount;
            itemsCount = (itemsCount > 100) ? 100 : itemsCount;
            page = (page < 1) ? 1 : page;
        }

        private IQueryable<Offer> PrepareQueryOffersCore
            (
            UserId comapnyId,
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

            //Qwery Part
            var query = _context.Offers
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Branch)
                .Include(x => x.OfferCharacteristics)
                .AsQueryable();
            query = query
                .Where(x => x.BranchOffers
                    .OrderBy(x => x.Created)
                    .First().Branch.CompanyId == comapnyId.Value
            );


            if (characteristics.Any())
            {
                query = query.Where(x => x.OfferCharacteristics
                    .Any(y => characteristics.Contains(y.CharacteristicId)
                ));
            }
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.Name != null && searchTerms.Any(st => x.Name.Contains(st))) ||
                     searchTerms.Any(st => x.Description.Contains(st))
                     );
            }
            if (isNegotiatedSalary == true)
            {
                query = query.Where(x => x.IsNegotiatedSalary == databaseBoolTrue);
            }
            if (isForStudents == true)
            {
                query = query.Where(x => x.IsForStudents == databaseBoolTrue);
            }
            if (minSalary.HasValue)
            {
                query = query.Where(x => x.MinSalary >= minSalary);
            }
            if (maxSalary.HasValue)
            {
                query = query.Where(x => x.MinSalary <= maxSalary);
            }

            ///IF CHANGE HERE OBLIGATORY CHANGE IN 
            ///"IOrderByService"
            switch (orderBy)
            {
                case "characteristics":
                    if (characteristics.Any())
                    {
                        query = ascending ?
                            query.OrderBy(x => x.OfferCharacteristics
                            .Select(x => x.CharacteristicId)
                                .Intersect(characteristics).Count()
                                )
                                .ThenBy(x => x.MinSalary)
                                .ThenBy(x => x.MaxSalary) :
                            query.OrderByDescending(x => x.OfferCharacteristics
                                .Select(x => x.CharacteristicId)
                                .Intersect(characteristics).Count()
                                )
                                .ThenByDescending(x => x.MinSalary)
                                .ThenByDescending(x => x.MaxSalary);
                    }
                    else
                    {
                        query = ascending ?
                            query.OrderBy(x => x.OfferCharacteristics.Count())
                                .ThenBy(x => x.MinSalary)
                                .ThenBy(x => x.MaxSalary) :
                            query.OrderByDescending(x => x.OfferCharacteristics.Count())
                                .ThenByDescending(x => x.MinSalary)
                                .ThenByDescending(x => x.MaxSalary);
                    }
                    break;
                case "minSalary":
                    query = ascending ?
                        query.OrderBy(x => x.MinSalary)
                            .ThenBy(x => x.MaxSalary) :
                        query.OrderByDescending(x => x.MinSalary)
                            .ThenByDescending(x => x.MaxSalary);
                    break;
                case "maxSalary":
                    query = ascending ?
                        query.OrderBy(x => x.MaxSalary)
                            .ThenBy(x => x.MinSalary) :
                        query.OrderByDescending(x => x.MaxSalary)
                            .ThenByDescending(x => x.MinSalary);
                    break;
                default:
                    //created
                    query = ascending ?
                        query.OrderBy(x => x.BranchOffers
                            .OrderBy(x => x.Created).First().Created) :
                        query.OrderByDescending(x => x.BranchOffers
                            .OrderBy(x => x.Created).First().Created);
                    break;
            };
            return query;
        }


    }
}
