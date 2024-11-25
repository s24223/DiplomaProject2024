using Application.Features.Users.Commands.Urls.DTOs;
using Application.Features.Users.Commands.Urls.DTOs.Response;
using Application.Features.Users.Commands.Urls.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.Users.Commands.Urls.Services
{
    public interface IUrlCmdSvc
    {
        Task<ResponseItem<DmlUrlsResp>> CreateAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<CreateUrlReq> dtos,
            CancellationToken cancellation
            );

        Task<ResponseItem<DmlUrlsResp>> UpdateAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<UpdateUrlReq> dtos,
           CancellationToken cancellation
           );

        Task<Response> DeleteAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<DeleteUrlReq> dtos,
            CancellationToken cancellation
            );

    }
}
