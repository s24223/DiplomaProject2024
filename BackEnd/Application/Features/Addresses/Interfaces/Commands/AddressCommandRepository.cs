using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Interfaces.Commands
{
    public class AddressCommandRepository : IAddressCommandRepository
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public AddressCommandRepository
            (
            IDomainFactory domainFactory,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
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
                    x.DivisionId == (int)address.DivisionId.Value &&
                    x.StreetId == (int)address.StreetId.Value &&
                    x.BuildingNumber == (string)address.BuildingNumber &&
                    x.ApartmentNumber == (string?)address.ApartmentNumber
                    ).FirstOrDefaultAsync(cancellation);

                if (redundancy != null)
                {
                    //updating Zip Code Thinking a new is correct
                    if (redundancy.ZipCode != address.ZipCode.Value)
                    {
                        redundancy.ZipCode = (string)address.ZipCode;
                        await _context.SaveChangesAsync(cancellation);
                    }
                    return redundancy.Id;
                }

                var collocation = await _context.Streets
                    .Include(x => x.Divisions)
                    .Where(x =>
                        x.Id == address.StreetId.Value &&
                        x.Divisions.Any(y => y.Id == address.DivisionId.Value)
                    ).AsNoTracking()
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
                    BuildingNumber = (string)address.BuildingNumber,
                    ApartmentNumber = (string?)address.ApartmentNumber,
                    ZipCode = (string)address.ZipCode,
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
                databaseAddress.ZipCode = (string)address.ZipCode;
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        public async Task<DomainAddress> GetAddressAsync
            (
            AddressId id,
            CancellationToken cancellation
            )
        {
            var databseAddress = await GetDatabaseAddress(id, cancellation);
            return _domainFactory.CreateDomainAddress
                (
                databseAddress.Id,
                databseAddress.DivisionId,
                databseAddress.StreetId,
                databseAddress.BuildingNumber,
                databseAddress.ApartmentNumber,
                databseAddress.ZipCode
                );
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
