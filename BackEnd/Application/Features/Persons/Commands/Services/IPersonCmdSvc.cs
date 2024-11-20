using Application.Features.Persons.Commands.DTOs;
using Application.Shared.DTOs.Features.Persons;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.Persons.Commands.Services
{
    public interface IPersonCmdSvc
    {
        Task<ResponseItem<PersonResp>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreatePersonReq dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<PersonResp>> UpdateAsync
            (
            IEnumerable<Claim> claims,
            UpdatePersonReq dto,
            CancellationToken cancellation
            );

    }
}
