using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Shared.Interfaces.SqlClient;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects;
using Domain.Shared.Factories;
using Domain.Shared.Templates.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Commands.Interfaces
{
    public class AddressCmdRepo : IAddressCmdRepo
    {
        //Values
        private readonly ISqlClientRepo _sql;
        private readonly IDomainFactory _domainFactory;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public AddressCmdRepo(
            ISqlClientRepo sql,
            IDomainFactory domainFactory,
            DiplomaProjectContext context
            )
        {
            _sql = sql;
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
            string wojewodztwo,
            string? powiat,
            string? gmina,
            string city,
            string? dzielnica,
            string? street,
            double lon,
            double lat,
            ZipCode postcode,
            BuildingNumber houseNumber,
            ApartmentNumber? apartmentNumber,
            CancellationToken cancellation
            )
        {
            try
            {
                var pair = await _sql.GetDivisionIdStreetIdAsync(
                    wojewodztwo, powiat, gmina, city, dzielnica, street, cancellation);

                var duplicate = await _context.Addresses
                    .Where(x =>
                        x.DivisionId == pair.DivisionId &&
                        x.StreetId == pair.StreetId &&
                        x.BuildingNumber == (string)houseNumber &&
                        x.ApartmentNumber == (string?)apartmentNumber
                        )
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellation);

                if (duplicate != null)
                {
                    return duplicate.Id;
                }

                var databaseAddress = new Address
                {
                    DivisionId = pair.DivisionId,
                    StreetId = pair.StreetId,
                    BuildingNumber = (string)houseNumber,
                    ApartmentNumber = (string?)apartmentNumber,
                    ZipCode = (string)postcode,
                    Lon = lon,
                    Lat = lat,
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


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods        

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
