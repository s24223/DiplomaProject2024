using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Addresses.DTOs.Select.Collocations;
using Application.Features.Addresses.DTOs.Select.Shared;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Interfaces
{
    public class AddressRepository : IAddressRepository
    {
        //Values
        private readonly IAddressSqlClientRepository _sql;
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public AddressRepository
            (
            IEntityToDomainMapper mapper,
            IAddressSqlClientRepository sql,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
            _sql = sql;
            _mapper = mapper;
            _exceptionsRepository = exceptionsRepository;
            _context = context;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        //DML
        public async Task<Guid> CreateAsync
            (
            DomainAddress address,
            CancellationToken cancellation
            )
        {
            try
            {
                var redundancy = await _context.Addresses.Where(x =>
                    x.DivisionId == address.DivisionId.Value &&
                    x.StreetId == address.StreetId.Value &&
                    x.BuildingNumber == address.BuildingNumber.Value &&
                    x.ApartmentNumber == (address.ApartmentNumber == null ?
                        null : address.ApartmentNumber.Value)
                    ).FirstOrDefaultAsync(cancellation);

                if (redundancy != null)
                {
                    //updating Zip Code Thinking a new is correct
                    if (redundancy.ZipCode != address.ZipCode.Value)
                    {
                        redundancy.ZipCode = address.ZipCode.Value;
                        await _context.SaveChangesAsync(cancellation);
                    }
                    return redundancy.Id;
                }

                var collocation = await _context.Streets
                    .Include(x => x.Divisions)
                    .Where(x =>
                    x.Id == address.StreetId.Value &&
                    x.Divisions.Any(y => y.Id == address.DivisionId.Value)
                    )
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellation);

                if (collocation == null)
                {
                    throw new AddressException
                        (
                        Messages.Address_Collocation_NotFound,
                        DomainExceptionTypeEnum.NotFound
                        );
                }

                var databaseAddress = new Address
                {
                    DivisionId = address.DivisionId.Value,
                    StreetId = address.StreetId.Value,
                    BuildingNumber = address.BuildingNumber.Value,
                    ApartmentNumber = address.ApartmentNumber == null ?
                    null : address.ApartmentNumber.Value,
                    ZipCode = address.ZipCode.Value,
                };
                await _context.Addresses.AddAsync(databaseAddress, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return databaseAddress.Id;
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateAsync
            (
            DomainAddress address,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseAddress = await GetDatabaseAddress(address.Id, cancellation);

                databaseAddress.ZipCode = address.ZipCode.Value;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        //DQL
        public async Task<IEnumerable<CollocationResponseDto>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            var collocations = new List<CollocationResponseDto>();

            var list = await _sql.GetCollocationsAsync(divisionName, streetName, cancellation);
            foreach (var item in list)
            {
                var street = item.Street;
                var hierarchy = await _sql.GetDivisionsHierachyUpAsync
                    (
                    item.DivisionId,
                    cancellation
                    );

                collocations.Add(new CollocationResponseDto(hierarchy, street));
            }
            return collocations;
        }

        public async Task<DomainAddress> GetAddressAsync
            (
            AddressId id,
            CancellationToken cancellation
            )
        {
            var databaseAddress = await GetDatabaseAddress(id, cancellation);
            var databseHierarchy = await _sql
                .GetDivisionsHierachyUpAsync(databaseAddress.DivisionId, cancellation);

            return _mapper.ToDomainAddress(databaseAddress, databseHierarchy);
        }

        public async Task<IEnumerable<DivisionResponseDto>> GetDivisionsDownAsync
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
                .Select(x => new DivisionResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentDivisionId,
                    AdministrativeType = new AdministrativeTypeResponseDto
                    {
                        Id = x.AdministrativeType.Id,
                        Name = x.AdministrativeType.Name,
                    }
                })
                .AsNoTracking()
                .ToListAsync(cancellation);
            }
            else
            {
                return await _context.AdministrativeDivisions
                .Include(x => x.AdministrativeType)
                .Where(x => x.ParentDivisionId == id.Value)
                .Select(x => new DivisionResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentDivisionId,
                    AdministrativeType = new AdministrativeTypeResponseDto
                    {
                        Id = x.AdministrativeType.Id,
                        Name = x.AdministrativeType.Name,
                    }
                })
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
    }
}
