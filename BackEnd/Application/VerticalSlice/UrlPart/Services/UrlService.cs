using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.UrlPart.DTOs;
using Application.VerticalSlice.UrlPart.Interfaces;
using Domain.Factories;
using Domain.Providers;
using Domain.ValueObjects.PartUrlType;
using System.Security.Claims;

namespace Application.VerticalSlice.UrlPart.Services
{
    public class UrlService : IUrlService
    {
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authentication;
        private readonly IDomainProvider _domainProvider;
        private readonly IUrlRepository _urlRepository;

        public UrlService(
            IDomainFactory domainFactory, 
            IAuthenticationService authentication,
            IDomainProvider domainProvider,
            IUrlRepository urlRepository
            )
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
            _domainProvider = domainProvider;
            _urlRepository = urlRepository;
        }

        public async Task<Response> CreateUrlAsync(IEnumerable<Claim> claims, 
            CreateUrlDto dto, 
            CancellationToken cancellation)
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var url = _domainFactory.CreateDomainUrl
                (
                userId,
                new UrlType(dto.UrlType.Id),
                dto.DateTime,
                dto.Url,
                dto.Name,
                dto.Description,
                _domainProvider
                );
            await _urlRepository.CreateUrlAsync(url, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }
    }
}
