using Domain.Features.Notification.Exceptions;
using Domain.Features.Notification.Repositories;
using Domain.Features.Notification.ValueObjects;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;

namespace Domain.Features.Notification.Entities
{
    public class DomainNotification : Entity<NotificationId>
    {
        private readonly IDomainNotificationDictionariesRepository _dictionries;

        //Values
        public Email? Email { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Completed { get; private set; }
        public NotificationId? PreviousProblemId { get; private set; }
        public Guid? IdAppProblem { get; private set; }
        public string? UserMessage { get; private set; }
        public string? Response { get; private set; }
        public DatabaseBool IsReadedAnswerByUser { get; private set; } = null!;
        //Pochodne
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
                    _user.AddNotifications([this]);
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
            IDomainNotificationDictionariesRepository repository,
            IProvider provider
            ) : base(new NotificationId(id), provider)
        {
            _dictionries = repository;

            UserId = userId.HasValue ? new UserId(userId) : null;
            Email = string.IsNullOrWhiteSpace(email) ? null : new Email(email);

            Created = created ?? _provider.TimeProvider().GetDateTimeNow();
            Completed = completed;

            PreviousProblemId = !previousProblemId.HasValue ? null : new NotificationId(previousProblemId);
            IdAppProblem = idAppProblem;
            UserMessage = userMessage;
            Response = response;
            IsReadedAnswerByUser = string.IsNullOrWhiteSpace(isReadedAnswerByUser) ?
                new DatabaseBool(false) :
                new DatabaseBool(isReadedAnswerByUser);

            NotificationSender = GetSender(notificationSenderId);
            NotificationStatus = GetStatus(notificationStatusId);

            //Dane pochodne
            IsExistAnswer = GetIsExistAnswer();
        }



        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //Public Methods
        public void Annul()
        {
            //3 Ststus completed
            //4 Ststus Annull
            if (
                NotificationStatus.Id == 3 ||
                NotificationStatus.Id == 4
                )
            {
                throw new NotificationException
                    (
                    $"{Messages.Notification_Status_UnableAnnulCompleted}: {NotificationStatus.Name}"
                    );
            }
            NotificationStatus = GetStatus(4);
            IsReadedAnswerByUser = new DatabaseBool(true);
        }

        public void SetReadedByUser()
        {
            if (!IsExistAnswer)
            {
                throw new NotificationException
                    (
                    Messages.Notification_IsExistAnswer_False
                    );
            }
            if (!IsReadedAnswerByUser.Value)
            {
                throw new NotificationException
                    (
                    $"{Messages.Notification_IsReadedAnswerByUser_True}: {IsReadedAnswerByUser.Value}"
                    );
            }
            IsReadedAnswerByUser = new DatabaseBool(true);
        }
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //Private Methods
        private DomainNotificationSender GetSender(int notificationSenderId)
        {
            if (!_dictionries.GetNotificationSenders().TryGetValue
               (
               (notificationSenderId),
               out var sender
               ))

            {
                throw new NotificationException
                    (
                    $"{Messages.Notification_Sender_NotFound}: {notificationSenderId}",
                    DomainExceptionTypeEnum.AppProblem
                    );
            }
            return sender;
        }

        private DomainNotificationStatus GetStatus(int? notificationStatusId)
        {
            notificationStatusId = notificationStatusId ?? 1;
            if (!_dictionries.GetNotificationStatuses().TryGetValue
                (
                (notificationStatusId.Value),
                out var status
                ))

            {
                throw new NotificationException
                    (
                    $"{Messages.Notification_Status_NotFound}: {notificationStatusId.Value}",
                    DomainExceptionTypeEnum.AppProblem
                    );
            }
            return status;
        }

        private bool GetIsExistAnswer()
        {
            return !string.IsNullOrWhiteSpace(Response) &&
                Completed != null &&
                Completed < _provider.TimeProvider().GetDateTimeNow();
        }
    }
}
