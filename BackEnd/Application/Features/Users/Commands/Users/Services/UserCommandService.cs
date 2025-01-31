using Application.Features.Users.Commands.Users.DTOs;
using Application.Features.Users.Commands.Users.DTOs.Create;
using Application.Features.Users.Commands.Users.DTOs.LoginIn;
using Application.Features.Users.Commands.Users.DTOs.Refresh;
using Application.Features.Users.Commands.Users.DTOs.ResetPassword;
using Application.Features.Users.Commands.Users.DTOs.ResetPasswordLink;
using Application.Features.Users.Commands.Users.DTOs.UpdateLogin;
using Application.Features.Users.Commands.Users.DTOs.UpdatePassword;
using Application.Features.Users.Commands.Users.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Interfaces.Email;
using Application.Shared.Services.Authentication;
using Domain.Features.User.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using System.Security.Claims;

namespace Application.Features.Users.Commands.Users.Services
{
    public class UserCommandService : IUserCommandService
    {
        //Values
        private readonly IProvider _provider;
        private readonly IUserCommandRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthJwtSvc _authenticationRepository;
        private readonly IEmailService _emailService;
        private static readonly string _url = "https://localhost:7166/api/User/";
        private static readonly string _urlActivation = $"{_url}activate/";
        private static readonly string _urlReset = $"{_url}reset/";

        //Constructor
        public UserCommandService
            (
            IProvider provider,
            IUserCommandRepository repository,
            IDomainFactory domainFactory,
            IAuthJwtSvc authentication,
            IEmailService emailService
            )
        {
            _provider = provider;
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
            _emailService = emailService;
        }


        //==========================================================================================================================================
        //==========================================================================================================================================
        //==========================================================================================================================================
        //Public Methods
        public async Task<Response> CreateAsync
            (
            CreateUserReq dto,
            CancellationToken cancellation
            )
        {
            var domainUser = _domainFactory.CreateDomainUser(dto.Email);
            var salt = _authenticationRepository.GenerateSalt();
            var password = _authenticationRepository.HashPassword(dto.Password, salt);

            var activationUrlSegment = GenerateUrlSegment();

            var id = await _repository.CreateUserAsync(domainUser, salt, password, activationUrlSegment, cancellation);

            var url = $"{_urlActivation}{id.ToString()}/{activationUrlSegment}";
            await _emailService.SendAsync(
                dto.Email,
                "Activation Account",
                @$"<div>Click on URL for activate account</div><div>{url}</div>");
            return new Response { };
        }

        public async Task<Response> ActivateAsync(Guid id, string activationUrlSegment, CancellationToken cancellation)
        {
            var userData = await _repository.GetUserDataByIdAsync(new UserId(id), cancellation);
            if (string.IsNullOrWhiteSpace(userData.ActivationUrlSegment))
            {
                throw new UserException
                    (
                    Messages.User_Cmd_ProfileActive,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            if (userData.ActivationUrlSegment != activationUrlSegment)
            {
                throw new UserException
                    (
                    Messages.User_Cmd_Unautorized,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }
            await _repository.UpdateAsync(
                userData.User,
                userData.Salt,
                userData.Password,
                null,
                userData.RefreshToken,
                userData.ExpiredToken,
                userData.ResetPasswordUrlSegment,
                userData.ResetPasswordInitiated,
                userData.IsHideProfile,
                cancellation);

            await _emailService.SendAsync(
                userData.User.Login.Value,
                "Account Activated",
                @$"<div>Your Account Activated</div>");
            return new Response { };
        }


        public async Task<Response> ResetPasswordInitiateAsync(
            ResetPasswordLinkReq req,
            CancellationToken cancellation)
        {
            var userData = await _repository.GetUserDataByLoginEmailAsync(
                new Email(req.Email),
                cancellation);


            var urlSegment = GenerateUrlSegment();

            await _repository.UpdateAsync(
                userData.User,
                userData.Salt,
                userData.Password,
                userData.ActivationUrlSegment,
                userData.RefreshToken,
                userData.ExpiredToken,
                urlSegment,
                _provider.TimeProvider().GetDateTimeNow(),
                userData.IsHideProfile,
                cancellation);

            var url = $"{_urlReset}{userData.User.Id.Value.ToString()}/{urlSegment}";
            await _emailService.SendAsync(
                userData.User.Login.Value.ToString(),
                "Reset password",
                @$"<div>Click on URL for reset password</div><div>{url}</div>");
            return new Response { };
        }

        public async Task<Response> ResetPasswordAsync(
            Guid id,
            string resetPasswordUrlSegment,
            ResetPasswordReq req,
            CancellationToken cancellation)
        {
            var userData = await _repository.GetUserDataByIdAsync(new UserId(id), cancellation);
            if (userData.ResetPasswordUrlSegment != resetPasswordUrlSegment)
            {
                throw new UserException
                    (
                    Messages.User_Cmd_Unautorized,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }

            var salt = _authenticationRepository.GenerateSalt();
            var password = _authenticationRepository.HashPassword(req.NewPassword, salt);

            userData.User.LastPasswordUpdate = _provider.TimeProvider().GetDateTimeNow();
            userData.ResetPasswordUrlSegment = null;
            userData.ResetPasswordInitiated = null;
            userData.Salt = salt;
            userData.Password = password;

            await _repository.UpdateAsync(
                userData.User,
                userData.Salt,
                userData.Password,
                userData.ActivationUrlSegment,
                userData.RefreshToken,
                userData.ExpiredToken,
                userData.ResetPasswordUrlSegment,
                userData.ResetPasswordInitiated,
                userData.IsHideProfile,
                cancellation);

            await _emailService.SendAsync(
                userData.User.Login.Value,
                "Password Changed",
                @$"<div>Password Changed</div>");
            return new Response { };
        }


        public async Task<Response> UpdateLoginAsync
            (
            IEnumerable<Claim> claims,
            UpdateLoginReq dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var userData = await _repository.GetUserDataByIdAsync(id, cancellation);
            userData.User.Login = new Email(dto.NewLogin);

            await _repository.UpdateAsync(
                userData.User,
                userData.Salt,
                userData.Password,
                userData.ActivationUrlSegment,
                userData.RefreshToken,
                userData.ExpiredToken,
                userData.ResetPasswordUrlSegment,
                userData.ResetPasswordInitiated,
                userData.IsHideProfile,
                cancellation);
            return new Response { };
        }

        public async Task<Response> UpdatePasswordAsync(
            IEnumerable<Claim> claims,
            UpdatePasswordReq dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var userData = await _repository.GetUserDataByIdAsync(id, cancellation);

            var inputPasswordHashed = _authenticationRepository.HashPassword(dto.OldPassword, userData.Salt);
            if (inputPasswordHashed != userData.Password)
            {
                throw new UserException
                    (
                    Messages.User_Cmd_Unautorized,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }

            userData.User.LastPasswordUpdate = _provider.TimeProvider().GetDateTimeNow();

            var salt = _authenticationRepository.GenerateSalt();
            var password = _authenticationRepository.HashPassword(dto.NewPassword, salt);

            await _repository.UpdateAsync(
                userData.User,
                salt,
                password,
                userData.ActivationUrlSegment,
                userData.RefreshToken,
                userData.ExpiredToken,
                userData.ResetPasswordUrlSegment,
                userData.ResetPasswordInitiated,
                userData.IsHideProfile,
                cancellation);
            return new Response { };
        }


        public async Task<Response> DeleteAsync(
            IEnumerable<Claim> claims,
            DeleteProfileReq req,
            CancellationToken cancellation)
        {
            var userId = _authenticationRepository.GetIdNameFromClaims(claims);
            var userData = await _repository.GetUserDataByIdAsync(userId, cancellation);

            var hashedPassword = _authenticationRepository
                .HashPassword(req.Password, userData.Salt);
            if (hashedPassword != userData.Password)
            {
                throw new UserException
                    (
                    Messages.User_Cmd_Unautorized,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }
            try
            {

                await _repository.DeleteAsync(userId, cancellation);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return new Response { };
        }
        //==========================================================================================================================================
        //Authentication Part
        public async Task<ResponseItem<LoginInResp>> LoginInAsync
            (
            LoginInReq dto,
            CancellationToken cancellation
            )
        {
            var userData = await _repository.GetUserDataByLoginEmailAsync
                (
                new Email(dto.Email),
                cancellation
                );
            userData.User.LastLoginIn = _provider.TimeProvider().GetDateTimeNow();


            var hashedInputPassword = _authenticationRepository.HashPassword
                (
                dto.Password,
                userData.Salt
                );
            if (hashedInputPassword != userData.Password)
            {
                //Incorrect Password
                throw new UserException
                    (
                    Messages.User_Cmd_Unautorized,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }
            if (!string.IsNullOrWhiteSpace(userData.ActivationUrlSegment)) //Nie aktywowano konta
            {
                //Incorrect Password
                throw new UserException
                    (
                    Messages.User_Cmd_ProfileNotActivated,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }



            var roles = new List<string>();
            if (userData.HasCompanyProfile)
            {
                roles.Add(_authenticationRepository.GetCompanyRole());
            }
            if (userData.HasPersonProfile)
            {
                roles.Add(_authenticationRepository.GetPersonRole());
            }


            var jwt = _authenticationRepository.GenerateJwtStringAndDateTimeValidTo
                (
                userData.User.Id.Value.ToString(),
                roles
                );

            if (
                string.IsNullOrWhiteSpace(userData.RefreshToken) ||
                userData.ExpiredToken == null ||
                userData.ExpiredToken <= _provider.TimeProvider().GetDateTimeNow()
                )
            {
                var refresh = _authenticationRepository.GenerateRefreshTokendAndDateTimeValidTo();
                userData.RefreshToken = refresh.RefreshToken;
                userData.ExpiredToken = refresh.ValidTo;
            }
            if (userData.ResetPasswordUrlSegment != null ||
               userData.ResetPasswordInitiated != null)
            {
                userData.ResetPasswordUrlSegment = null;
                userData.ResetPasswordInitiated = null;
            }
            if (userData.IsHideProfile)
            {
                userData.IsHideProfile = false;
            }

            await _repository.UpdateAsync(
               userData.User,
               userData.Salt,
               userData.Password,
               userData.ActivationUrlSegment,
               userData.RefreshToken,
               userData.ExpiredToken,
               userData.ResetPasswordUrlSegment,
               userData.ResetPasswordInitiated,
               userData.IsHideProfile,
               cancellation);

            //Correct
            return new ResponseItem<LoginInResp>
            {
                Item = new LoginInResp
                {
                    Jwt = jwt.Jwt,
                    JwtValidTo = jwt.ValidTo,
                    RefereshToken = userData.RefreshToken,
                    RefereshTokenValidTo = userData.ExpiredToken.Value,
                },
            };
        }

        public async Task<ResponseItem<RefreshResp>> RefreshTokenAsync
            (
            string jwtFromHeader,
            RefreshReq dto,
            CancellationToken cancellation
            )
        {
            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                throw new UserException
                    (
                    Messages.User_Cmd_RefreshToken_IsNullOrWhiteSpace,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }

            var id = _authenticationRepository.GetIdNameFromJwt(jwtFromHeader);
            var data = await _repository.GetUserDataByIdAsync(id, cancellation);

            if (
                data.ExpiredToken == null ||
                data.ExpiredToken <= _provider.TimeProvider().GetDateTimeNow() ||
                string.IsNullOrWhiteSpace(data.RefreshToken) ||
                dto.RefreshToken != data.RefreshToken
                )
            {
                throw new UserException(
                    Messages.User_Cmd_RefreshToken_IsNullOrWhiteSpace,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }


            var roles = new List<string>();
            if (data.User.Company != null)
            {
                roles.Add(_authenticationRepository.GetCompanyRole());
            }
            if (data.User.Person != null)
            {
                roles.Add(_authenticationRepository.GetPersonRole());
            }

            var jwt = _authenticationRepository.GenerateJwtStringAndDateTimeValidTo
                (
                data.User.Id.Value.ToString(),
                roles
                );

            return new ResponseItem<RefreshResp>
            {
                Item = new RefreshResp
                {
                    Jwt = jwt.Jwt,
                    JwtValidTo = jwt.ValidTo,
                    RefereshToken = data.RefreshToken,
                    RefereshTokenValidTo = data.ExpiredToken.Value,
                },
            };
        }

        public async Task<Response> LogOutAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var userData = await _repository.GetUserDataByIdAsync(id, cancellation);
            await _repository.UpdateAsync(
                userData.User,
                userData.Salt,
                userData.Password,
                userData.ActivationUrlSegment,
                null,
                null,
                userData.ResetPasswordUrlSegment,
                userData.ResetPasswordInitiated,
                userData.IsHideProfile,
                cancellation);
            return new Response { };
        }

        //============================================================================================================
        //============================================================================================================
        //============================================================================================================
        //Private Methods

        private string GenerateUrlSegment() =>
            _authenticationRepository.GenerateSalt().Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}
