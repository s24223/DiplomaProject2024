using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Companies.Mappers;
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
using System.Text;

namespace Application.Features.Companies.Commands.CompanyBranches.Interfaces
{
    public class CompanyBranchCmdRepo : ICompanyBranchCmdRepo
    {
        //Values
        private readonly ICompanyMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Cosntructors
        public CompanyBranchCmdRepo
            (
            ICompanyMapper mapper,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        public async Task<string?> CheckDbDuplicatesCompanyAsync
            (
            DomainCompany domain,
            bool isCreating,
            CancellationToken cancellation
            )
        {
            //Query Builder
            var query = _context.Companies
                .Where(x =>
                    x.UserId == domain.Id.Value ||
                    (x.Name != null && x.Name == domain.Name) ||
                    (x.Regon != null && x.Regon == domain.Regon.Value) ||
                    (x.ContactEmail != null && x.ContactEmail == domain.ContactEmail.Value)
                    ).AsQueryable();

            if (domain.UrlSegment != null)
            {
                query = _context.Companies
                .Where(x =>
                    x.UserId == domain.Id.Value ||
                    (x.Name != null && x.Name == domain.Name) ||
                    (x.Regon != null && x.Regon == domain.Regon.Value) ||
                    (x.UrlSegment != null && x.UrlSegment == domain.UrlSegment.Value) ||
                    (x.ContactEmail != null && x.ContactEmail == domain.ContactEmail.Value)
                    ).AsQueryable();
            }
            //Get Items from DB
            var dbItems = await query.ToListAsync(cancellation);



            //Creating result
            if (isCreating) // IF is creating And Exist this company
            {
                var thisCompanyItemsCount = dbItems
                    .Where(x => x.UserId == domain.Id.Value)
                    .Count();
                if (thisCompanyItemsCount > 0)
                {
                    return Messages.Company_Cmd_ExistProfile;
                }

            }


            var builder = new StringBuilder();
            //ContactEmail Part
            var duplicateCount = dbItems
                .Where(x =>
                    x.ContactEmail == domain.ContactEmail.Value &&
                    x.UserId != domain.Id.Value
                )
                .Count();
            if (duplicateCount != 0)
            {
                builder.AppendLine(
                    $"{Messages.Company_Cmd_NotUniqueContactEmail}: {domain.ContactEmail.Value}"
                    );
            }
            //Regon Part
            duplicateCount = dbItems
                .Where(x =>
                    x.Regon == domain.Regon.Value &&
                    x.UserId != domain.Id.Value
                )
                .Count();
            if (duplicateCount != 0)
            {
                builder.AppendLine(
                    $"{Messages.Company_Cmd_NotUniqueRegon}: {domain.Regon.Value}"
                    );
            }
            //Name Part
            duplicateCount = dbItems
                .Where(x =>
                    x.Name == domain.Name &&
                    x.UserId != domain.Id.Value
                )
                .Count();
            if (duplicateCount != 0)
            {
                builder.AppendLine(
                    $"{Messages.Company_Cmd_NotUniqueName}: {domain.Name}"
                    );
            }
            if (domain.UrlSegment != null)
            {
                duplicateCount = dbItems
                .Where(x =>
                    x.UrlSegment == domain.UrlSegment.Value &&
                    x.UserId != domain.Id.Value
                )
                .Count();
                if (duplicateCount != 0)
                {
                    builder.AppendLine(
                        $"{Messages.Company_Cmd_NotUniqueUrlSegment}: {domain.UrlSegment.Value}"
                        );
                }
            }

            return builder.Length == 0 ? null : builder.ToString();
        }

        public async Task<(IEnumerable<(DomainBranch Item, bool IsDuplicate)> Items, bool HasDuplicates)>
            CheckDbDuplicatesBranchesCreateAsync
            (
            IEnumerable<DomainBranch> domains,
            CancellationToken cancellation
            )
        {
            //Database Operations
            var companyId = domains.FirstOrDefault()?.CompanyId.Value;
            var databases = await _context.Branches
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.UrlSegment != null
                    )
                .Select(x => x.UrlSegment)
                .ToListAsync(cancellation);
            var urlStringsSet = databases.ToHashSet();

            //Work with Data
            var hasDuplicates = false;
            var result = new List<(DomainBranch Item, bool IsDuplicate)>();
            foreach (var item in domains)
            {
                if (item.UrlSegment != null && urlStringsSet.Contains(item.UrlSegment.Value))
                {
                    result.Add((item, true));
                    if (hasDuplicates != true)
                    {
                        hasDuplicates = true;
                    }
                }
                else
                {
                    result.Add((item, false));
                }
            }
            return (result, hasDuplicates);
        }

        public async Task<(IEnumerable<(DomainBranch Item, bool IsDuplicate)> Items, bool HasDuplicates)>
            CheckDbDuplicatesBranchesUpdateAsync
            (
            Dictionary<BranchId, DomainBranch> dictionary,
            CancellationToken cancellation
            )
        {
            var companyId = dictionary.FirstOrDefault().Value.CompanyId.Value;
            var urlSegmentsAndIds = await _context.Branches
                .Where(x => x.CompanyId == companyId)
                .Select(x => new { x.Id, x.UrlSegment })
                .ToListAsync(cancellation);


            var dbDictionary = urlSegmentsAndIds
                .ToDictionary(x => x.Id, x => x.UrlSegment);
            var inputDictionary = dictionary.Values
                .ToDictionary(x => x.Id.Value, x => x.UrlSegment?.Value);

            var notFoundKeys = inputDictionary
                .Where(x => !dbDictionary.ContainsKey(x.Key))
                .Select(x => x.Key);
            ThrowNotFoundBranchIds(notFoundKeys);

            foreach (var key in inputDictionary.Keys)
            {
                var dbItem = dbDictionary[key];
                var inputItem = inputDictionary[key];

                dbItem = inputItem;
            }

            //Work with Data
            var hasDuplicates = false;
            var result = new List<(DomainBranch Item, bool IsDuplicate)>();
            var groupedItems = dbDictionary.GroupBy(x => x.Value);
            foreach (var pair in groupedItems)
            {
                if (pair.Key == null || pair.Count() == 1)
                {
                    foreach (var id in pair)
                    {
                        var domainId = new BranchId(id.Key);
                        if (dictionary.ContainsKey(domainId))
                        {
                            result.Add((dictionary[domainId], false));
                        }
                    }
                }
                else
                {
                    hasDuplicates = true;
                    foreach (var id in pair)
                    {
                        var domainId = new BranchId(id.Key);
                        if (dictionary.ContainsKey(domainId))
                        {
                            result.Add((dictionary[domainId], true));
                        }
                    }
                }
            }

            return (result, hasDuplicates);
        }

        //DML
        public async Task<DomainCompany> CreateCompanyAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            )
        {
            try
            {
                //Databse Operatons
                var databaseCompany = MapCompany(company, null);
                var databaseBranches = company.Branches.Values.Select(x => MapBranch(x, null));
                await _context.Companies.AddAsync(databaseCompany, cancellation);
                await _context.Branches.AddRangeAsync(databaseBranches, cancellation);
                await _context.SaveChangesAsync(cancellation);
                //Map to Domain
                company = _mapper.DomainCompany(databaseCompany);
                var branchesDictionary = await _mapper.DomainBranchesAsync(databaseBranches, cancellation);
                company.AddBranches(branchesDictionary.Values);
                return company;
            }
            catch (System.Exception ex)
            {
                throw HandleComapanyException(ex, company);
            }
        }

        public async Task<DomainCompany> UpdateCompanyAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            )
        {
            try
            {
                //Databse Operatons
                var databaseCompany = await GetDatabseCompanyAsync(company.Id, cancellation);
                databaseCompany = MapCompany(company, databaseCompany);
                await _context.SaveChangesAsync(cancellation);
                //Map to Domain
                return _mapper.DomainCompany(databaseCompany);
            }
            catch (System.Exception ex)
            {
                throw HandleComapanyException(ex, company);
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
            try
            {
                //Database Operations
                var databaseBranches = branches.Select(x => MapBranch(x, null));
                await _context.Branches.AddRangeAsync(databaseBranches, cancellation);
                await _context.SaveChangesAsync(cancellation);
                //Map to Doamin
                var domainBranchesDictionary = await _mapper.DomainBranchesAsync(databaseBranches, cancellation);
                return domainBranchesDictionary.Values;
            }
            catch (System.Exception ex)
            {
                throw HandleBranchesException(ex);
            }
        }

        public async Task<IEnumerable<DomainBranch>> UpdateBranchesAsync
            (
            Dictionary<BranchId, DomainBranch> branches,
            CancellationToken cancellation
            )
        {
            try
            {
                //Database Operations
                var companyId = branches.First().Value.CompanyId;
                var databaseBranches =
                    await GetDatabaseBranchesAsync(companyId, branches.Keys, cancellation);
                //Update
                foreach (var key in branches.Keys)
                {
                    var domain = branches[key];
                    var database = databaseBranches[key];
                    database = MapBranch(domain, database);
                }
                //Save
                await _context.SaveChangesAsync(cancellation);
                //Map to Doamin
                var domainBranchesDictionary = await _mapper.DomainBranchesAsync(databaseBranches.Values, cancellation);
                return domainBranchesDictionary.Values;
            }
            catch (System.Exception ex)
            {
                throw HandleBranchesException(ex);
            }
        }

        //DQL 
        public async Task<Dictionary<BranchId, DomainBranch>> GetBranchesAsync
            (UserId companyId, IEnumerable<BranchId> ids, CancellationToken cancellation)
        {
            var databaseBranches = await GetDatabaseBranchesAsync(companyId, ids, cancellation);
            return databaseBranches.ToDictionary(x => x.Key, x => _mapper.DomainBranch(x.Value));
        }

        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods      

        //Getters
        private async Task<Company> GetDatabseCompanyAsync
            (UserId id, CancellationToken cancellation)
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
           (UserId companyId, IEnumerable<BranchId> ids, CancellationToken cancellation)
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
            ThrowNotFoundBranchIds(misingIds);
            return databaseBranches;
        }

        private void ThrowNotFoundBranchIds(IEnumerable<Guid> items)
        {
            if (items.Any())
            {
                throw new BranchException
                    (
                    $"{Messages.Branch_Cmd_Id_NotFound}\n{string.Join("\n", items)}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
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

        private Branch MapBranch(DomainBranch domain, Branch? database = null)
        {
            var branch = database ?? new Branch();
            if (database == null)
            {
                branch.CompanyId = domain.CompanyId.Value;
            }
            branch.AddressId = domain.AddressId.Value;
            branch.UrlSegment = (string?)domain.UrlSegment;
            branch.Name = domain.Name;
            branch.Description = domain.Description;
            return branch;
        }


        /*//UrlSegments
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
        }*/

        private System.Exception HandleComapanyException(System.Exception ex, DomainCompany domain)
        {
            //Branch_Address - other
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

        private System.Exception HandleBranchesException(System.Exception ex)
        {
            //2627  Unique, Pk 
            //547   Check, FK
            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;
                if (number == 547)
                {
                    if (sqlEx.Message.Contains("Branch_Address"))
                    {
                        return new BranchException(Messages.Branch_Cmd_Address_NotFound);
                    }
                }
            }
            return ex;
        }

    }
}
