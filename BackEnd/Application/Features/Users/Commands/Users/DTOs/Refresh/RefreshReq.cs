using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs.Refresh
{
    public class RefreshReq
    {
        [Required]
        public required string RefreshToken { get; set; } = null!;
    }
}
