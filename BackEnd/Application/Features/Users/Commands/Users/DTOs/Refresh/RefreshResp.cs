namespace Application.Features.Users.Commands.Users.DTOs.Refresh
{
    public class RefreshResp
    {
        public required string Jwt { get; set; } = null!;
        public required DateTime JwtValidTo { get; set; }
        public required string RefereshToken { get; set; } = null!;
        public required DateTime RefereshTokenValidTo { get; set; }
    }
}
