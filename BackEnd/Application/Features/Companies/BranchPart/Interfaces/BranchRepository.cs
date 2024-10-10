using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.BranchPart.Interfaces
{
    public class BranchRepository : IBranchRepository
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public BranchRepository
            (
            IDomainFactory domainFactory,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
            _domainFactory = domainFactory;
            _exceptionsRepository = exceptionsRepository;
            _context = context;
        }


        //======================================================================================================
        //======================================================================================================
        //======================================================================================================
        //Public Methods
        //DML
        public async Task CreateAsync
            (
            DomainBranch branch,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseBranch = new Branch
                {
                    CompanyId = branch.CompanyId.Value,
                    AddressId = branch.AddressId.Value,
                    Id = branch.Id.Value,
                    UrlSegment = branch.UrlSegment == null ?
                    null : branch.UrlSegment.Value,
                    Name = branch.Name,
                    Description = branch.Description

                };
                await _context.Branches.AddAsync(databaseBranch, cancellation);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateAsync
            (
            DomainBranch branch,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseBranch = await GetDatabaseBranchAsync(branch.Id, branch.CompanyId, cancellation);

                databaseBranch.AddressId = branch.AddressId.Value;
                databaseBranch.UrlSegment = branch.UrlSegment == null
                    ? null : branch.UrlSegment.Value;
                databaseBranch.Name = branch.Name;
                databaseBranch.Description = branch.Description;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        //DQL 
        public async Task<DomainBranch> GetBranchAsync
            (
            BranchId id,
            UserId companyId,
            CancellationToken cancellation
            )
        {
            var databaseBranch = await GetDatabaseBranchAsync(id, companyId, cancellation);
            return ConvertToDomainBranch(databaseBranch);
        }
        //======================================================================================================
        //======================================================================================================
        //======================================================================================================
        //Private Methods
        private async Task<Branch> GetDatabaseBranchAsync
            (
            BranchId id,
            UserId companyId,
            CancellationToken cancellation
            )
        {
            var databaseBranch = await _context.Branches
                .Where(x =>
                x.Id == id.Value &&
                x.CompanyId == companyId.Value
                ).FirstOrDefaultAsync(cancellation);

            if (databaseBranch == null)
            {
                throw new BranchException
                    (
                    Messages.NotFoundBranch,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseBranch;
        }

        private DomainBranch ConvertToDomainBranch(Branch branch)
        {
            return _domainFactory.CreateDomainBranch
                (
                branch.Id,
                branch.CompanyId,
                branch.AddressId,
                branch.UrlSegment,
                branch.Name,
                branch.Description
                );
        }

    }
}
