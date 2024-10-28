using Application.Databases.Relational;
using Application.Features.Addresses.Interfaces.Queries;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Entities;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Interfaces.QueriesCompany
{
    public class CompanyQueryRepository : ICompanyQueryRepository
    {
        //Values 
        private readonly IAddressQueryRepository _addressQueryRepository;
        private readonly IEntityToDomainMapper _mapper;
        private readonly DiplomaProjectContext _context;
        private readonly string _trueDatabaseBool = new DatabaseBool(true).Code;
        private readonly string _falseDatabaseBool = new DatabaseBool(false).Code;


        //Constructor
        public CompanyQueryRepository
            (
            IAddressQueryRepository addressQueryRepository,
            IEntityToDomainMapper mapper,
            DiplomaProjectContext context
            )
        {
            _addressQueryRepository = addressQueryRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<DomainBranchOffer>> GetOffersAsync
            (
            string orderBy,
            bool ascending,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd = null,
            DateOnly? workStart = null,
            DateOnly? workEnd = null,
            Guid? companyId = null,
            Guid? branchId = null,
            Guid? offerId = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            bool? isNegotietedSalary = null,
            int? divisionId = null,
            int maxItems = 100,
            int page = 1,
            CancellationToken cancellation = default
            )
        {
            var query = _context.BranchOffers
                .Include(x => x.Offer)
                .Include(x => x.Branch)
                .ThenInclude(x => x.Company)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Urls)
                .Where(x => x.Branch.AddressId.HasValue)
                .AsQueryable();

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
            if (minSalary.HasValue)
            {
                query = query.Where(x => x.Offer.MinSalary >= minSalary.Value);
            }
            if (maxSalary.HasValue)
            {
                query = query.Where(x => x.Offer.MaxSalary >= maxSalary.Value);
            }
            if (isNegotietedSalary.HasValue && isNegotietedSalary == true)
            {
                query = query.Where(x => x.Offer.IsNegotiatedSalary == _trueDatabaseBool);
            }

            //Company
            if (companyId.HasValue)
            {
                query = query.Where(x => x.Branch.CompanyId == companyId);
            }
            if (branchId.HasValue)
            {
                query = query.Where(x => x.BranchId == branchId);
            }
            if (offerId.HasValue)
            {
                query = query.Where(x => x.OfferId == offerId);
            }

            //OrderBy - Switch Ewentualnie jesli 1 parametr
            if (orderBy == "publishStart")
            {
                query = (ascending) ?
                    query.OrderBy(x => x.PublishStart) :
                    query.OrderByDescending(x => x.PublishStart);
            }

            if (page == 1)
            {
                query = query.Take(maxItems);
            }
            else
            {
                query = query.Skip((page - 1) * maxItems).Take(page * maxItems);
            }

            var databseList = await query.ToListAsync(cancellation);
            var addresesSet = databseList.Select(x => x.Branch.AddressId)
                .ToHashSet().Select(x => new AddressId(x));

            var addressesDictionary = await _addressQueryRepository
                .GetAddressDictionaryAsync(addresesSet, cancellation);

            var intersectAddressIds = addresesSet
                .Except(addressesDictionary.Keys.ToHashSet())
                .Select(x => x.Value);

            if (intersectAddressIds.Any())
            {
                throw new AddressException(
                    $"{Messages.Address_Id_NotFound}\n{string.Join("\n", intersectAddressIds)}",
                    DomainExceptionTypeEnum.AppProblem
                    );
            }

            var domainList = databseList.Select(x =>
            {
                var bo = _mapper.ToDomainBranchOffer(x);
                bo.Offer = _mapper.ToDomainOffer(x.Offer);
                bo.Branch = _mapper.ToDomainBranch(x.Branch);

                var addressId = new AddressId(x.Branch.AddressId);
                bo.Branch.Address = addressesDictionary[addressId];

                bo.Branch.Company = _mapper.ToDomainCompany(x.Branch.Company);
                bo.Branch.Company.User = _mapper.ToDomainUser(x.Branch.Company.User);
                var domainUrls = x.Branch.Company.User.Urls.Select(u => _mapper.ToDomainUrl(u));
                bo.Branch.Company.User.AddUrls(domainUrls);

                return bo;

            }).ToList();

            return domainList;
            /*
            int? divisionId = null,
             */
        }
    }
}
