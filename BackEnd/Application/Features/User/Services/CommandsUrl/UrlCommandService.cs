using Application.Features.User.DTOs.CommandsUrl;
using Application.Features.User.DTOs.CommandsUrl.Create;
using Application.Features.User.DTOs.CommandsUrl.Update;
using Application.Features.User.Interfaces.CommandsUrl;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Url.ValueObjects.UrlTypePart;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using System.Security.Claims;

namespace Application.Features.User.Services.CommandsUrl
{
    public class UrlCommandService : IUrlCommandService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authentication;
        private readonly IProvider _domainProvider;
        private readonly IUrlCommandRepository _urlRepository;


        //Cosntructor
        public UrlCommandService(
            IDomainFactory domainFactory,
            IAuthenticationService authentication,
            IProvider domainProvider,
            IUrlCommandRepository urlRepository
            )
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
            _domainProvider = domainProvider;
            _urlRepository = urlRepository;
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        //DML
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
                userId.Value,
                dto.UrlTypeId,
                dto.Path,
                dto.Name,
                dto.Description
                );
            await _urlRepository.CreateAsync(url, cancellation);
            return new Response { };
        }

        public async Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            int urlTypeId,
            DateTime created,
            UpdateUrlRequestDto dto,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);

            var url = await _urlRepository.GetUrlAsync
                (
                    userId,
                    new UrlType(urlTypeId),
                    created,
                    cancellation
                );
            url.Update(dto.Path, dto.Name, dto.Description);

            await _urlRepository.UpdateAsync(url, cancellation);
            return new Response { };
        }

        public async Task<Response> DeleteAsync
            (
            IEnumerable<Claim> claims,
            int urlTypeId,
            DateTime created,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);

            await _urlRepository.DeleteAsync
                (
                    userId,
                    new UrlType(urlTypeId),
                    created,
                    cancellation
                );

            return new Response { };
        }

        //DQL
        public IEnumerable<UrlTypeResponseDto> GetUrlTypes() =>
            UrlType.GetTypesDictionary().Values
            .Select(x => new UrlTypeResponseDto(x)).ToList();

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
    }
}
