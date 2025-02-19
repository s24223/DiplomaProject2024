﻿using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs.LoginIn
{
    public class LoginInReq
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; } = null!;
        [Required]
        public required string Password { get; set; } = null!;
    }
}
