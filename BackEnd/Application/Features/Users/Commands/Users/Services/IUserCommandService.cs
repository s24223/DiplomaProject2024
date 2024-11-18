using Application.Features.Users.Commands.Users.DTOs.Create;
using Application.Features.Users.Commands.Users.DTOs.LoginIn;
using Application.Features.Users.Commands.Users.DTOs.Refresh;
using Application.Features.Users.Commands.Users.DTOs.UpdateLogin;
using Application.Features.Users.Commands.Users.DTOs.UpdatePassword;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Users.Commands.Users.Services
{
    public interface IUserCommandService
    {
        //==========================================================================================================================================
        //Data Part
        Task<Response> CreateAsync
            (
            CreateUserRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateLoginAsync
            (
            IEnumerable<Claim> claims,
            UpdateLoginRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdatePasswordAsync
            (
            IEnumerable<Claim> claims,
            UpdatePasswordRequestDto dto,
            CancellationToken cancellation
            );

        //==========================================================================================================================================
        //Authetication Part
        Task<ResponseItem<LoginInResponseDto>> LoginInAsync
            (
            LoginInRequestDto dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<RefreshResponseDto>> RefreshTokenAsync
            (
            string jwtFromHeader,
            RefreshRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> LogOutAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            );
    }
}
