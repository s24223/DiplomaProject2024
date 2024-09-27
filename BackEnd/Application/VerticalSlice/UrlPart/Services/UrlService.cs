using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.UrlPart.DTOs;
using Application.VerticalSlice.UrlPart.DTOs.Create;
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

        public async Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateUrlRequestDto dto,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var url = _domainFactory.CreateDomainUrl
                (
                userId,
                dto.UrlTypeId,
                _domainProvider.GetTimeProvider().GetDateTimeNow(),
                dto.Url,
                dto.Name,
                dto.Description
                );
            await _urlRepository.CreateAsync(url, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }

        public IEnumerable<UrlTypeResponseDto> GetUrlTypes() =>
            UrlType.GetTypesDictionary().Values.Select(x => new UrlTypeResponseDto
            {
                Id = (int)x.Type,
                Name = x.Name,
                Description = x.Description,
            }).ToList();
    }
}
