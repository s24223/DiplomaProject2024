using Application.Databases.Relational;
using Application.Databases.Relational.Models;
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

namespace Application.Features.Companies.Interfaces.CommandsBranchOffer
{
    public class BranchOfferRepository : IBranchOfferRepository
    {
        //Values
        private readonly IProvider _provider;
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


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
        public async Task<IEnumerable<DomainOffer>> CreateOffersAsync
            (
            UserId companyId,
            IEnumerable<DomainOffer> offers,
            CancellationToken cancellation
            )
        {
            try
            {
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

                var databseOffers = new List<Offer>();
                foreach (var offer in offers)
                {
                    var databaseOffer = new Offer
                    {
                        Name = offer.Name,
                        Description = offer.Description,
                        MinSalary = offer.MinSalary?.Value,
                        MaxSalary = offer.MaxSalary?.Value,
                        IsNegotiatedSalary = offer.IsNegotiatedSalary?.Code,
                        IsForStudents = offer.IsForStudents.Code,
                    };
                    databseOffers.Add(databaseOffer);

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
                    await _context.BranchOffers.AddAsync(databaseBranchOffer, cancellation);
                }

                await _context.Offers.AddRangeAsync(databseOffers, cancellation);
                await _context.SaveChangesAsync(cancellation);

                return databseOffers.Select(x => _mapper.ToDomainOffer(x));
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateOffersAsync
            (
            UserId companyId,
            Dictionary<OfferId, DomainOffer> offers,
            CancellationToken cancellation
            )
        {
            try
            {
                var offerIds = offers.Keys.ToHashSet();
                var databseOfferDictionary = await GetDatabseOfferDictionaryAsync
                    (
                    offerIds,
                    companyId,
                    cancellation
                    );

                foreach (var key in offerIds.Intersect(databseOfferDictionary.Keys))
                {
                    var database = databseOfferDictionary[key];
                    var domain = offers[key];

                    database.Name = domain.Name;
                    database.Description = domain.Description;
                    database.MinSalary = domain.MinSalary?.Value;
                    database.MaxSalary = domain.MaxSalary?.Value;
                    database.IsNegotiatedSalary = domain.IsNegotiatedSalary?.Code;
                    database.IsForStudents = domain.IsForStudents.Code;
                }

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        //DQL
        public async Task<Dictionary<OfferId, DomainOffer>> GetOfferDictionaryAsync
            (
            UserId companyId,
            IEnumerable<OfferId> ids,
            CancellationToken cancellation
            )
        {
            var databseOfferDictionary = await GetDatabseOfferDictionaryAsync
                (
                ids,
                companyId,
                cancellation
                );

            return databseOfferDictionary.ToDictionary
                (
                x => x.Key,
                x => _mapper.ToDomainOffer(x.Value)
                );
        }


        //BranchOffer Part
        //DML
        public async Task<IEnumerable<DomainBranchOffer>> CreateBranchOffersAsync
            (
            UserId companyId,
            IEnumerable<DomainBranchOffer> branchOffers,
            CancellationToken cancellation
            )
        {
            try
            {
                var offerIds = branchOffers.Select(x => x.OfferId).ToHashSet();
                var branchIds = branchOffers.Select(x => x.BranchId).ToHashSet();

                var databaseOffers = await GetDatabseOfferDictionaryAsync(offerIds, companyId, cancellation);
                var databaseBranches = await GetDatabaseBranchDictionaryAsync(branchIds, companyId, cancellation);

                var offerIdsIntersect = offerIds.Intersect(databaseOffers.Keys).ToHashSet();
                var branchIdsIntersect = branchIds.Intersect(databaseBranches.Keys).ToHashSet();

                var correctBranchOffers = branchOffers.Where(x =>
                    offerIdsIntersect.Contains(x.OfferId) &&
                    branchIdsIntersect.Contains(x.BranchId)
                    );

                var list = new List<BranchOffer>();
                foreach (var item in correctBranchOffers)
                {
                    var databaseOffer = databaseOffers[item.OfferId];
                    var databaseBranch = databaseBranches[item.BranchId];

                    var databaseBranchOffer = new BranchOffer
                    {
                        Offer = databaseOffer,
                        Branch = databaseBranch,
                        OfferId = databaseOffer.Id,
                        BranchId = databaseBranch.Id,
                        //Created = item.Created,
                        PublishStart = item.PublishStart,
                        PublishEnd = item.PublishEnd,
                        WorkStart = item.WorkStart,
                        WorkEnd = item.WorkEnd,
                        LastUpdate = item.LastUpdate,
                    };
                    list.Add(databaseBranchOffer);
                }

                await _context.BranchOffers.AddRangeAsync(list, cancellation);
                await _context.SaveChangesAsync(cancellation);

                return list.Select(item =>
                            {
                                var bo = _mapper.ToDomainBranchOffer(item);
                                bo.Offer = _mapper.ToDomainOffer(item.Offer);
                                return bo;
                            });
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateBranchOfferAsync
            (
            UserId companyId,
            Dictionary<BranchOfferId, DomainBranchOffer> dictionary,
            CancellationToken cancellation
            )
        {
            try
            {
                var domainKeysSet = dictionary.Keys.ToHashSet();
                var databaseDictionary = await GetDatabaseBranchOfferDictionaryAsync
                    (
                    domainKeysSet,
                    companyId,
                    cancellation
                    );

                foreach (var key in domainKeysSet.Intersect(databaseDictionary.Keys))
                {
                    var domain = dictionary[key];
                    var database = databaseDictionary[key];

                    database.PublishStart = domain.PublishStart;
                    database.PublishEnd = domain.PublishEnd;

                    database.WorkStart = domain.WorkStart;
                    database.WorkEnd = domain.WorkEnd;

                    database.LastUpdate = domain.LastUpdate;
                }
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }


        //DQL
        public async Task<Dictionary<BranchOfferId, DomainBranchOffer>> GetBranchOfferDictionaryAsync
            (
            UserId companyId,
            IEnumerable<BranchOfferId> ids,
            CancellationToken cancellation
            )
        {
            var dictionary = await GetDatabaseBranchOfferDictionaryAsync
                (
                ids,
                companyId,
                cancellation
                );
            return dictionary.ToDictionary
                (
                x => x.Key,
                x => _mapper.ToDomainBranchOffer(x.Value)
                );
        }

        //==============================================================================================================================================
        //==============================================================================================================================================
        //==============================================================================================================================================
        //Private Methods
        private async Task<Dictionary<OfferId, Offer>> GetDatabseOfferDictionaryAsync
            (
            IEnumerable<OfferId> ids,
            UserId companyId,
            CancellationToken cancellation
            )
        {
            var hashSet = ids.Select(x => x.Value).ToHashSet();
            var dictionary = await _context.BranchOffers
                .Include(x => x.Branch)
                .Include(x => x.Offer)
                .Where(x =>
                    x.Branch.CompanyId == companyId.Value &&
                    hashSet.Contains(x.OfferId)
                )
                .Select(x => x.Offer)
                .ToDictionaryAsync
                (
                x => new OfferId(x.Id),
                x => x,
                cancellation
            );

            var missingIds = hashSet.Except(dictionary.Keys.Select(k => k.Value));

            if (missingIds.Any())
            {
                throw new OfferException
                    (
                    $"{Messages.Offer_Ids_NotFound}\n{string.Join("\n", missingIds)}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return dictionary;
        }

        private async Task<Dictionary<BranchId, Branch>> GetDatabaseBranchDictionaryAsync
            (
            IEnumerable<BranchId> ids,
            UserId companyId,
            CancellationToken cancellation
            )
        {
            var hashSet = ids.Select(x => x.Value).ToHashSet();
            var dictionary = await _context.Branches
                .Where(x =>
                    x.CompanyId == companyId.Value &&
                    hashSet.Contains(x.Id)
                ).ToDictionaryAsync
                (
                x => new BranchId(x.Id),
                x => x,
                cancellation
                );

            var missingIds = hashSet.Except(dictionary.Keys.Select(y => y.Value));

            if (missingIds.Any())
            {
                throw new BranchException
                    (
                    $"{Messages.Branch_Id_NotFound}\n{string.Join("\n", missingIds)}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return dictionary;
        }

        private async Task<Dictionary<BranchOfferId, BranchOffer>> GetDatabaseBranchOfferDictionaryAsync
            (
            IEnumerable<BranchOfferId> ids,
            UserId companyId,
            CancellationToken cancellation
            )
        {
            var hashSet = ids.Select(x => x.Value).ToHashSet();
            var dictionary = await _context.BranchOffers
                .Include(x => x.Branch)
                .Include(x => x.Offer)
                .Where(x =>
                    x.Branch.CompanyId == companyId.Value &&
                    hashSet.Contains(x.Id)
                )
                .ToDictionaryAsync
                (
                x => new BranchOfferId(x.Id),
                x => x,
                cancellation
                );

            var missingIds = hashSet.Except(dictionary.Keys.Select(x => x.Value));

            if (missingIds.Any())
            {
                throw new BranchOfferException
                    (
                    $"{Messages.BranchOffer_Ids_NotFound}\n{string.Join("\n", missingIds)}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return dictionary;
        }
    }
}
