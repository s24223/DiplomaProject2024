﻿namespace Application.Features.Users.Commands.Notifications.DTOs.Create.Authorize
{
    public class CreateAuthorizeNotificationRequestDto
    {
        public Guid? PreviousProblemId { get; set; }
        public Guid? IdAppProblem { get; set; }
        public required string UserMessage { get; set; } = null!;
    }
}