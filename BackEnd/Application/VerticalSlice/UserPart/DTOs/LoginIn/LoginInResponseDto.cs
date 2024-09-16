namespace Application.VerticalSlice.UserPart.DTOs.LoginIn
{
    public class LoginInResponseDto
    {
        public string Jwt { get; set; } = null!;
        public DateTime JwtValidTo { get; set; }
        public string RefereshToken { get; set; } = null!;
        public DateTime RefereshTokenValidTo { get; set; }
    }
}
