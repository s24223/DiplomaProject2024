namespace Application.Shared.DTOs.Features.Users.Notifications
{
    public class NotificationSenderRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
