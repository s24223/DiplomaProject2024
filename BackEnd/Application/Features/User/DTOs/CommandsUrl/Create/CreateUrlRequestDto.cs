﻿namespace Application.Features.User.DTOs.CommandsUrl.Create
{
    public class CreateUrlRequestDto
    {
        public required int UrlTypeId { get; set; }
        public required string Path { get; set; } = null!;
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
    }
}
