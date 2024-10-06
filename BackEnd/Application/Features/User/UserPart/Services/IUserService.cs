using Application.Features.User.UserPart.DTOs.Create;
using Application.Features.User.UserPart.DTOs.LoginIn;
using Application.Features.User.UserPart.DTOs.Refresh;
using Application.Features.User.UserPart.DTOs.UpdateLogin;
using Application.Features.User.UserPart.DTOs.UpdatePassword;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.User.UserPart.Services
{
    public interface IUserService
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
