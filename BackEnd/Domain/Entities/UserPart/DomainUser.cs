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
        public Email Login { get; set; } = null!;
        public DateTime? LastLoginIn { get; set; } = null;
        public DateTime LastUpdatePassword { get; set; }


        //References
        //DomainPerson 
        private DomainPerson? _person = null;
        public DomainPerson? Person
        {
            get { return _person; }
            set
            {
                if (_person == null && value != null && value.Id == this.Id)
                {
                    _person = value;
                    _person.User = this;
                }
            }
        }
        //DomainCompany
        private DomainCompany? _company = null;
        public DomainCompany? Company
        {
            get { return _company; }
            set
            {
                if (_company == null && value != null && value.Id == this.Id)
                {
                    _company = value;
                    _company.User = this;
                }
            }
        }
        //DomainUserProblem
        private Dictionary<UserProblemId, DomainUserProblem> _userProblems = new();
        public IReadOnlyDictionary<UserProblemId, DomainUserProblem> UserProblems => _userProblems;
        //DomainUrl
        private Dictionary<UrlId, DomainUrl> _urls = new();
        public IReadOnlyDictionary<UrlId, DomainUrl> Urls => _urls;


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
            //Values with exeptions
            Login = new Email(loginEmail);

            //Values with no exeptions
            LastLoginIn = lastLoginIn;
            LastUpdatePassword = lastUpdatePassword != null ?
                lastUpdatePassword.Value : _provider.GetTimeProvider().GetDateTimeNow();
        }


        //Methods
        public void AddUserProblem(DomainUserProblem userProblem)
        {
            if (userProblem.UserId == this.Id && !_userProblems.ContainsKey(userProblem.Id))
            {
                _userProblems.Add(userProblem.Id, userProblem);
                userProblem.User = this;
            }
        }

        public void AddUrl(DomainUrl domainUrl)
        {
            if (domainUrl.Id.UserId == this.Id && !_urls.ContainsKey(domainUrl.Id))
            {
                _urls.Add(domainUrl.Id, domainUrl);
                domainUrl.User = this;
            }
        }
    }
}
