using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Company.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Interfaces.CommandsCompanyBranch
{
    public class CompanyRepository : ICompanyRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructors
        public CompanyRepository
            (
            IEntityToDomainMapper mapper,
            IExceptionsRepository exceptionRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _exceptionRepository = exceptionRepository;
            _context = context;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        //DML
        public async Task<Guid> CreateAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            )
        {
            try
            {
                var inputDatabaseCompany = new Company
                {
                    UserId = company.Id.Value,
                    UrlSegment = company.UrlSegment == null ? null : company.UrlSegment.Value,
                    Created = company.Created,
                    ContactEmail = company.ContactEmail.Value,
                    Name = company.Name,
                    Regon = company.Regon.Value,
                };
                await _context.Companies.AddAsync(inputDatabaseCompany, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return company.Id.Value;
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseCompany = await GetDatabseCompanyAsync(company.Id, cancellation);

                databaseCompany.UrlSegment = company.UrlSegment == null ?
                    null : company.UrlSegment.Value;
                databaseCompany.ContactEmail = company.ContactEmail.Value;
                databaseCompany.Name = company.Name;
                databaseCompany.Description = company.Description;

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
    }
}
