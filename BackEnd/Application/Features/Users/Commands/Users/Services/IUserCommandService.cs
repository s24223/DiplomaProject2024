using Application.Features.Users.Commands.Users.DTOs.Create;
using Application.Features.Users.Commands.Users.DTOs.LoginIn;
using Application.Features.Users.Commands.Users.DTOs.Refresh;
using Application.Features.Users.Commands.Users.DTOs.ResetPassword;
using Application.Features.Users.Commands.Users.DTOs.ResetPasswordLink;
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
            CreateUserReq dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateLoginAsync
            (
            IEnumerable<Claim> claims,
            UpdateLoginReq dto,
            CancellationToken cancellation
            );

        Task<Response> UpdatePasswordAsync
            (
            IEnumerable<Claim> claims,
            UpdatePasswordReq dto,
            CancellationToken cancellation
            );

        //==========================================================================================================================================
        //Authentication Part
        Task<ResponseItem<LoginInResp>> LoginInAsync
            (
            LoginInReq dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<RefreshResp>> RefreshTokenAsync
            (
            string jwtFromHeader,
            RefreshReq dto,
            CancellationToken cancellation
            );

        Task<Response> LogOutAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            );

        Task<Response> ActivateAsync(
            Guid id,
            string activationUrlSegment,
            CancellationToken cancellation);
        Task<Response> ResetPasswordInitiateAsync(
            ResetPasswordLinkReq req,
            CancellationToken cancellation);

        Task<Response> ResetPasswordAsync(
            Guid id,
            string resetPasswordUrlSegment,
            ResetPasswordReq req,
            CancellationToken cancellation);
    }
}
