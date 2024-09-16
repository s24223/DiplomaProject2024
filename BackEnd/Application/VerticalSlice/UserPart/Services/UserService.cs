using Application.Shared.DTOs.Response;
using Application.Shared.Repositories.Authentication;
using Application.VerticalSlice.UserPart.DTOs.CreateProfile;
using Application.VerticalSlice.UserPart.DTOs.LoginIn;
using Application.VerticalSlice.UserPart.DTOs.Refresh;
using Application.VerticalSlice.UserPart.Interfaces;
using Domain.Factories;
using Domain.Repositories;
using Domain.ValueObjects.ValueEmail;
using System.Security.Claims;

namespace Application.VerticalSlice.UserPart.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IDomainRepository _domainRepository;

        private readonly string _personRole = "person";
        private readonly string _companyRole = "company";

        public UserService
            (
            IUserRepository repository,
            IAuthenticationRepository authentication,
            IDomainFactory domainFactory,
            IDomainRepository domainRepository
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
            try
            {
                var domainUser = _domainFactory.CreateUser(null, dto.Email, null, null);
                var isExistEmail = await _repository.IsExistLoginEmailAsync(domainUser.LoginEmail, cancellation);

                if (isExistEmail)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        IsServerFault = false,
                        IsUserFault = true,
                        MessageForAdmin = Messages.ResponseCreateProfileExistEmailForAdmin,
                        MessageForUser = Messages.ResponseCreateProfileExistEmailForUser,
                    };
                }

                var salt = _authenticationRepository.GenerateSalt();
                var password = _authenticationRepository.HashPassword(dto.Password, salt);
                var now = _domainRepository.GetTimeRepository().GetDateTimeNow();

                await _repository.SetUserAsync(domainUser, password, salt, now, cancellation);
                return new Response
                {
                    IsSuccess = true,
                    IsServerFault = false,
                    IsUserFault = false,
                    MessageForAdmin = Messages.ResponseCreateProfileSucessForAdmin,
                    MessageForUser = Messages.ResponseCreateProfileSucessForUser,
                };
            }
            catch (EmailException)
            {
                return new Response
                {
                    IsSuccess = false,
                    IsServerFault = false,
                    IsUserFault = true,
                    MessageForAdmin = Messages.ResponseCreateProfileIncorrectEmailForAdmin,
                    MessageForUser = Messages.ResponseCreateProfileIncorrectEmailForUser,
                };
            }
            catch (Exception ex)
            {
#warning finish thgis
                return new Response
                {
                    IsSuccess = false,
                    IsServerFault = true,
                    IsUserFault = false,
                    MessageForAdmin = Messages.ResponseCreateProfileAppProblemForAdmin,
                    MessageForUser = Messages.ResponseCreateProfileAppProblemForUser,
                };
            }
        }

        public async Task<ItemResponse<LoginInResponseDto>> LoginInAsync
            (
            LoginInRequestDto dto,
            CancellationToken cancellation
            )
        {
            var user = await _repository.GetUserByLoginEmailAsync(dto.Email, cancellation);
            if (user == null)
            {
                //Not Exist
                return new ItemResponse<LoginInResponseDto>
                {
                    IsSuccess = false,
                    IsUserFault = true,
                    IsServerFault = false,
                    MessageForUser = Messages.ResponseLoginInNotCorrectLoginEmailForUser,
                    MessageForAdmin = Messages.ResponseLoginInNotCorrecPasswordForAdmin,
                    Item = null,
                };
            }

            var hashedInputPassword = _authenticationRepository.HashPassword(dto.Password, user.Salt);

            if (hashedInputPassword != user.Password)
            {
                //Uncorrect Password
                return new ItemResponse<LoginInResponseDto>
                {
                    IsSuccess = false,
                    IsUserFault = true,
                    IsServerFault = false,
                    MessageForUser = Messages.ResponseLoginInNotCorrectPasswordForUser,
                    MessageForAdmin = Messages.ResponseLoginInNotCorrecPasswordForAdmin,
                    Item = null,
                };
            }

            var roles = new List<string>();
            if (user.Company != null)
            {
                roles.Add(_companyRole);
            }
            if (user.Person != null)
            {
                roles.Add(_personRole);
            }
            var jwt = _authenticationRepository.GenerateJWTStringAndDateTimeValidTo(user.Id.ToString(), roles);
            var refresh = _authenticationRepository.GenerateRefreshToken();
            var validToRefreshToken = _domainRepository.GetTimeRepository().GetDateTimeNow()
                .AddHours(_authenticationRepository.GetTimeInHourValidRefreshToken());

            await _repository.SetRefreshTokenDataLoginInAsync
                (
                user,
                refresh,
                validToRefreshToken,
                _domainRepository.GetTimeRepository().GetDateTimeNow()
                );

            //Correct
            return new ItemResponse<LoginInResponseDto>
            {
                IsSuccess = true,
                IsUserFault = false,
                IsServerFault = false,
                MessageForUser = Messages.ResponseLoginInCorrectForUser,
                MessageForAdmin = Messages.ResponseLoginInCorrectForAdmin,
                Item = new LoginInResponseDto
                {
                    Jwt = jwt.Item1,
                    JwtValidTo = jwt.Item2,
                    RefereshToken = refresh,
                    RefereshTokenValidTo = validToRefreshToken,
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
            try
            {
                var userFaultResponse = new ItemResponse<RefreshResponseDto>
                {
                    IsSuccess = false,
                    IsUserFault = true,
                    IsServerFault = false,
                    Item = null,
                };

                var isJwtOfthisServer = _authenticationRepository.IsJWTGeneratedByThisServer(jwtFromHeader);

                if (!isJwtOfthisServer)
                {
                    userFaultResponse.MessageForUser = Messages.ResponsRefreshJwtNotOfThisServerForUser;
                    userFaultResponse.MessageForAdmin = Messages.ResponseRefreshJwtNotOfThisServerForAdmin;
                    return userFaultResponse;
                }

                var claims = _authenticationRepository.GetClaimsFromJWT(jwtFromHeader);
                var id = _authenticationRepository.GetNameFromClaims(claims);
                var user = await _repository.GetUserByIdAsync(Guid.Parse(id), cancellation);

                if (user == null)
                {
                    //Not Exist User Probably 
                    userFaultResponse.MessageForUser = Messages.ResponseRefreshUserNotExistForUser;
                    userFaultResponse.MessageForAdmin = Messages.ResponseRefreshUserNotExistForAdmin;
                    return userFaultResponse;
                }
                else if (user.ExpiredToken == null)
                {
                    //Log Out
                    userFaultResponse.MessageForUser = Messages.ResponseRefreshUserLogOutForUser;
                    userFaultResponse.MessageForAdmin = Messages.ResponseRefreshUserLogOutForAdmin;
                    return userFaultResponse;
                }
                else if (user.ExpiredToken <= _domainRepository.GetTimeRepository().GetDateTimeNow())
                {
                    //expiered
                    userFaultResponse.MessageForUser = Messages.ResponseRefreshExpieredForUser;
                    userFaultResponse.MessageForAdmin = Messages.ResponseRefreshExpieredForAdmin;
                    return userFaultResponse;
                }
                else if (user.RefreshToken != dto.RefreshToken)
                {
                    //Not Same
                    userFaultResponse.MessageForUser = Messages.ResponseRefreshNotSameForUser;
                    userFaultResponse.MessageForAdmin = Messages.ResponseRefreshNotSameForAdmin;
                    return userFaultResponse;
                };

                var roles = new List<string>();
                if (user.Company != null)
                {
                    roles.Add(_companyRole);
                }
                if (user.Person != null)
                {
                    roles.Add(_personRole);
                }
                var jwt = _authenticationRepository.GenerateJWTStringAndDateTimeValidTo(user.Id.ToString(), roles);
                //Correct
                return new ItemResponse<RefreshResponseDto>
                {
                    IsSuccess = true,
                    IsUserFault = false,
                    IsServerFault = false,
                    MessageForUser = Messages.ResponseRefreshCorrectForUser,
                    MessageForAdmin = Messages.ResponseRefreshCorrectForAdmin,
                    Item = new RefreshResponseDto
                    {
                        Jwt = jwt.Item1,
                        JwtValidTo = jwt.Item2,
                        RefereshToken = user.RefreshToken,
                        RefereshTokenValidTo = user.ExpiredToken ?? throw new NotImplementedException("Unable"),
                    },
                };
            }
            catch (Exception)
            {
#warning Implemented unexpected Exeption
                //Uncorrect JWT
                return new ItemResponse<RefreshResponseDto>
                {
                    IsSuccess = false,
                    IsUserFault = false,
                    IsServerFault = true,
                    MessageForUser = "",
                    MessageForAdmin = "",
                    Item = null,
                };
            }
        }

        public async Task<Response> LogOutAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetNameFromClaims(claims);
            var user = await _repository.GetUserByIdAsync(Guid.Parse(id), cancellation);
            if (user == null)
            {
                return new Response
                {
                    IsSuccess = false,
                    IsUserFault = true,
                    IsServerFault = false,
                    MessageForAdmin = "",
                    MessageForUser = "",
                };
            }
            await _repository.SetRefreshTokenDataLogOutAsync(user, cancellation);
            return new Response
            {
                IsSuccess = true,
                IsUserFault = false,
                IsServerFault = false,
                MessageForAdmin = "",
                MessageForUser = "",
            };
        }
        //============================================================================================================
        //Private Methods


    }
}
