
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.OfferPart.DTOs.CreateProfile;
using Application.VerticalSlice.OfferPart.Interfaces;
using Domain.Entities.CompanyPart;
using Domain.Factories;
using Domain.Providers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.VerticalSlice.OfferPart.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IDomainProvider _domainProvider;


        public OfferService
            (
            IOfferRepository repository,
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

        public async Task CreateOfferProfileAsync
            (
            //IEnumerable<Claim> claims,
            CreateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            //var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainOffer = _domainFactory.CreateDomainOffer(
                dto.Id,
                dto.Name,
                dto.Description,
                dto.MinSalary,
                dto.MaxSalary,
                dto.NegotiatedSalary,
                dto.ForStudents
 
            );
            await _repository.CreateOfferProfileAsync(domainOffer, cancellation);
        }
    }
}
