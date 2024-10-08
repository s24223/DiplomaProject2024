using Application.Features.User.UrlPart.DTOs;
using Application.Features.User.UrlPart.DTOs.Create;
using Application.Features.User.UrlPart.DTOs.Update;
using Application.Features.User.UrlPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Url.ValueObjects.UrlTypePart;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using System.Security.Claims;

namespace Application.Features.User.UrlPart.Services
{
    public class UrlService : IUrlService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authentication;
        private readonly IProvider _domainProvider;
        private readonly IUrlRepository _urlRepository;


        //Cosntructor
        public UrlService(
            IDomainFactory domainFactory,
            IAuthenticationService authentication,
            IProvider domainProvider,
            IUrlRepository urlRepository
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
                _domainProvider.TimeProvider().GetDateTimeNow(),
                dto.Path,
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

            url.Path = new Uri(dto.Path);
            url.Name = dto.Name;
            url.Description = dto.Description;

            await _urlRepository.UpdateAsync(url, cancellation);

            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
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

            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }

        //DQL
        public async Task<IEnumerable<UrlResponseDto>> GetUrlsAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var list = await _urlRepository.GetUrlsAsync(userId, cancellation);

            return list.Select(x => new UrlResponseDto
            {
                UserId = x.Id.UserId.Value,
                UrlTypeId = (int)x.Id.UrlType.Type,
                UrlType = x.Id.UrlType.Name,
                UrlTypeDescription = x.Id.UrlType.Description,
                Created = x.Id.Created,
                Path = x.Path.ToString(),
                Name = x.Name,
                Description = x.Description,
            }).ToList();
        }

        public IEnumerable<UrlTypeResponseDto> GetUrlTypes() =>
            UrlType.GetTypesDictionary().Values.Select(x => new UrlTypeResponseDto
            {
                Id = (int)x.Type,
                Name = x.Name,
                Description = x.Description,
            }).ToList();

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
    }
}
