using Domain.Entities.CompanyPart;
using Domain.Entities.PersonPart;
using Domain.Exceptions.UserExceptions;
using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;

namespace Domain.Entities.UserPart
{
    /// <summary>
    /// 
    /// </summary>
    ///<exception cref="EmailException"></exception>
    public class DomainUser : Entity<UserId>
    {
        //Values
        public Email Login { get; private set; } = null!;
        public DateTime? LastLoginIn { get; private set; } = null;
        public DateTime LastUpdatePassword { get; private set; }


        //References
        public DomainPerson? Person { get; set; } = null;
        public DomainCompany? Company { get; set; } = null;
        public Dictionary<UserProblemId, DomainUserProblem> UserProblems { get; private set; } = new();
        public Dictionary<UrlId, DomainUrl> Urls { get; private set; } = new();


        //Constructor
        public DomainUser
            (
            Guid? id,
            string loginEmail,
            DateTime? lastLoginIn,
            DateTime? lastUpdatePassword,
            IDomainProvider provider
            )
            : base(id: new UserId(id), provider)
        {
            Login = new Email(loginEmail);
            LastLoginIn = lastLoginIn;
            LastUpdatePassword = lastUpdatePassword != null ?
                lastUpdatePassword.Value : _provider.GetTimeProvider().GetDateTimeNow();
        }

    }
}
