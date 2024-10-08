using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Features.UserProblem.Exceptions.Entities;
using Domain.Features.UserProblem.ValueObjects.Identificators;
using Domain.Features.UserProblem.ValueObjects.UserProblemStatusPart;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;

namespace Domain.Features.UserProblem.Entities
{
    public class DomainUserProblem : Entity<UserProblemId>
    {
        //Values
        public DateTime Created { get; private set; }
        public string UserMessage { get; private set; } = null!;
        public string? Response { get; set; }
        public UserProblemId? PreviousProblemId { get; private set; }
        public Email? Email { get; private set; }
        public UserProblemStatus Status { get; set; } = null!;


        //References
        public UserId? UserId { get; private set; }
        private DomainUser? _user = null;
        public DomainUser? User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && value.Id == UserId)
                {
                    _user = value;
                    _user.AddUserProblem(this);
                }
            }
        }


        //Constructor
        public DomainUserProblem
            (
            Guid? id,
            DateTime? created,
            string userMessage,
            string? response,
            Guid? previousProblemId,
            string? email,
            string? status,
            Guid? userId,
            IProvider provider
            ) : base(new UserProblemId(id), provider)
        {
            //Values with Exeptions 
            Email = string.IsNullOrWhiteSpace(email) ?
                null : new Email(email);
            Status = new UserProblemStatus(status);

            //Values with no exeptions
            Created = created ?? _provider.TimeProvider().GetDateTimeNow();
            UserMessage = userMessage;
            Response = response;
            PreviousProblemId = previousProblemId == null ?
                null : new UserProblemId(previousProblemId);
            UserId = userId == null ?
                null : new UserId(userId);
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public void Annul()
        {
            if (
                Status == new UserProblemStatus("d") ||
                Status == new UserProblemStatus("a")
                )
            {
                throw new UserProblemException(Messages.DomainUserProblemCannotAnnulTaskClosed);
            }
            Status = new UserProblemStatus("a");
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
    }
}
