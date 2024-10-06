using Application.Features.Person.DTOs.Create;
using Application.Features.Person.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using Domain.Shared.ValueObjects;
using System.Security.Claims;

namespace Application.Features.Person.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IProvider _domainProvider;


        public PersonService
            (
            IPersonRepository repository,
            IAuthenticationService authentication,
            IDomainFactory domainFactory,
            IProvider domainProvider
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
            _domainProvider = domainProvider;
        }

        public async Task<Response> CreatePersonProfileAsync
            (
            IEnumerable<Claim> claims,
            CreatePersonRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainPerson = _domainFactory.CreateDomainPerson
                (
                 id.Value,
                 dto.UrlSegment,
                 _domainProvider.TimeProvider().GetDateOnlyToday(),
                 dto.ContactEmail,
                 dto.Name,
                 dto.Surname,
                 dto.BirthDate,
                 dto.ContactPhoneNum,
                 dto.Description,
                 new DatabaseBool(dto.IsStudent).Code,
                 dto.IsPublicProfile,
                 null
                );
            await _repository.CreatePersonProfileAsync(domainPerson, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess
            };
        }
    }
}
