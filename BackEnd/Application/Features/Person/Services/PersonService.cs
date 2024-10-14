using Application.Features.Person.DTOs.Create;
using Application.Features.Person.DTOs.Update;
using Application.Features.Person.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Person.Services
{
    public class PersonService : IPersonService
    {
        //Values
        private readonly IPersonRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;


        //Cosntructor
        public PersonService
            (

            IPersonRepository repository,
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
        public async Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreatePersonRequestDto dto,
            CancellationToken cancellation
            )
        {
            DateOnly? birthDate = dto.BirthDate == null ? null : (DateOnly)dto.BirthDate;
            var id = _authenticationRepository.GetIdNameFromClaims(claims);

            var domainPerson = _domainFactory.CreateDomainPerson
                (
                 id.Value,
                 dto.UrlSegment,
                 dto.ContactEmail,
                 dto.Name,
                 dto.Surname,
                 birthDate,
                 dto.ContactPhoneNum,
                 dto.Description,
                 dto.IsStudent,
                 dto.IsPublicProfile,
                 dto.AddressId
                );

            await _repository.CreateAsync(domainPerson, cancellation);
            return new Response { };
        }

        public async Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            UpdatePersonRequestDto dto,
            CancellationToken cancellation
            )
        {
            DateOnly? birthDate = dto.BirthDate == null ? null : (DateOnly)dto.BirthDate;
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainPerson = await _repository.GetPersonAsync(id, cancellation);

            domainPerson.Update
                (
                dto.UrlSegment,
                dto.ContactEmail,
                dto.Name,
                dto.Surname,
                birthDate,
                dto.ContactPhoneNum,
                dto.Description,
                dto.IsStudent,
                dto.IsPublicProfile,
                dto.AddressId
                );

            await _repository.UpdateAsync(domainPerson, cancellation);
            return new Response { };
        }


        //DQL
        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
