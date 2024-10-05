using Application.Shared.DTOs.Response;
using Application.Shared.Exceptions.UserExceptions;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.UserPart.DTOs.Create;
using Application.VerticalSlice.UserPart.DTOs.LoginIn;
using Application.VerticalSlice.UserPart.DTOs.Refresh;
using Application.VerticalSlice.UserPart.DTOs.UpdateLogin;
using Application.VerticalSlice.UserPart.DTOs.UpdatePassword;
using Application.VerticalSlice.UserPart.Interfaces;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using Domain.Shared.ValueObjects;
using System.Security.Claims;

namespace Application.VerticalSlice.UserPart.Services
{
    public class UserService : IUserService
    {
        //Values
        private readonly IUserRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IProvider _domainProvider;
        private readonly IAuthenticationService _authenticationRepository;


        //Constructor
        public UserService
            (
            IUserRepository repository,
            IDomainFactory domainFactory,
            IProvider domainProvider,
            IAuthenticationService authentication
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _domainProvider = domainProvider;
            _authenticationRepository = authentication;
        }


        //Methods
        //==========================================================================================================================================
        //Data Part
        public async Task<Response> CreateAsync
            (
            CreateUserRequestDto dto,
            CancellationToken cancellation
            )
        {
            var domainUser = _domainFactory.CreateDomainUser(dto.Email);
            var salt = _authenticationRepository.GenerateSalt();
            var password = _authenticationRepository.HashPassword(dto.Password, salt);

            await _repository.CreateUserAsync(domainUser, password, salt, cancellation);

            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }

        public async Task<Response> UpdateLoginAsync
            (
            IEnumerable<Claim> claims,
            UpdateLoginRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var userData = await _repository.GetUserDataByIdAsync(id, cancellation);
            userData.User.Login = new Email(dto.NewLogin);

            await _repository.UpdateAsync
                (
                userData.User,
                userData.Password,
                userData.Salt,
                userData.RefreshToken,
                userData.ExpiredToken,
                cancellation
               );

            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }

        public async Task<Response> UpdatePasswordAsync
            (
            IEnumerable<Claim> claims,
            UpdatePasswordRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var userData = await _repository.GetUserDataByIdAsync(id, cancellation);
            userData.User.LastPasswordUpdate = _domainProvider.TimeProvider().GetDateTimeNow();

            var salt = _authenticationRepository.GenerateSalt();
            var password = _authenticationRepository.HashPassword(dto.NewPassword, salt);

            await _repository.UpdateAsync
                (
                userData.User,
                password,
                salt,
                userData.RefreshToken,
                userData.ExpiredToken,
                cancellation
                );

            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }

        //==========================================================================================================================================
        //Authetication Part
        public async Task<ResponseItem<LoginInResponseDto>> LoginInAsync
            (
            LoginInRequestDto dto,
            CancellationToken cancellation
            )
        {
            var userData = await _repository.GetUserDataByLoginEmailAsync
                (
                new Email(dto.Email),
                cancellation
                );
            userData.User.LastLoginIn = _domainProvider.TimeProvider().GetDateTimeNow();


            var hashedInputPassword = _authenticationRepository.HashPassword
                (
                dto.Password,
                userData.Salt
                );
            if (hashedInputPassword != userData.Password)
            {
                //Incorrect Password
                throw new UnauthorizedUserException();
            }


            var roles = new List<string>();
            if (userData.User.Company != null)
            {
                roles.Add(_authenticationRepository.GetCompanyRole());
            }
            if (userData.User.Person != null)
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
                userData.ExpiredToken <= _domainProvider.TimeProvider().GetDateTimeNow()
                )
            {
                var refresh = _authenticationRepository.GenerateRefreshTokendAndDateTimeValidTo();
                userData.RefreshToken = refresh.RefreshToken;
                userData.ExpiredToken = refresh.ValidTo;
            }

            await _repository.UpdateAsync
                (
                userData.User,
                userData.Password,
                userData.Salt,
                userData.RefreshToken,
                userData.ExpiredToken,
                cancellation
                );

            //Correct
            return new ResponseItem<LoginInResponseDto>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = new LoginInResponseDto
                {
                    Jwt = jwt.Jwt,
                    JwtValidTo = jwt.ValidTo,
                    RefereshToken = userData.RefreshToken,
                    RefereshTokenValidTo = userData.ExpiredToken.Value,
                },
            };
        }

        public async Task<ResponseItem<RefreshResponseDto>> RefreshTokenAsync
            (
            string jwtFromHeader,
            RefreshRequestDto dto,
            CancellationToken cancellation
            )
        {
            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                throw new UnauthorizedUserException();
            }

            var id = _authenticationRepository.GetIdNameFromJwt(jwtFromHeader);
            var data = await _repository.GetUserDataByIdAsync(id, cancellation);

            if (
                data.ExpiredToken == null ||
                data.ExpiredToken <= _domainProvider.TimeProvider().GetDateTimeNow() ||
                string.IsNullOrWhiteSpace(data.RefreshToken) ||
                dto.RefreshToken != data.RefreshToken
                )
            {
                throw new UnauthorizedUserException();
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
                data.User.Id.ToString(),
                roles
                );

            return new ResponseItem<RefreshResponseDto>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = new RefreshResponseDto
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
            await _repository.LogOutAndDeleteRefreshTokenDataAsync(id, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }

        //============================================================================================================
        //============================================================================================================
        //============================================================================================================
        //Private Methods
    }
}
