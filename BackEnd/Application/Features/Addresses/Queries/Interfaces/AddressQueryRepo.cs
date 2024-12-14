using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Addresses.Mappers;
using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Features.Addresses.Queries.Interfaces
{
    public class AddressQueryRepo : IAddressQueryRepo
    {
        //Values
        private readonly IAddressMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public AddressQueryRepo
            (
            IAddressMapper mapper,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        //DML
        public async Task<DomainAddress> GetAddressAsync
            (
            AddressId id,
            CancellationToken cancellation
            )
        {
            var dbAddress = await GetDatabaseAddress(id, cancellation);
            if (dbAddress == null)
            {
                throw new AddressException(
                    $"{Messages.Address_Cmd_DivisionId_NotFound}: {id.Value}",
                    DomainExceptionTypeEnum.NotFound);
            }

            var ids = dbAddress.Division.PathIds?
                .Split("-")
                .Select(int.Parse)
                .ToList() ?? [];
            ids.Add(dbAddress.DivisionId);

            var dbDivisions = await _context.AdministrativeDivisions
                .Include(x => x.AdministrativeType)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellation);

            dbDivisions = dbDivisions
                .OrderBy(x => ids.IndexOf(x.Id))
                .ToList();

            var hierarchy = dbDivisions.ToDictionary(
                x => new DivisionId(x.Id),
                x => x);

            return _mapper.DomainAddress(dbAddress, hierarchy);
        }

        public async Task<Dictionary<AddressId, DomainAddress>> GetAddressDictionaryAsync
           (
           IEnumerable<AddressId> ids,
           CancellationToken cancellation
           )
        {
            var uniqueIds = ids.Distinct();
            var dictionary = await GetDatabaseAddressesDictionary(uniqueIds, cancellation);

            var dictionaryIds = new Dictionary<AddressId, List<int>>();
            foreach (var pair in dictionary)
            {
                var addressIds = pair.Value.Division.PathIds?
                    .Split("-")
                    .Select(int.Parse)
                    .ToList() ?? [];
                addressIds.Add(pair.Value.DivisionId);
                dictionaryIds.Add(pair.Key, addressIds);
            }

            var idsOnly = dictionaryIds.SelectMany(x => x.Value).ToHashSet();

            var divisions = await _context.AdministrativeDivisions
                .Include(x => x.AdministrativeType)
                .Where(x => idsOnly.Contains(x.Id))
                .ToDictionaryAsync(
                    x => x.Id,
                    cancellation);

            var result = new Dictionary<AddressId, DomainAddress>();
            foreach (var pair in dictionaryIds)
            {
                var hierarchy = pair.Value
                    .Where(id => divisions.ContainsKey(id))
                    .ToDictionary(
                        id => new DivisionId(id),
                        id => divisions[id]
                    );
                var domainAddress = _mapper.DomainAddress(
                    dictionary[pair.Key], hierarchy);
                result.Add(domainAddress.Id, domainAddress);
            }
            return result;
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
        private async Task<Address> GetDatabaseAddress
            (
            AddressId id,
            CancellationToken cancellation
            )
        {
            var databaseAddress = await _context.Addresses
                .Where(x => x.Id == id.Value)
                .Include(x => x.Street)
                .ThenInclude(x => x.AdministrativeType)
                .Include(x => x.Division)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellation);

            if (databaseAddress == null)
            {
                throw new AddressException
                    (
                    Messages.Address_Query_Id_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseAddress;
        }

        private async Task<Dictionary<AddressId, Address>> GetDatabaseAddressesDictionary
           (
           IEnumerable<AddressId> ids,
           CancellationToken cancellation
           )
        {
            var idsList = ids.Select(id => id.Value).ToList();
            var addresses = await _context.Addresses
                .Where(x => idsList.Contains(x.Id))
                .Include(x => x.Street)
                .ThenInclude(x => x.AdministrativeType)
                .Include(x => x.Division)
                .AsNoTracking()
                .ToDictionaryAsync
                (
                x => new AddressId(x.Id),
                x => x,
                cancellation
                );

            var missingIds = ids.Where(x => !addresses.ContainsKey(x));

            if (missingIds.Any())
            {
                var builder = new StringBuilder();
                builder.AppendLine(Messages.Address_Query_Id_NotFound);
                foreach (var id in missingIds)
                {
                    builder.AppendLine(id.Value.ToString());
                }

                throw new AddressException
                    (
                    builder.ToString(),
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return addresses;
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Past Methods
        /*
                public async Task<IEnumerable<CollocationResponseDto>> GetCollocationsAsync
                    (
                    string divisionName,
                    string streetName,
                    CancellationToken cancellation
                    )
                {
                    var collocations = new List<CollocationResponseDto>();

                    var divisionIdStreetList = await _sql.GetCollocationsAsync(divisionName, streetName, cancellation);
                    foreach (var item in divisionIdStreetList)
                    {
                        var street = item.Street;
                        var dictionaryDivisions = await _sql
                            .GetDivisionsHierachyUpAsync(item.DivisionId, cancellation);
                        collocations.Add(new CollocationResponseDto(dictionaryDivisions, street));
                    }
                    return collocations;
                }

                public async Task<IEnumerable<StreetResponseDto>> GetStreetsAsync
                    (
                    int divisionId,
                    CancellationToken cancellation
                    )
                {
                    return await _context.Streets
                        .Include(x => x.AdministrativeType)
                        .Where(x => x.Divisions.Any(x => x.Id == divisionId))
                        .AsNoTracking()
                        .Select(y => new StreetResponseDto
                        {
                            Id = y.Id,
                            Name = y.Name,
                            AdministrativeType = y.AdministrativeType == null ? null : new AdministrativeTypeResponseDto
                            {
                                Id = y.AdministrativeType.Id,
                                Name = y.AdministrativeType.Name,
                            }
                        })
                        .ToListAsync(cancellation);
                }

                public async Task<IEnumerable<DivisionUpResp>> GetDivisionsDownHorizontalAsync
                    (
                    int? divisionId,
                    CancellationToken cancellation
                    )
                {
                    if (!divisionId.HasValue)
                    {
                        var woj = await _context.AdministrativeDivisions
                            .Include(x => x.AdministrativeType)
                            .Where(x => x.ParentDivisionId == null)
                            .AsNoTracking()
                            .ToListAsync(cancellation);

                        return woj.Select(x => new DivisionUpResp(x));// woj 
                    }

                    var ids = await _sql.GetDivisionIdsDownAsync(divisionId.Value, cancellation);
                    var results = await Task.WhenAll(ids.Select(id =>
                            _sql.GetDivisionsHierachyUpAsync(id, cancellation)
                        ));
                    return results.Select(x => new DivisionUpResp(x));
                }


                public async Task<IEnumerable<DivisionStreetsResponseDto>> GetDivisionsDownVerticalAsync
                    (
                    DivisionId? id,
                    CancellationToken cancellation
                    )
                {
                    if (id is null)
                    {
                        return await _context.AdministrativeDivisions
                        .Include(x => x.AdministrativeType)
                        .Where(x => x.ParentDivisionId == null)
                        .Select(x => new DivisionStreetsResponseDto(x))
                        .AsNoTracking()
                        .ToListAsync(cancellation);
                    }
                    else
                    {
                        return await _context.AdministrativeDivisions
                        .Include(x => x.AdministrativeType)
                        .Include(x => x.Streets)
                        .ThenInclude(x => x.AdministrativeType)
                        .Where(x => x.ParentDivisionId == id.Value)
                        .Select(x => new DivisionStreetsResponseDto(x))
                        .AsNoTracking()
                        .ToListAsync(cancellation);
                    }
                }*/
    }
}
