using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.BranchPart.Interfaces
{
    public class BranchRepository : IBranchRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public BranchRepository
            (
            IEntityToDomainMapper mapper,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _exceptionsRepository = exceptionsRepository;
            _context = context;
        }


        //======================================================================================================
        //======================================================================================================
        //======================================================================================================
        //Public Methods
        //DML
        public async Task<Guid> CreateAsync
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
                    UrlSegment = branch.UrlSegment == null ?
                    null : branch.UrlSegment.Value,
                    Name = branch.Name,
                    Description = branch.Description
                };
                await _context.Branches.AddAsync(databaseBranch, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return databaseBranch.Id;
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
                var databaseBranch = await GetDatabaseBranchAsync
                    (
                    branch.Id,
                    branch.CompanyId,
                    cancellation
                    );

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
            return _mapper.ToDomainBranch(databaseBranch);
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
                    Messages.Branch_Id_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseBranch;
        }

    }
}
