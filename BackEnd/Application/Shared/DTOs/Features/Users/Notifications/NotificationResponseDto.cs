using Domain.Features.Notification.Entities;

namespace Application.Shared.DTOs.Features.Users.Notifications
{
    public class NotificationResponseDto
    {
        //Values
        public DateTime Created { get; private set; }
        public DateTime? Completed { get; private set; }
        public Guid? PreviousProblemId { get; private set; }
        public Guid? IdAppProblem { get; private set; }
        public string? UserMessage { get; private set; }
        public string? Response { get; private set; }
        public bool IsReadedAnswerByUser { get; private set; }
        public bool IsExistAnswer { get; private set; }
        public NotificationSenderRequestDto Sender { get; private set; }
        public NotificationStatusRequestDto Status { get; private set; }


        //Constructor
        public NotificationResponseDto(DomainNotification notification)
        {
            Created = notification.Created;
            Completed = notification.Completed;
            PreviousProblemId = notification.PreviousProblemId?.Value;
            IdAppProblem = notification.IdAppProblem;
            UserMessage = notification.UserMessage;
            Response = notification.Response;
            IsReadedAnswerByUser = notification.IsReadedAnswerByUser.Value;
            IsExistAnswer = notification.IsExistAnswer;

            Status = new NotificationStatusRequestDto
            {
                Id = notification.NotificationStatus.Id,
                Name = notification.NotificationStatus.Name,
            };
            Sender = new NotificationSenderRequestDto
            {
                Id = notification.NotificationSender.Id,
                Name = notification.NotificationSender.Name,
                Description = notification.NotificationSender.Description,
            };
        }
    }
}
