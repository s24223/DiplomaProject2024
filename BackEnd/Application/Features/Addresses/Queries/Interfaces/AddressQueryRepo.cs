﻿using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Addresses.Queries.DTOs;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.SqlClient;
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
        private readonly ISqlClientRepo _sql;
        private readonly IEntityToDomainMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public AddressQueryRepo
            (
            IEntityToDomainMapper mapper,
            ISqlClientRepo sql,
            DiplomaProjectContext context
            )
        {
            _sql = sql;
            _mapper = mapper;
            _context = context;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        //DML
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


        public async Task<DomainAddress> GetAddressAsync
            (
            AddressId id,
            CancellationToken cancellation
            )
        {
            var dbAddress = await GetDatabaseAddress(id, cancellation);
            var hierarchy = await _sql
                .GetDivisionsHierachyUpAsync(dbAddress.DivisionId, cancellation);
            return _mapper.ToDomainAddress(dbAddress, hierarchy);
        }

        public async Task<Dictionary<AddressId, DomainAddress>> GetAddressDictionaryAsync
           (
           IEnumerable<AddressId> ids,
           CancellationToken cancellation
           )
        {
            var uniqueIds = ids.Distinct();
            var dictionary = await GetDatabaseAddressesDictionary(uniqueIds, cancellation);

            // Lista tasków do pobrania hierarchii
            var tasks = dictionary.Select(async item =>
            {
                var dbAddress = item.Value;
                // Pobierz hierarchię dla danego adresu
                var hierarchy = await _sql
                    .GetDivisionsHierachyUpAsync(dbAddress.DivisionId, cancellation);
                // Zwróć wynik przetworzony za pomocą mappera
                return new KeyValuePair<AddressId, DomainAddress>
                (
                    item.Key,
                    _mapper.ToDomainAddress(dbAddress, hierarchy)
                );
            });

            // Uruchom wszystkie taski równocześnie i czekaj na wyniki
            var resultList = await Task.WhenAll(tasks);
            return resultList.ToDictionary(x => x.Key, x => x.Value);
        }


        public async Task<IEnumerable<DivisionStreetsResponseDto>> GetDivisionsDownAsync
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
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellation);

            if (databaseAddress == null)
            {
                throw new AddressException
                    (
                    Messages.Address_Id_NotFound,
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
            var adresses = await _context.Addresses
                .Where(x => idsList.Contains(x.Id))
                .Include(x => x.Street)
                .ThenInclude(x => x.AdministrativeType)
                .AsNoTracking()
                .ToDictionaryAsync
                (
                x => new AddressId(x.Id),
                x => x,
                cancellation
                );

            var missingIds = ids.Where(x => !adresses.ContainsKey(x));

            if (missingIds.Any())
            {
                var builder = new StringBuilder();
                builder.AppendLine(Messages.Address_Id_NotFound);
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
            return adresses;
        }
    }
}