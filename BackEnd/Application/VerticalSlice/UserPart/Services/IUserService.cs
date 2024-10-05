using Application.Shared.DTOs.Response;
using Application.VerticalSlice.UserPart.DTOs.Create;
using Application.VerticalSlice.UserPart.DTOs.LoginIn;
using Application.VerticalSlice.UserPart.DTOs.Refresh;
using Application.VerticalSlice.UserPart.DTOs.UpdateLogin;
using Application.VerticalSlice.UserPart.DTOs.UpdatePassword;
using System.Security.Claims;

namespace Application.VerticalSlice.UserPart.Services
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
        Task<ItemResponse<LoginInResponseDto>> LoginInAsync
            (
            LoginInRequestDto dto,
            CancellationToken cancellation
            );

        Task<ItemResponse<RefreshResponseDto>> RefreshTokenAsync
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
