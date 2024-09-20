using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;

namespace Domain.Entities.UserPart
{
    public class DomainUserProblem : Entity<UserProblemId>
    {
        //Values
        public DateTime DateTime { get; private set; }
        public string UserMessage { get; set; } = null!;
        public string? Response { get; set; }
        public UserProblemId? PreviousProblemId { get; set; }
        public Email? Email { get; set; }
        public string Status { get; set; } = null!;

        //References
        public UserId? UserId { get; private set; }
        public DomainUser? User { get; set; }

        //Constructor
        public DomainUserProblem
            (
            Guid id,
            DateTime dateTime,
            string userMessage,
            string? response,
            Guid? previousProblemId,
            string? email,
            string status,
            Guid? userId,
            IDomainProvider provider
            ) : base(new UserProblemId(id), provider)
        {
            DateTime = dateTime;
            UserMessage = userMessage;
            Response = response;
            PreviousProblemId = (previousProblemId == null) ? null : new UserProblemId(previousProblemId);
            Email = (string.IsNullOrWhiteSpace(email)) ? null : new Email(email);
            Status = status;
            UserId = (userId == null) ? null : new UserId(userId);
        }
    }
}
