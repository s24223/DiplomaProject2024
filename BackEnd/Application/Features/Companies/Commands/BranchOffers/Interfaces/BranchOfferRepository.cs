using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Companies.Mappers;
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

namespace Application.Features.Companies.Commands.BranchOffers.Interfaces
{
    public class BranchOfferRepository : IBranchOfferRepository
    {
        //Values
        private readonly IProvider _provider;
        private readonly ICompanyMapper _mapper;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Constructor
        public BranchOfferRepository
            (
            IProvider provider,
            ICompanyMapper mapper,
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
                    .AsNoTracking()
                    .Where(x => x.CompanyId == companyId.Value)
                    .FirstOrDefaultAsync(cancellation);

                if (databaseBranch == null)
                {
                    throw new BranchException
                        (
                        Messages2.BranchOffer_Authorized_NotExistAnyBranchForCreatingOffer,
                        DomainExceptionTypeEnum.NotFound
                        );
                }

                var databaseOffers = new List<Offer>();
                var databaseBranchOffers = new List<BranchOffer>();
                var databaseOfferCharacteristics = new List<OfferCharacteristic>();

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

                    var databaseBranchOffer = new BranchOffer
                    {
                        Offer = databaseOffer,
                        BranchId = databaseBranch.Id,
                        Created = _provider.TimeProvider().GetDateTimeNow(),
                    };

                    var characteristics = offer.Characteristics.Select(x => new OfferCharacteristic
                    {
                        Offer = databaseOffer,
                        CharacteristicId = x.Value.Item1.Id.Value,
                        QualityId = x.Value.Item2?.Id.Value,
                    });

                    databaseOffers.Add(databaseOffer);
                    databaseBranchOffers.Add(databaseBranchOffer);
                    databaseOfferCharacteristics.AddRange(characteristics);
                }

                await _context.Offers
                    .AddRangeAsync(databaseOffers, cancellation);
                await _context.BranchOffers
                    .AddRangeAsync(databaseBranchOffers, cancellation);
                await _context.OfferCharacteristics
                    .AddRangeAsync(databaseOfferCharacteristics, cancellation);
                await _context.SaveChangesAsync(cancellation);

                //Parse To Domain
                return databaseOffers.Select(x => _mapper.DomainOffer(x));
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }


        public async Task<Dictionary<OfferId, DomainOffer>> UpdateOffersAsync
            (
            UserId companyId,
            Dictionary<OfferId, DomainOffer> offers,
            CancellationToken cancellation
            )
        {
            try
            {
                var offerIds = offers.Keys;
                var databseOfferDictionary =
                    await GetDatabseOfferDictionaryAsync(offerIds, companyId, cancellation);

                foreach (var key in offerIds)
                {
                    var database = databseOfferDictionary[key];
                    var domain = offers[key];

                    database.Name = domain.Name;
                    database.Description = domain.Description;
                    database.MinSalary = domain.MinSalary?.Value;
                    database.MaxSalary = domain.MaxSalary?.Value;
                    database.IsNegotiatedSalary = domain.IsNegotiatedSalary?.Code;
                    database.IsForStudents = domain.IsForStudents.Code;


                    database.OfferCharacteristics.Clear();
                    foreach (var characteristic in domain.Characteristics.Values)
                    {
                        database.OfferCharacteristics.Add(new OfferCharacteristic
                        {
                            Offer = database,
                            CharacteristicId = characteristic.Item1.Id.Value,
                            QualityId = characteristic.Item2?.Id.Value,
                        });
                    }
                }
                await _context.SaveChangesAsync(cancellation);

                return MapOfferDictionary(databseOfferDictionary);
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
            var databseOfferDictionary =
                await GetDatabseOfferDictionaryAsync(ids, companyId, cancellation);
            return MapOfferDictionary(databseOfferDictionary);
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

                var offerDictionary =
                    await GetDatabseOfferDictionaryAsync(offerIds, companyId, cancellation);

                var branchDictionary =
                    await GetDatabaseBranchDictionaryAsync(branchIds, companyId, cancellation);

                var list = new List<BranchOffer>();
                foreach (var bo in branchOffers)
                {
                    var databaseOffer = offerDictionary[bo.OfferId];
                    var databaseBranch = branchDictionary[bo.BranchId];

                    var databaseBranchOffer = new BranchOffer
                    {
                        Offer = databaseOffer,
                        Branch = databaseBranch,
                        //OfferId = databaseOffer.Id,
                        //BranchId = databaseBranch.Id,
                        //Created = bo.Created,
                        PublishStart = bo.PublishStart,
                        PublishEnd = bo.PublishEnd,
                        WorkStart = bo.WorkStart,
                        WorkEnd = bo.WorkEnd,
                        LastUpdate = bo.LastUpdate,
                    };
                    list.Add(databaseBranchOffer);
                }

                await _context.BranchOffers.AddRangeAsync(list, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return list.Select(item =>
                            {
                                var bo = _mapper.DomainBranchOffer(item);
                                bo.Offer = _mapper.DomainOffer(item.Offer);
                                bo.Branch = _mapper.DomainBranch(item.Branch);
                                return bo;
                            });
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task<Dictionary<BranchOfferId, DomainBranchOffer>> UpdateBranchOfferAsync
            (
            UserId companyId,
            Dictionary<BranchOfferId, DomainBranchOffer> dictionary,
            CancellationToken cancellation
            )
        {
            try
            {
                var domainKeysSet = dictionary.Keys.ToHashSet();
                var databaseDictionary =
                    await GetDatabaseBranchOfferDictionaryAsync(domainKeysSet, companyId, cancellation);

                foreach (var key in domainKeysSet)
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

                return MapBranchOfferDictionary(databaseDictionary);
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
            var dictionary = await GetDatabaseBranchOfferDictionaryAsync(ids, companyId, cancellation);
            return MapBranchOfferDictionary(dictionary);
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
            ids = ids.ToHashSet();
            var hashSet = ids.Select(x => x.Value).ToHashSet();

            var dictionary = await _context.BranchOffers
                .Include(x => x.Branch)
                .Include(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)
                .Where(x =>
                    x.Branch.CompanyId == companyId.Value &&
                    hashSet.Contains(x.OfferId)
                )
                .Select(x => x.Offer)
                .Distinct()
                .ToDictionaryAsync
                (
                x => new OfferId(x.Id),
                x => x,
                cancellation
            );

            var missingIds = ids
                .Where(x => !dictionary.ContainsKey(x))
                .Select(x => x.Value);

            if (missingIds.Any())
            {
                throw new OfferException
                    (
                    $"{Messages2.Offer_Ids_NotFound}\n{string.Join("\n", missingIds)}",
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
            ids = ids.ToHashSet();
            var hashSet = ids.Select(x => x.Value);

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

            var missingIds = ids
                .Where(x => !dictionary.ContainsKey(x))
                .Select(x => x.Value);

            if (missingIds.Any())
            {
                throw new BranchException
                    (
                    $"{Messages2.Branch_Id_NotFound}\n{string.Join("\n", missingIds)}",
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
            ids = ids.ToHashSet();
            var hashSet = ids.Select(x => x.Value);

            var dictionary = await _context.BranchOffers
                .Include(x => x.Branch)
                .Include(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)
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

            var missingIds = ids
                .Where(x => !dictionary.ContainsKey(x))
                .Select(x => x.Value);

            if (missingIds.Any())
            {
                throw new BranchOfferException
                    (
                    $"{Messages2.BranchOffer_Ids_NotFound}\n{string.Join("\n", missingIds)}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return dictionary;
        }

        private Dictionary<BranchOfferId, DomainBranchOffer> MapBranchOfferDictionary
            (
            Dictionary<BranchOfferId, BranchOffer> dictionary
            )
        {
            var domainDictionary = new Dictionary<BranchOfferId, DomainBranchOffer>();
            foreach (var kvp in dictionary)
            {
                var bo = _mapper.DomainBranchOffer(kvp.Value);
                bo.Offer = _mapper.DomainOffer(kvp.Value.Offer);
                bo.Branch = _mapper.DomainBranch(kvp.Value.Branch);

                domainDictionary[kvp.Key] = bo;
            }
            return domainDictionary;
        }

        private Dictionary<OfferId, DomainOffer> MapOfferDictionary
            (
            Dictionary<OfferId, Offer> dictionary
            )
        {
            var domainDictionary = new Dictionary<OfferId, DomainOffer>();
            foreach (var kvp in dictionary)
            {
                domainDictionary[kvp.Key] = _mapper.DomainOffer(kvp.Value);
            }
            return domainDictionary;
        }
    }
}
