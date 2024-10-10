using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.Offer.Exceptions.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.OfferPart.Interfaces
{
    public class OfferRepository : IOfferRepository
    {
        //Values
        private readonly IProvider _provider;
        private readonly IDomainFactory _domainFactory;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public OfferRepository
            (
            IProvider provider,
            IDomainFactory domainFactory,
            IExceptionsRepository exceptionRepository,
            DiplomaProjectContext context
            )
        {
            _provider = provider;
            _domainFactory = domainFactory;
            _exceptionRepository = exceptionRepository;
            _context = context;
        }


        //==============================================================================================================================================
        //==============================================================================================================================================
        //==============================================================================================================================================
        //Public Methods
        public async Task<Guid> CreateAsync
            (
            UserId companyId,
            DomainOffer offer,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseOffer = new Offer
                {
                    Name = offer.Name,
                    Description = offer.Description,
                    MinSalary = offer.MinSalary is null
                ? null : offer.MinSalary.Value,
                    MaxSalary = offer.MaxSalary is null
                ? null : offer.MaxSalary.Value,
                    IsNegotiatedSalary = offer.IsNegotiatedSalary is null ?
                null : offer.IsNegotiatedSalary.Code,
                    IsForStudents = offer.IsForStudents.Code,
                };
                var databaseBranch = await _context.Branches
                    .Where(x => x.CompanyId == companyId.Value)
                    .FirstOrDefaultAsync(cancellation);

                if (databaseBranch == null)
                {
                    throw new BranchException(Messages.NotExistAnyBranch, DomainExceptionTypeEnum.NotFound);
                }

                var databaseBranchOffer = new BranchOffer
                {
                    Offer = databaseOffer,
                    Branch = databaseBranch,
                    Created = _provider.TimeProvider().GetDateTimeNow(),
                    /*
                    CONSTRAINT Default_BranchOffer_PublishStart DEFAULT GETDATE() FOR [PublishStart],
                    CONSTRAINT Default_BranchOffer_PublishEnd DEFAULT GETDATE() FOR [PublishEnd],
                    CONSTRAINT Default_BranchOffer_LastUpdate DEFAULT GETDATE() FOR [LastUpdate],
                     */
                };

                await _context.Offers.AddAsync(databaseOffer, cancellation);
                await _context.BranchOffers.AddAsync(databaseBranchOffer, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return databaseBranch.Id;
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateAsync
            (
            UserId companyId,
            DomainOffer offer,
            CancellationToken cancellation
            )
        {
            try
            {
                var databseOffer = await GetDatabseOfferAsync(companyId, offer.Id, cancellation);

                databseOffer.Name = offer.Name;
                databseOffer.Description = offer.Description;
                databseOffer.MinSalary = offer.MinSalary is null ?
                    null : offer.MinSalary.Value;
                databseOffer.MaxSalary = offer.MaxSalary is null ?
                    null : offer.MaxSalary.Value;
                databseOffer.IsNegotiatedSalary = offer.IsNegotiatedSalary is null ?
                    null : offer.IsNegotiatedSalary.Code;
                databseOffer.IsForStudents = offer.IsForStudents.Code;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task<DomainOffer> GetOfferAsync
            (
            UserId companyId,
            OfferId id,
            CancellationToken cancellation
            )
        {
            var databseOffer = await GetDatabseOfferAsync(companyId, id, cancellation);
            return ConvertToDomainOffer(databseOffer);
        }
        //==============================================================================================================================================
        //==============================================================================================================================================
        //==============================================================================================================================================
        //Private Methods
        private async Task<Offer> GetDatabseOfferAsync
            (
            UserId companyId,
            OfferId id,
            CancellationToken cancellation
            )
        {
            var databseWayToOffer = await _context.Branches
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Offer)
                .Where(x =>
                x.CompanyId == companyId.Value &&
                x.BranchOffers.Any(x => x.OfferId == id.Value)
                ).FirstOrDefaultAsync(cancellation);

            if (databseWayToOffer == null)
            {
                throw new OfferException(Messages.NotFoundOffer, DomainExceptionTypeEnum.NotFound);
            }

            return databseWayToOffer.BranchOffers.First().Offer;
        }

        private DomainOffer ConvertToDomainOffer(Offer offer)
        {
            return _domainFactory.CreateDomainOffer
                (
                offer.Id,
                offer.Name,
                offer.Description,
                offer.MinSalary,
                offer.MaxSalary,
                offer.IsNegotiatedSalary,
                offer.IsForStudents
                );
        }
    }
}
