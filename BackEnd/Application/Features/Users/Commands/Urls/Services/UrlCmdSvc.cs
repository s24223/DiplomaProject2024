using Application.Features.Users.Commands.Urls.DTOs;
using Application.Features.Users.Commands.Urls.DTOs.Response;
using Application.Features.Users.Commands.Urls.DTOs.Update;
using Application.Features.Users.Commands.Urls.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Url.Entities;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Users.Commands.Urls.Services
{
    public class UrlCmdSvc : IUrlCmdSvc
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthJwtSvc _authentication;
        private readonly IUrlCmdRepo _urlRepository;


        //Cosntructor
        public UrlCmdSvc
            (
            IDomainFactory domainFactory,
            IAuthJwtSvc authentication,
            IUrlCmdRepo urlRepository
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
        public async Task<ResponseItem<DmlUrlsResp>> CreateAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<CreateUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var userId = GetUserId(claims);
            var urls = dtos.Select(x => _domainFactory.CreateDomainUrl
                (
                userId.Value,
                x.UrlTypeId,
                x.Path,
                x.Name,
                x.Description
                ));

            var beforeDB = DomainUrl.SeparateDuplicates(urls);
            if (beforeDB.Duplicates.Any())
            {
                return new ResponseItem<DmlUrlsResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Url_Cmd_PathConflicts,
                    Item = new DmlUrlsResp(beforeDB.Correct, beforeDB.Duplicates)
                };
            }

            var adterDBData = await _urlRepository.CreateAsync(userId, beforeDB.Correct, cancellation);
            if (adterDBData.Input.Any())
            {
                var data = DomainUrl
                    .FindConflictsWithDatabase(adterDBData.Database, adterDBData.Input);

                return new ResponseItem<DmlUrlsResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Url_Cmd_PathConflicts,
                    Item = new DmlUrlsResp(data.Correct, data.Duplicates)
                };
            }

            return new ResponseItem<DmlUrlsResp>
            {
                Item = new DmlUrlsResp(adterDBData.Database),
            };
        }


        public async Task<ResponseItem<DmlUrlsResp>> UpdateAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var userId = GetUserId(claims);
            var dtoDictionary = new Dictionary<UrlId, UpdateUrlReq>();
            foreach (var dto in dtos)
            {
                var key = new UrlId(userId, dto.UrlTypeId, dto.Created);
                dtoDictionary[key] = dto;
            }
            var domainDictionary = await _urlRepository
                .GetUrlsDictionaryAsync(dtoDictionary.Keys, cancellation);


            foreach (var key in dtoDictionary.Keys)
            {
                var dto = dtoDictionary[key];
                var domain = domainDictionary[key];

                domain.Update(dto.Data.Path, dto.Data.Name, dto.Data.Description);
            }

            var beforeDB = DomainUrl.SeparateDuplicates(domainDictionary.Values);
            if (beforeDB.Duplicates.Any())
            {
                return new ResponseItem<DmlUrlsResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Url_Cmd_PathConflicts,
                    Item = new DmlUrlsResp(beforeDB.Correct, beforeDB.Duplicates)
                };
            }


            var adterDBData = await _urlRepository.UpdateAsync(userId, domainDictionary, cancellation);
            if (adterDBData.Input.Any())
            {
                var data = DomainUrl
                    .FindConflictsWithDatabase(adterDBData.Database, adterDBData.Input);

                return new ResponseItem<DmlUrlsResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Url_Cmd_PathConflicts,
                    Item = new DmlUrlsResp(data.Correct, data.Duplicates)
                };
            }

            return new ResponseItem<DmlUrlsResp>
            {
                Item = new DmlUrlsResp(adterDBData.Database),
            };
        }


        public async Task<Response> DeleteAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<DeleteUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var userId = GetUserId(claims);
            var ids = dtos
                .Select(x => new UrlId(userId, x.UrlTypeId, x.Created))
                .ToHashSet();
            await _urlRepository.DeleteAsync(ids, cancellation);
            return new Response { };
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
        private UserId GetUserId(IEnumerable<Claim> claims)
        {
            return _authentication.GetIdNameFromClaims(claims);
        }
    }
}
