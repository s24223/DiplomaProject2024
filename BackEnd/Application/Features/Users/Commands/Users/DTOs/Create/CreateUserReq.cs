using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs.Create
{
    public class CreateUserReq
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; } = null!;
        [Required]
        public required string Password { get; set; } = null!;
    }
}
