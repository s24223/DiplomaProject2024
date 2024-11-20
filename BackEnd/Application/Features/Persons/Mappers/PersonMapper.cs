using Application.Databases.Relational.Models;
using Application.Features.Addresses.Queries.Interfaces;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.Person.Entities;
using Domain.Shared.Factories;

namespace Application.Features.Persons.Mappers
{
    public class PersonMapper : IPersonMapper
    {
        //Values
        private readonly IDomainFactory _factory;
        private readonly IAddressQueryRepo _addressRepository;


        //Constructor
        public PersonMapper
            (
            IDomainFactory factory,
            IAddressQueryRepo addressRepository
            )
        {
            _factory = factory;
            _addressRepository = addressRepository;
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
        public async Task<DomainPerson> DomainPerson(Person database, CancellationToken cancellation)
        {
            var characteristics = database.PersonCharacteristics
                .Select(x => MapCharacteristicId(x));
            var domainPerson = _factory.CreateDomainPerson
                (
                    database.UserId,
                    database.UrlSegment,
                    database.Created,
                    database.ContactEmail,
                    database.Name,
                    database.Surname,
                    database.BirthDate,
                    database.ContactPhoneNum,
                    database.Description,
                    database.IsStudent,
                    database.IsPublicProfile,
                    database.AddressId
                );
            domainPerson.SetCharacteristics(characteristics);

            if (database.AddressId.HasValue)
            {
                var address = await _addressRepository
                .GetAddressAsync(new AddressId(database.AddressId), cancellation);

                domainPerson.Address = address;
            }
            return domainPerson;
        }
        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
        private static (CharacteristicId, QualityId?) MapCharacteristicId(PersonCharacteristic database)
        {
            return (
                new CharacteristicId(database.CharacteristicId),
                (database.QualityId.HasValue ? new QualityId(database.QualityId.Value) : null)
                );
        }

    }
}
