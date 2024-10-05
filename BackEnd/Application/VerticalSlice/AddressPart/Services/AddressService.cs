using Application.Shared.Services.Authentication;
using Application.VerticalSlice.CompanyPart.Interfaces;
using Domain.Factories;
using Domain.Providers;

namespace Application.VerticalSlice.AddressPart.Services
{
    public class AddressService : IAddressService
    {
        private readonly ICompanyRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IProvider _domainProvider;


        public AddressService
            (
            ICompanyRepository repository,
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



    }
}
