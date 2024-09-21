using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;
using Domain.ValueObjects.PartUserProblemStatus;

namespace Domain.Entities.UserPart
{
    public class DomainUserProblem : Entity<UserProblemId>
    {
        //Values
        public DateTime DateTime { get; private set; }
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
            Guid id,
            DateTime dateTime,
            string userMessage,
            string? response,
            Guid? previousProblemId,
            string? email,
            string? status,
            Guid? userId,
            IDomainProvider provider
            ) : base(new UserProblemId(id), provider)
        {
            //Values with Exeptions 
            Email = (string.IsNullOrWhiteSpace(email)) ?
                null : new Email(email);
            Status = new UserProblemStatus(status);

            //Values with no exeptions
            DateTime = dateTime;
            UserMessage = userMessage;
            Response = response;
            PreviousProblemId = (previousProblemId == null) ?
                null : new UserProblemId(previousProblemId);
            UserId = (userId == null) ?
                null : new UserId(userId);
        }
    }
}
