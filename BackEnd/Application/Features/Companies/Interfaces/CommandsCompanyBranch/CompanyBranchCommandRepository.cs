using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Addresses.Interfaces.Queries;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Company.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Interfaces.CommandsCompanyBranch
{
    public class CompanyBranchCommandRepository : ICompanyBranchCommandRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly IAddressQueryRepository _addressRepository;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructors
        public CompanyBranchCommandRepository
            (
            IEntityToDomainMapper mapper,
            IAddressQueryRepository addressRepository,
            IExceptionsRepository exceptionRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _addressRepository = addressRepository;
            _exceptionRepository = exceptionRepository;
            _context = context;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        //DML
        public async Task<DomainCompany> CreateCompanyAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseCompany = new Company
                {
                    UserId = company.Id.Value,
                    UrlSegment = company.UrlSegment,
                    Created = company.Created,
                    ContactEmail = company.ContactEmail,
                    Name = company.Name,
                    Regon = company.Regon,
                    Description = company.Description,
                };

                var databaseBranches = company.Branches.Values.Select(branch => new Branch
                {
                    CompanyId = databaseCompany.UserId,
                    AddressId = branch.AddressId.Value,
                    UrlSegment = branch.UrlSegment,
                    Name = branch.Name,
                    Description = branch.Description,
                }).ToList();

                await _context.Companies.AddAsync(databaseCompany, cancellation);
                await _context.Branches.AddRangeAsync(databaseBranches, cancellation);
                await _context.SaveChangesAsync(cancellation);

                var dictionaryBranches = await PrepareBranchesAsync(databaseBranches, cancellation);
                company = _mapper.ToDomainCompany(databaseCompany);
                if (dictionaryBranches.Any())
                {
                    company.AddBranches(dictionaryBranches.Values);
                }
                return company;
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateCompanyAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseCompany = await GetDatabseCompanyAsync(company.Id, cancellation);

                databaseCompany.Created = company.Created;
                databaseCompany.Name = company.Name;
                databaseCompany.Regon = company.Regon;
                databaseCompany.Description = company.Description;

                databaseCompany.ContactEmail = company.ContactEmail;
                databaseCompany.UrlSegment = company.UrlSegment;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        //DQL
        public async Task<DomainCompany> GetCompanyAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databaseCompany = await GetDatabseCompanyAsync(id, cancellation);
            return _mapper.ToDomainCompany(databaseCompany);
        }

        //Branch Part
        //DML
        public async Task<IEnumerable<DomainBranch>> CreateBranchesAsync
            (
            IEnumerable<DomainBranch> branches,
            CancellationToken cancellation
            )
        {
            try
            {
                var databseBranches = branches.Select(branch => new Branch
                {
                    CompanyId = branch.CompanyId.Value,
                    AddressId = branch.AddressId.Value,
                    UrlSegment = branch.UrlSegment,
                    Name = branch.Name,
                    Description = branch.Description,
                }).ToList();

                await _context.Branches.AddRangeAsync(databseBranches, cancellation);
                await _context.SaveChangesAsync(cancellation);

                var domainBranchesDictionary = await PrepareBranchesAsync(databseBranches, cancellation);
                return domainBranchesDictionary.Values;
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task<Dictionary<BranchId, DomainBranch>> UpdateBranchesAsync
            (
            Dictionary<BranchId, DomainBranch> branches,
            CancellationToken cancellation
            )
        {
            try
            {
                var companyId = branches.First().Value.CompanyId;
                var databaseBranches = await GetDatabaseBranchesAsync(companyId, branches.Keys, cancellation);
                var intersectKeys = branches.Keys.ToHashSet()
                    .Intersect(databaseBranches.Keys.ToHashSet());

                foreach (var key in intersectKeys)
                {
                    var domain = branches[key];
                    var database = databaseBranches[key];

                    database.AddressId = domain.AddressId.Value;
                    database.UrlSegment = (string?)domain.UrlSegment;
                    database.Name = domain.Name;
                    database.Description = domain.Description;
                }

                await _context.SaveChangesAsync(cancellation);

                return await PrepareBranchesAsync(databaseBranches.Values, cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        //DQL 
        public async Task<Dictionary<BranchId, DomainBranch>> GetBranchesAsync
            (
            UserId companyId,
            IEnumerable<BranchId> ids,
            CancellationToken cancellation
            )
        {
            var databaseBranches = await GetDatabaseBranchesAsync(companyId, ids, cancellation);
            return databaseBranches.ToDictionary
                (
                x => x.Key,
                x => _mapper.ToDomainBranch(x.Value)
                );
        }

        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods        

        private async Task<Company> GetDatabseCompanyAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databaseCompany = await _context.Companies
                .Where(x => x.UserId == id.Value)
                .FirstOrDefaultAsync(cancellation);

            if (databaseCompany == null)
            {
                throw new CompanyException
                    (
                    Messages.Company_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseCompany;
        }

        private async Task<Dictionary<BranchId, Branch>> GetDatabaseBranchesAsync
           (
            UserId companyId,
            IEnumerable<BranchId> ids,
            CancellationToken cancellation
           )
        {
            var idsList = ids.Select(x => x.Value).ToHashSet();
            var databaseBranches = await _context.Branches
                .Where(x =>
                    x.CompanyId == companyId.Value &&
                    idsList.Contains(x.Id)
                ).ToDictionaryAsync
                (
                    x => new BranchId(x.Id),
                    x => x,
                    cancellation
                );

            var misingIds = idsList.Except(databaseBranches.Keys.Select(x => x.Value).ToHashSet());

            if (misingIds.Any())
            {
                throw new BranchException
                    (
                    $"{Messages.Branch_Id_NotFound}\n{string.Join("\n", misingIds)}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseBranches;
        }

        private async Task<Dictionary<BranchId, DomainBranch>> PrepareBranchesAsync
            (
            IEnumerable<Branch> branches,
            CancellationToken cancellation
            )

        {
            var ids = branches
                    .Where(x => x.AddressId.HasValue)
                    .Select(x => new AddressId(x.AddressId))
                    .ToHashSet();
            var addresses = await _addressRepository.GetAddressDictionaryAsync(ids, cancellation);

            var dictionary = new Dictionary<BranchId, DomainBranch>();
            foreach (var databaseBranch in branches)
            {
                if (databaseBranch.AddressId.HasValue)
                {
                    var key = new AddressId(databaseBranch.AddressId.Value);
                    if (addresses.TryGetValue(key, out var address))
                    {
                        var branch = _mapper.ToDomainBranch(databaseBranch);
                        branch.Address = address;
                        dictionary[branch.Id] = branch;
                    }
                }
            }
            return dictionary;
        }
    }
}
