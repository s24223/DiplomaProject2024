using Application.Databases.Relational.Models;
using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Shared.Factories;

namespace Application.Features.Addresses.Mappers
{
    public class AddressMapper : IAddressMapper
    {
        //Values
        private readonly IDomainFactory _domainFactory;


        //Constructor
        public AddressMapper(IDomainFactory domainFactory)
        {
            _domainFactory = domainFactory;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Public Methods
        public DomainAddress DomainAddress
           (
           Address databaseAddress,
           Dictionary<DivisionId, AdministrativeDivision> databseDictionary
           )
        {
            var domainHierarchy = databseDictionary.ToDictionary(
                x => x.Key,
                x => new DomainAdministrativeDivision
                (
                x.Value.Id,
                x.Value.Name,
                x.Value.ParentDivisionId,
                x.Value.AdministrativeType.Id,
                x.Value.AdministrativeType.Name
                ));


            var address = _domainFactory.CreateDomainAddress
                (
                databaseAddress.Id,
                databaseAddress.DivisionId,
                databaseAddress.StreetId,
                databaseAddress.BuildingNumber,
                databaseAddress.ApartmentNumber,
                databaseAddress.ZipCode,
                databaseAddress.Lon,
                databaseAddress.Lat
                );

            if (databaseAddress.Street != null)
            {
                address.Street = new DomainStreet
                    (
                    databaseAddress.Street.Id,
                    databaseAddress.Street.Name,
                    databaseAddress.Street.AdministrativeType?.Id,
                    databaseAddress.Street.AdministrativeType?.Name
                    );
            }

            address.SetHierarchy(domainHierarchy);
            return address;
        }
        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Private Methods
    }
}
