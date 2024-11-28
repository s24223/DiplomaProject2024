using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Addresses.Queries.Interfaces;
using Application.Features.Companies.Mappers;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Company.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application.Features.Companies.Commands.CompanyBranches.Interfaces
{
    public class CompanyBranchCmdRepo : ICompanyBranchCmdRepo
    {
        //Values
        private readonly ICompanyMapper _mapper;
        private readonly IAddressQueryRepo _addressRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructors
        public CompanyBranchCmdRepo
            (
            ICompanyMapper mapper,
            IAddressQueryRepo addressRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _addressRepository = addressRepository;
            _context = context;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        //DML
        public async Task<DomainCompany> CreateCompanyAsync(DomainCompany company, CancellationToken cancellation)
        {
            try
            {
                await ThrowIfCompanyUrlSegmentNotUnique(company, cancellation);

                var databaseCompany = MapCompany(company, null);
                var databaseBranches = MapBranches(company.Branches.Values);

                await _context.Companies.AddAsync(databaseCompany, cancellation);
                await _context.Branches.AddRangeAsync(databaseBranches, cancellation);
                await _context.SaveChangesAsync(cancellation);

                company = _mapper.DomainCompany(databaseCompany);
                if (databaseBranches.Any())
                {
                    var branchesDictionary = await PrepareBranchesAsync(databaseBranches, cancellation);
                    company.AddBranches(branchesDictionary.Values);
                }
                return company;
            }
            catch (System.Exception ex)
            {
                throw HandleCompanyException(ex, company);
            }
        }

        public async Task UpdateCompanyAsync(DomainCompany company, CancellationToken cancellation)
        {
            try
            {
                await ThrowIfCompanyUrlSegmentNotUnique(company, cancellation);

                var databaseCompany = await GetDatabseCompanyAsync(company.Id, cancellation);
                databaseCompany = MapCompany(company, databaseCompany);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw HandleCompanyException(ex, company);
            }
        }

        //DQL
        public async Task<DomainCompany> GetCompanyAsync(UserId id, CancellationToken cancellation)
        {
            var databaseCompany = await GetDatabseCompanyAsync(id, cancellation);
            return _mapper.DomainCompany(databaseCompany);
        }

        //Branch Part
        //DML
        public async Task<IEnumerable<DomainBranch>> CreateBranchesAsync
            (
            IEnumerable<DomainBranch> branches,
            CancellationToken cancellation
            )
        {
            var databseBranches = MapBranches(branches);

            await _context.Branches.AddRangeAsync(databseBranches, cancellation);
            await _context.SaveChangesAsync(cancellation);

            var domainBranchesDictionary = await PrepareBranchesAsync(databseBranches, cancellation);
            return domainBranchesDictionary.Values;
        }

        public async Task<Dictionary<BranchId, DomainBranch>> UpdateBranchesAsync
            (
            Dictionary<BranchId, DomainBranch> branches,
            CancellationToken cancellation
            )
        {
            var companyId = branches.First().Value.CompanyId;
            var databaseBranches =
                await GetDatabaseBranchesAsync(companyId, branches.Keys, cancellation);

            foreach (var key in branches.Keys)
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
                x => _mapper.DomainBranch(x.Value)
                );
        }

        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods        


        //Getters
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
                    Messages.Company_Cmd_Id_NotFound,
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
                    $"{Messages.Branch_Cmd_Id_NotFound}\n{string.Join("\n", misingIds)}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseBranches;
        }


        //Mapers To Database
        private Company MapCompany(DomainCompany domain, Company? database = null)
        {
            var databaseCompany = database ?? new Company();

            databaseCompany.UserId = domain.Id.Value;
            databaseCompany.UrlSegment = (string?)domain.UrlSegment;
            //databaseCompany.Created = domain.Created;
            databaseCompany.ContactEmail = domain.ContactEmail;
            databaseCompany.Name = domain.Name;
            databaseCompany.Regon = domain.Regon;
            databaseCompany.Description = domain.Description;

            return databaseCompany;
        }

        private IEnumerable<Branch> MapBranches(IEnumerable<DomainBranch> domains)
        {
            var list = new List<Branch>();
            foreach (var domainBranch in domains)
            {
                var branch = new Branch();

                branch.CompanyId = domainBranch.CompanyId.Value;
                branch.AddressId = domainBranch.AddressId.Value;
                branch.UrlSegment = (string?)domainBranch.UrlSegment;
                branch.Name = domainBranch.Name;
                branch.Description = domainBranch.Description;

                list.Add(branch);
            }

            return list;
        }


        //UrlSegments
        private async Task ThrowIfCompanyUrlSegmentNotUnique(DomainCompany domain, CancellationToken cancellation)
        {
            if (domain.UrlSegment != null)
            {
                var query = _context.Companies
                .Where(x => domain.UrlSegment != null && x.UrlSegment == domain.UrlSegment.Value)
                .Where(x => x.UserId != domain.Id.Value);


                if (await query.AnyAsync(cancellation))
                {
                    throw new CompanyException($"{Messages.Company_Cmd_NotUniqueUrlSegment}: {domain.UrlSegment.Value}");
                }
            }
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
                        var branch = _mapper.DomainBranch(databaseBranch);
                        branch.Address = address;
                        dictionary[branch.Id] = branch;
                    }
                }
            }
            return dictionary;
        }

        private System.Exception HandleCompanyException(System.Exception ex, DomainCompany domain)
        {
            //Branch_Address - other

            /*
            Company_UNIQUE_ContactEmail
            Company_UNIQUE_Name
            Company_UNIQUE_Regon
            Company_pk
             */
            var dictionary = new Dictionary<string, string>()
            {
                {"Company_UNIQUE_ContactEmail",$"{Messages.Company_Cmd_NotUniqueContactEmail}: {domain.ContactEmail.Value}" },
                {"Company_UNIQUE_Name",$"{Messages.Company_Cmd_NotUniqueName}: {domain.Name}" },
                {"Company_UNIQUE_Regon",$"{Messages.Company_Cmd_NotUniqueRegon}: {domain.Regon.Value}" },
                {"Company_pk",$"{Messages.Company_Cmd_ExistProfile}" },
            };

            //2627  Unique, Pk 
            //547   Check, FK
            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;

                if (number == 2627)
                {
                    foreach (var (constraintName, errorMessage) in dictionary)
                    {
                        if (sqlEx.Message.Contains(constraintName))
                        {
                            return new CompanyException(errorMessage);
                        }
                    }
                }
            }

            return ex;
        }
    }
}
