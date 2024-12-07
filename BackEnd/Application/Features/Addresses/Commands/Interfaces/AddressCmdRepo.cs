using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Domain.Shared.Templates.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Commands.Interfaces
{
    public class AddressCmdRepo : IAddressCmdRepo
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public AddressCmdRepo
            (
            IDomainFactory domainFactory,
            DiplomaProjectContext context
            )
        {
            _domainFactory = domainFactory;
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
                        Messages2.Address_Collocation_NotFound,
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
                throw HandleException(ex);
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
                throw HandleException(ex);
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
                databseAddress.StreetId ?? -1,
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
                    $"{Messages.Address_Cmd_Id_NotFound}: {id.Value}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databaseAddress;
        }

        private System.Exception HandleException(System.Exception ex)
        {
            if (ex is DbUpdateException dbEx && dbEx.InnerException is SqlException sqlex)
            {
                //547 FK, CHECK
                //2627 - PK UNIUE
                var number = sqlex.Number;
                var message = sqlex.Message;

                var dictionary = new Dictionary<string, string>()
                {
                   { "Address_Division" ,Messages.Address_Cmd_DivisionId_NotFound},
                   { "Address_Street", Messages.Address_Cmd_StreetId_NotFound},
                };

                if (number == 547)
                {
                    foreach (var x in dictionary)
                    {
                        if (message.Contains(x.Key))
                        {
                            return new AddressException
                                (
                                x.Value,
                                DomainExceptionTypeEnum.NotFound
                                );
                        }
                    }
                }
            }
            return ex;
        }
    }
}
