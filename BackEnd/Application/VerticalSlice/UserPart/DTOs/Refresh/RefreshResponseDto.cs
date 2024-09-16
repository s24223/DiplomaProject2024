namespace Application.VerticalSlice.UserPart.DTOs.Refresh
{
    public class RefreshResponseDto
    {
        public string Jwt { get; set; } = null!;
        public DateTime JwtValidTo { get; set; }
        public string RefereshToken { get; set; } = null!;
        public DateTime RefereshTokenValidTo { get; set; }
    }
}
