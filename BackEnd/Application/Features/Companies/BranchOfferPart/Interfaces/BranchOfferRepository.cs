using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.Exceptions.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Offer.Entities;
using Domain.Features.Offer.Exceptions.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.BranchOfferPart.Interfaces
{
    public class BranchOfferRepository : IBranchOfferRepository
    {
        //Values
        private readonly IProvider _provider;
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;

        private readonly int _timeMistakeInSeconds = 60;


        //Constructor
        public BranchOfferRepository
            (
            IProvider provider,
            IEntityToDomainMapper mapper,
            IExceptionsRepository exceptionRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _provider = provider;
            _exceptionRepository = exceptionRepository;
            _context = context;
        }


        //==============================================================================================================================================
        //==============================================================================================================================================
        //==============================================================================================================================================
        //Public Methods
        //Offer Part
        public async Task<Guid> CreateOfferAsync
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
                    //Id Defaul value in DB
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
                    throw new BranchException
                        (
                        Messages.BranchOffer_Authorized_NotExistAnyBranchForCreatingOffer,
                        DomainExceptionTypeEnum.NotFound
                        );
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
                return databaseOffer.Id;
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateOfferAsync
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


        //BranchOffer Part
        public async Task CreateBranchOfferAsync
            (
            UserId companyId,
            DomainBranchOffer branchOffer,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseOffer = await GetDatabseOfferAsync
                    (
                    companyId,
                    branchOffer.Id.OfferId,
                    cancellation
                    );
                var databaseBranch = await GetDatabaseBranchAsync
                    (
                    companyId,
                    branchOffer.Id.BranchId,
                    cancellation
                    );

                var databaseBranchOffer = new BranchOffer
                {
                    Offer = databaseOffer,
                    Branch = databaseBranch,
                    Created = branchOffer.Id.Created,
                    WorkStart = branchOffer.WorkStart,
                    WorkEnd = branchOffer.WorkEnd,
                };

                var maxTimeMistake = _provider.TimeProvider()
                    .GetDateTimeNow().AddSeconds(_timeMistakeInSeconds);

                if (branchOffer.PublishStart > maxTimeMistake)
                {
                    databaseBranchOffer.PublishStart = branchOffer.PublishStart;
                }
                if (branchOffer.PublishEnd > maxTimeMistake)
                {
                    databaseBranchOffer.PublishEnd = branchOffer.PublishEnd;
                }

                await _context.BranchOffers.AddAsync(databaseBranchOffer, cancellation);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateBranchOfferAsync
            (
            UserId companyId,
            DomainBranchOffer branchOffer,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseBranchOffer = await GetDatabaseBranchOfferAsync
                    (
                    companyId,
                    branchOffer.Id,
                    cancellation
                    );

                var maxTimeMistake = _provider.TimeProvider()
                    .GetDateTimeNow().AddSeconds(_timeMistakeInSeconds);

                if (branchOffer.PublishStart > maxTimeMistake)
                {
                    databaseBranchOffer.PublishStart = branchOffer.PublishStart;
                }

                databaseBranchOffer.PublishEnd = branchOffer.PublishEnd;

                databaseBranchOffer.WorkStart = branchOffer.WorkStart;
                databaseBranchOffer.WorkEnd = branchOffer.WorkEnd;
                databaseBranchOffer.LastUpdate = branchOffer.LastUpdate;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }


        //DQL
        public async Task<DomainOffer> GetOfferAsync
            (
            UserId companyId,
            OfferId id,
            CancellationToken cancellation
            )
        {
            var databseOffer = await GetDatabseOfferAsync(companyId, id, cancellation);
            return _mapper.ToDomainOffer(databseOffer);
        }

        public async Task<DomainBranchOffer> GetBranchOfferAsync
            (
            UserId companyId,
            BranchOfferId id,
            CancellationToken cancellation
            )
        {
            var databaseBranchOffer = await GetDatabaseBranchOfferAsync(companyId, id, cancellation);
            return _mapper.ToDomainBranchOffer(databaseBranchOffer);
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
                    x.BranchOffers.Any(y => y.OfferId == id.Value)
                ).FirstOrDefaultAsync(cancellation);

            if (databseWayToOffer == null)
            {
                throw new OfferException
                    (
                    Messages.Offer_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databseWayToOffer.BranchOffers.First().Offer;
        }

        private async Task<Branch> GetDatabaseBranchAsync
            (
            UserId companyId,
            BranchId branchId,
            CancellationToken cancellation
            )
        {
            var databaseBranch = await _context.Branches
                .Where(x =>
                    x.Id == branchId.Value &&
                    x.CompanyId == companyId.Value)
                .FirstOrDefaultAsync(cancellation);

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

        private async Task<BranchOffer> GetDatabaseBranchOfferAsync
            (
            UserId companyId,
            BranchOfferId id,
            CancellationToken cancellation
            )
        {
            var databaseBranch = await _context.Branches
                .Include(x => x.BranchOffers)
                .Where(x =>
                    x.CompanyId == companyId.Value &&
                    x.BranchOffers.Any(y =>
                        y.BranchId == id.BranchId.Value &&
                        y.OfferId == id.OfferId.Value &&
                        y.Created == id.Created
                )).FirstOrDefaultAsync(cancellation);

            if (databaseBranch == null)
            {
                throw new BranchOfferException
                    (
                    Messages.BranchOffer_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }

            return databaseBranch.BranchOffers.First();
        }
    }
}
