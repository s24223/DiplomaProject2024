namespace Application.Features.User.UserPart.DTOs.LoginIn
{
    public class LoginInResponseDto
    {
        public required string Jwt { get; set; } = null!;
        public required DateTime JwtValidTo { get; set; }
        public required string RefereshToken { get; set; } = null!;
        public required DateTime RefereshTokenValidTo { get; set; }
    }
}
