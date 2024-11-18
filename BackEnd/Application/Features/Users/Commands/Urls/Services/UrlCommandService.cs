using Application.Features.Users.Commands.Urls.DTOs.Create;
using Application.Features.Users.Commands.Urls.DTOs.Delete;
using Application.Features.Users.Commands.Urls.DTOs.Update;
using Application.Features.Users.Commands.Urls.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Users.Commands.Urls.Services
{
    public class UrlCommandService : IUrlCommandService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authentication;
        private readonly IUrlCommandRepository _urlRepository;


        //Cosntructor
        public UrlCommandService
            (
            IDomainFactory domainFactory,
            IAuthenticationService authentication,
            IUrlCommandRepository urlRepository
            )
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
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
            IEnumerable<CreateUrlRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var urls = dtos.Select(x => _domainFactory.CreateDomainUrl
                (
                userId.Value,
                x.UrlTypeId,
                x.Path,
                x.Name,
                x.Description
                ));
            await _urlRepository.CreateAsync(urls, cancellation);
            return new Response { };
        }


        public async Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateUrlRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var values = dtos.ToDictionary(x =>
                    new UrlId(userId, x.UrlTypeId, x.Created), x => x);
            var urlsDictionary = await _urlRepository.GetUrlsDictionaryAsync
                (
                    values.Keys,
                    cancellation
                );

            foreach (var item in urlsDictionary)
            {
                if (values.TryGetValue(item.Key, out var dto))
                {
                    item.Value.Update
                        (
                        dto.UpdateData.Path,
                        dto.UpdateData.Name,
                        dto.UpdateData.Description
                        );
                }
            }

            await _urlRepository.UpdateAsync(urlsDictionary, cancellation);
            return new Response { };
        }


        public async Task<Response> DeleteAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<DeleteUrlRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var values = dtos.Select(x => new UrlId(userId, x.UrlTypeId, x.Created));
            await _urlRepository.DeleteAsync
                (
                    values,
                    cancellation
                );

            return new Response { };
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
    }
}
