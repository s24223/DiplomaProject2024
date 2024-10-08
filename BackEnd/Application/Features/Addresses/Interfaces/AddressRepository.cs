using Application.Database;
using Application.Features.Addresses.DTOs.Select.Collocations;
using Application.Features.Addresses.DTOs.Select.Shared;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.EntitiesExceptions;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Interfaces
{
    public class AddressRepository : IAddressRepository
    {
        //Values
        private readonly IProvider _provider;
        private readonly IDomainFactory _domainFactory;
        private readonly IAddressSqlClientRepository _sql;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public AddressRepository
            (
            IProvider provider,
            IDomainFactory domainFactory,
            IAddressSqlClientRepository sql,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
            _sql = sql;
            _provider = provider;
            _domainFactory = domainFactory;
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
                    return redundancy.Id;
                }

                var collocation = await _context.Streets
                    .Include(x => x.Divisions)
                    .Where(x =>
                    x.Id == address.StreetId.Value &&
                    x.Divisions.Any(y => y.Id == address.DivisionId.Value)
                    )
                    .FirstOrDefaultAsync(cancellation);

                if (collocation == null)
                {
                    throw new AddressException
                        (
                        Messages.NotFoundAddressCollocation,
                        DomainExceptionTypeEnum.NotFound
                        );
                }

                var databaseAddress = new Database.Models.Address
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
            catch (Exception ex)
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
            var databaseAddress = await _context.Addresses
                .Where(x => x.Id == address.Id.Value)
                .FirstOrDefaultAsync(cancellation);

            if (databaseAddress == null)
            {
                throw new AddressException
                    (
                    Messages.NotFoundAddress,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            databaseAddress.ZipCode = address.ZipCode.Value;

            await _context.SaveChangesAsync(cancellation);
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


                collocations.Add(new CollocationResponseDto
                {
                    Hierarchy = hierarchy.ToList().Select(x => new DivisionResponseDto
                    {
                        Id = x.Id,
                        ParentId = x.ParentDivisionId,
                        Name = x.Name,
                        AdministrativeType = new AdministrativeTypeResponseDto
                        {
                            Id = x.AdministrativeType.Id,
                            Name = x.AdministrativeType.Name,
                        },
                    }),
                    Street = new StreetResponseDto
                    {
                        Id = street.Id,
                        Name = street.Name,
                        AdministrativeType = street.AdministrativeType == null ?
                            null : new AdministrativeTypeResponseDto
                            {
                                Id = street.AdministrativeType.Id,
                                Name = street.AdministrativeType.Name,
                            },
                    },
                });
            }
            return collocations;
        }

        public async Task<DomainAddress> GetAddressAsync
            (
            Guid id,
            CancellationToken cancellation
            )
        {
            var databaseAddress = await _context.Addresses
                .Where(x => x.Id == id)
                .Include(x => x.Street)
                .ThenInclude(x => x.AdministrativeType)
                .FirstOrDefaultAsync(cancellation);

            if (databaseAddress == null)
            {
                throw new AddressException
                    (
                    Messages.NotFoundAddress,
                    DomainExceptionTypeEnum.NotFound
                    );
            }


            var databseHierarchy = await _sql
                .GetDivisionsHierachyUpAsync(databaseAddress.DivisionId, cancellation);
            var domainHierachy = databseHierarchy.Select(x => new DomainAdministrativeDivision
                (
                x.Id,
                x.Name,
                x.ParentDivisionId,
                x.AdministrativeType.Id,
                x.AdministrativeType.Name
                )).ToList();


            var address = _domainFactory.CreateDomainAddress
                (
                databaseAddress.Id,
                databaseAddress.DivisionId,
                databaseAddress.StreetId,
                databaseAddress.BuildingNumber,
                databaseAddress.ApartmentNumber,
                databaseAddress.ZipCode
                );
            address.Street = new DomainStreet
                (
                databaseAddress.Street.Id,
                databaseAddress.Street.Name,
                (databaseAddress.Street.AdministrativeType == null
                ? null : databaseAddress.Street.AdministrativeType.Id),
                (databaseAddress.Street.AdministrativeType == null
                ? null : databaseAddress.Street.AdministrativeType.Name)
                );
            address.SetHierarchy(domainHierachy);
            return address;
        }
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
    }
}
