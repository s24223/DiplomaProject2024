using Domain.Features.Company.Entities;
using Domain.Features.Person.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Features.UserProblem.Entities;
using Domain.Features.UserProblem.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;

namespace Domain.Features.User.Entities
{
    public class DomainUser : Entity<UserId>
    {
        //Values
        public Email Login { get; set; } = null!;
        public DateTime? LastLoginIn { get; set; } = null;
        public DateTime LastPasswordUpdate { get; set; }


        //References
        //DomainPerson 
        private DomainPerson? _person = null;
        public DomainPerson? Person
        {
            get { return _person; }
            set
            {
                if (_person == null && value != null && value.Id == Id)
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
                if (_company == null && value != null && value.Id == Id)
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
            DateTime? lastPasswordUpdate,
            IProvider provider
            )
            : base(id: new UserId(id), provider)
        {
            //Values with exeptions
            Login = new Email(loginEmail);

            //Values with no exeptions
            LastLoginIn = lastLoginIn;
            LastPasswordUpdate = lastPasswordUpdate ?? _provider.TimeProvider().GetDateTimeNow();
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public void AddUserProblem(DomainUserProblem userProblem)
        {
            if (userProblem.UserId == Id && !_userProblems.ContainsKey(userProblem.Id))
            {
                _userProblems.Add(userProblem.Id, userProblem);
                userProblem.User = this;
            }
        }

        public void AddUrl(DomainUrl domainUrl)
        {
            if (domainUrl.Id.UserId == Id && !_urls.ContainsKey(domainUrl.Id))
            {
                _urls.Add(domainUrl.Id, domainUrl);
                domainUrl.User = this;
            }
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Pivate Methods
    }
}
