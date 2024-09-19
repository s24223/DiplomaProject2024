using Application.Shared.DTOs.Response;
using Application.Shared.Exceptions.UserExceptions;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.UserPart.DTOs.CreateProfile;
using Application.VerticalSlice.UserPart.DTOs.LoginIn;
using Application.VerticalSlice.UserPart.DTOs.Refresh;
using Application.VerticalSlice.UserPart.Interfaces;
using Domain.Exceptions.UserExceptions;
using Domain.Factories;
using Domain.Providers;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;
using System.Security.Claims;

namespace Application.VerticalSlice.UserPart.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IDomainProvider _domainRepository;


        public UserService
            (
            IUserRepository repository,
            IAuthenticationService authentication,
            IDomainFactory domainFactory,
            IDomainProvider domainRepository
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
            _domainRepository = domainRepository;
        }


        public async Task<Response> CreateProfileAsync
            (
            CreateProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            var domainUser = _domainFactory.CreateDomainUser
                (
                null,
                dto.Email,
                null,
                _domainRepository.GetTimeProvider().GetDateTimeNow()
                );

            var isExistEmail = await _repository.IsExistLoginAsync(domainUser.Login, cancellation);

            if (isExistEmail)
            {
                throw new EmailException(Messages.ExeptionMessageUserWithThisEmailExist);
            }

            var salt = _authenticationRepository.GenerateSalt();
            var password = _authenticationRepository.HashPassword(dto.Password, salt);

            await _repository.CreateUserAsync(domainUser, password, salt, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }

        public async Task<ItemResponse<LoginInResponseDto>> LoginInAsync
            (
            LoginInRequestDto dto,
            CancellationToken cancellation
            )
        {
            var data = await _repository.GetUserDataByLoginEmailAsync(new Email(dto.Email), cancellation);

            var hashedInputPassword = _authenticationRepository.HashPassword(dto.Password, data.Salt);
            if (hashedInputPassword != data.Password)
            {
                //Uncorrect Password
                throw new UnauthorizedUserException();
            }

            var roles = new List<string>();
            /*if (user.Company != null)
            {
                roles.Add(_authenticationRepository.GetCompanyRole());
            }
            if (user.Person != null)
            {
                roles.Add(_authenticationRepository.GetPersonRole());
            }*/

            var jwt = _authenticationRepository.GenerateJwtStringAndDateTimeValidTo
                (
                data.User.Id.Value.ToString(),
                roles
                );
            var refresh = _authenticationRepository.GenerateRefreshTokendAndDateTimeValidTo();

            await _repository.UpdateRefreshTokenAsync
                (
                data.User,
                refresh.RefreshToken,
                refresh.ValidTo,
                _domainRepository.GetTimeProvider().GetDateTimeNow(),
                cancellation
                );

            //Correct
            return new ItemResponse<LoginInResponseDto>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = new LoginInResponseDto
                {
                    Jwt = jwt.Jwt,
                    JwtValidTo = jwt.ValidTo,
                    RefereshToken = refresh.RefreshToken,
                    RefereshTokenValidTo = refresh.ValidTo,
                },
            };
        }

        public async Task<ItemResponse<RefreshResponseDto>> RefreshTokenAsync
            (
            string jwtFromHeader,
            RefreshRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromJwt(jwtFromHeader);
            var data = await _repository.GetUserDataByIdAsync(new UserId(id), cancellation);

            if (
                data.ExpiredToken == null ||
                data.ExpiredToken <= _domainRepository.GetTimeProvider().GetDateTimeNow() ||
                data.RefreshToken != dto.RefreshToken
                )
            {
                throw new UnauthorizedUserException();
            };

            var roles = new List<string>();
            /*if (user.Company != null)
            {
                roles.Add(_companyRole);
            }
            if (user.Person != null)
            {
                roles.Add(_personRole);
            }
*/
            var jwt = _authenticationRepository.GenerateJwtStringAndDateTimeValidTo(data.User.Id.ToString(), roles);
            //Correct
            return new ItemResponse<RefreshResponseDto>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = new RefreshResponseDto
                {
                    Jwt = jwt.Jwt,
                    JwtValidTo = jwt.ValidTo,
                    RefereshToken = data.RefreshToken,
                    RefereshTokenValidTo = data.ExpiredToken ?? throw new NotImplementedException("Unable"),
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
            await _repository.DeleteRefreshTokenDataAsync(new UserId(id), cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }
        //============================================================================================================
        //Private Methods


    }
}
