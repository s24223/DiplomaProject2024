using Application.Features.User.DTOs.CommandsUser.Create;
using Application.Features.User.DTOs.CommandsUser.LoginIn;
using Application.Features.User.DTOs.CommandsUser.Refresh;
using Application.Features.User.DTOs.CommandsUser.UpdateLogin;
using Application.Features.User.DTOs.CommandsUser.UpdatePassword;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.User.Services.CommandsUser
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
