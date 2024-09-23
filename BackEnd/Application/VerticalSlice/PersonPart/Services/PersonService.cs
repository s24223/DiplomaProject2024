using Application.Shared.Services.Authentication;
using Application.VerticalSlice.PersonPart.DTOs.CreateProfile;
using Application.VerticalSlice.PersonPart.Interfaces;
using Domain.Factories;
using Domain.Providers;
using System.Security.Claims;

namespace Application.VerticalSlice.PersonPart.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IDomainProvider _domainProvider;


        public PersonService
            (
            IPersonRepository repository,
            IAuthenticationService authentication,
            IDomainFactory domainFactory,
            IDomainProvider domainProvider
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
            _domainProvider = domainProvider;
        }

        public async Task CreatePersonProfileAsync(
            IEnumerable<Claim> claims,
            CreatePersonProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainPerson = _domainFactory.CreateDomainPerson
                (
                 id,
                 dto.UrlSegment,
                 _domainProvider.GetTimeProvider().GetDateOnlyToday(),
                 dto.ContactEmail,
                 dto.Name,
                 dto.Surname,
                 dto.BirthDate,
                 dto.ContactPhoneNum,
                 dto.Description,
                 dto.IsStudent,
                 dto.IsPublicProfile,
                 null
                /*dto.AddressId*/
                );
            await _repository.CreatePersonProfileAsync(domainPerson, cancellation);
        }
    }
}
