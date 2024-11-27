using Domain.Features.Notification.Entities;

namespace Application.Shared.DTOs.Features.Users.Notifications
{
    public class NotificationResp
    {
        //Values
        public Guid Id { get; set; }
        public DateTime Created { get; private set; }
        public DateTime? Completed { get; private set; }
        public Guid? PreviousProblemId { get; private set; }
        public Guid? IdAppProblem { get; private set; }
        public string? UserMessage { get; private set; }
        public string? Response { get; private set; }
        public bool IsReadedAnswerByUser { get; private set; }
        public bool IsExistAnswer { get; private set; }
        public NotificationSenderResp Sender { get; private set; }
        public NotificationStatusResp Status { get; private set; }


        //Constructor
        public NotificationResp(DomainNotification notification)
        {
            Id = notification.Id.Value;
            Created = notification.Created;
            Completed = notification.Completed;
            PreviousProblemId = notification.PreviousProblemId?.Value;
            IdAppProblem = notification.IdAppProblem;
            UserMessage = notification.UserMessage;
            Response = notification.Response;
            IsReadedAnswerByUser = notification.IsReadedAnswerByUser.Value;
            IsExistAnswer = notification.IsExistAnswer;

            Status = new NotificationStatusResp
            {
                Id = notification.NotificationStatus.Id,
                Name = notification.NotificationStatus.Name,
            };
            Sender = new NotificationSenderResp
            {
                Id = notification.NotificationSender.Id,
                Name = notification.NotificationSender.Name,
                Description = notification.NotificationSender.Description,
            };
        }
    }
}
