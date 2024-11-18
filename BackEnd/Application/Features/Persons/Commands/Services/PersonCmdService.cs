using Application.Features.Persons.Commands.DTOs.Create;
using Application.Features.Persons.Commands.DTOs.Update;
using Application.Features.Persons.Commands.Interfaces;
using Application.Shared.DTOs.Features.Characteristics.Requests;
using Application.Shared.DTOs.Features.Persons;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Persons.Commands.Services
{
    public class PersonCmdService : IPersonCmdService
    {
        //Values
        private readonly IPersonCmdRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;


        //Cosntructor
        public PersonCmdService
            (
            IPersonCmdRepository repository,
            IDomainFactory domainFactory,
            IAuthenticationService authentication
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods

        //DML
        public async Task<ResponseItem<PersonResponseDto>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreatePersonRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = GetId(claims);
            var cahrachteristics = MapCharacteristics(dto.Characteristics);
            var domainPerson = _domainFactory.CreateDomainPerson
                (
                 id.Value,
                 dto.UrlSegment,
                 dto.ContactEmail,
                 dto.Name,
                 dto.Surname,
                 (DateOnly?)dto.BirthDate,
                 dto.ContactPhoneNum,
                 dto.Description,
                 dto.IsStudent,
                 dto.IsPublicProfile,
                 dto.AddressId
                );
            domainPerson.SetCharacteristics(cahrachteristics);

            domainPerson = await _repository.CreateAsync(domainPerson, cancellation);
            return new ResponseItem<PersonResponseDto>
            {
                Item = new PersonResponseDto(domainPerson),
            };
        }

        public async Task<ResponseItem<PersonResponseDto>> UpdateAsync
            (
            IEnumerable<Claim> claims,
            UpdatePersonRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = GetId(claims);
            var cahrachteristics = MapCharacteristics(dto.Characteristics);
            var domainPerson = await _repository.GetPersonAsync(id, cancellation);

            domainPerson.Update
                (
                dto.UrlSegment,
                dto.ContactEmail,
                dto.Name,
                dto.Surname,
                (DateOnly?)dto.BirthDate,
                dto.ContactPhoneNum,
                dto.Description,
                dto.IsStudent,
                dto.IsPublicProfile,
                dto.AddressId
                );
            domainPerson.SetCharacteristics(cahrachteristics);

            domainPerson = await _repository.UpdateAsync(domainPerson, cancellation);
            return new ResponseItem<PersonResponseDto>
            {
                Item = new PersonResponseDto(domainPerson),
            };
        }


        //DQL
        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
        private UserId GetId(IEnumerable<Claim> claims)
        {
            return _authenticationRepository.GetIdNameFromClaims(claims);
        }

        private IEnumerable<(CharacteristicId, QualityId?)> MapCharacteristics
            (IEnumerable<CharacteristicCollocationRequestDto> characteristics)
        {
            return characteristics.Select(x => ((CharacteristicId, QualityId?))x);
        }
    }
}
