using Domain.Features.Notification.Repositories;
using Domain.Features.Notification.ValueObjects;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;

namespace Domain.Features.Notification.Entities
{
    public class DomainNotification : Entity<NotificationId>
    {
        //Values
        public Email? Email { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Completed { get; private set; }
        public NotificationId? PreviousProblemId { get; private set; }
        public Guid? IdAppProblem { get; private set; }
        public string? UserMessage { get; private set; }
        public string? Response { get; private set; }
        public DatabaseBool IsReadedAnswerByUser { get; private set; } = null!;
        public bool IsExistAnswer { get; private set; }



        //References
        public UserId? UserId { get; private set; }
        private DomainUser _user = null!;
        public DomainUser User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && UserId != null && UserId == value.Id)
                {
                    _user = value;
                }
            }
        }

        public DomainNotificationSender NotificationSender { get; private set; }
        public DomainNotificationStatus NotificationStatus { get; private set; }


        //Constructor
        public DomainNotification
            (
            Guid? id,
            Guid? userId,
            string? email,
            DateTime? created,
            DateTime? completed,
            Guid? previousProblemId,
            Guid? idAppProblem,
            string? userMessage,
            string? response,
            string? isReadedAnswerByUser,
            int notificationSenderId,
            int? notificationStatusId,
            IDomainUserDictionariesRepository repository,
            IProvider provider
            ) : base(new NotificationId(id), provider)
        {
            UserId = userId.HasValue ? new UserId(userId) : null;
            Email = string.IsNullOrWhiteSpace(email) ? null : new Email(email);

            Created = created ?? _provider.TimeProvider().GetDateTimeNow();
            Completed = completed;

            PreviousProblemId = previousProblemId == null ? null : new NotificationId(previousProblemId);
            IdAppProblem = idAppProblem;
            UserMessage = userMessage;
            Response = response;
            IsReadedAnswerByUser = string.IsNullOrWhiteSpace(isReadedAnswerByUser) ?
                new DatabaseBool(false) :
                new DatabaseBool(isReadedAnswerByUser);



            IsExistAnswer =
                !string.IsNullOrWhiteSpace(response) &&
                Completed != null &&
                Completed < _provider.TimeProvider().GetDateTimeNow();


            if (!repository.GetNotificationSenders().TryGetValue
               (
               (notificationSenderId),
               out var sender
               ))

            {
                throw new InvalidOperationException();
            }

            notificationStatusId = notificationStatusId ?? 1;
            if (!repository.GetNotificationStatuses().TryGetValue
                (
                (notificationStatusId.Value),
                out var status
                ))

            {
                throw new InvalidOperationException();
            }


            NotificationSender = sender;
            NotificationStatus = status;
        }



        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //Public Methods

        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //Private Methods

    }
}
