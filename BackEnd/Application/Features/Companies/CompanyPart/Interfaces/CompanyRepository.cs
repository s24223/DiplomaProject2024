using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Company.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.CompanyPart.Interfaces
{
    public class CompanyRepository : ICompanyRepository
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructors
        public CompanyRepository
            (
            IDomainFactory domainFactory,
            IExceptionsRepository exceptionRepository,
            DiplomaProjectContext context
            )
        {
            _domainFactory = domainFactory;
            _exceptionRepository = exceptionRepository;
            _context = context;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        //DML
        public async Task CreateAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            )
        {
            try
            {
                var inputDatabaseCompany = new Database.Models.Company
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
            return ConvertToDomainCompany(databaseCompany);
        }

        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods
        private DomainCompany ConvertToDomainCompany(Company databaseCompany)
        {
            return _domainFactory.CreateDomainCompany
                (
                databaseCompany.UserId,
                databaseCompany.UrlSegment,
                databaseCompany.ContactEmail,
                databaseCompany.Name,
                databaseCompany.Regon,
                databaseCompany.Description
                );
        }

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
                    Messages.NotFoundCompany,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseCompany;
        }
    }
}
