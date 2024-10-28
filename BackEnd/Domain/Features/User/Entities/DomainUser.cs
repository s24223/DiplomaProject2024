using Domain.Features.Company.Entities;
using Domain.Features.Notification.Entities;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Features.Person.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
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
        //Notification
        private Dictionary<NotificationId, DomainNotification> _notifications = new();
        public IReadOnlyDictionary<NotificationId, DomainNotification> Notifications => _notifications;
        //DomainUrl
        private Dictionary<UrlId, DomainUrl> _urls = new();
        public IReadOnlyDictionary<UrlId, DomainUrl> Urls => _urls;


        //Constructor
        public DomainUser
            (
            Guid? id,
            string login,
            DateTime? lastLoginIn,
            DateTime? lastPasswordUpdate,
            IProvider provider
            )
            : base(id: new UserId(id), provider)
        {
            Login = new Email(login);//Value with exeptions
            LastLoginIn = lastLoginIn;
            LastPasswordUpdate = lastPasswordUpdate ?? _provider.TimeProvider().GetDateTimeNow();
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public void AddNotifications(IEnumerable<DomainNotification> notifications)
        {
            foreach (var notification in notifications)
            {
                AddNotification(notification);
            }
        }

        public void AddUrls(IEnumerable<DomainUrl> urls)
        {
            foreach (DomainUrl url in urls)
            {
                AddUrl(url);
            }
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Pivate Methods
        private void AddNotification(DomainNotification notification)
        {
            if (notification.UserId == Id && !_notifications.ContainsKey(notification.Id))
            {
                _notifications.Add(notification.Id, notification);
                notification.User = this;
            }
        }

        private void AddUrl(DomainUrl domainUrl)
        {
            if (domainUrl.Id.UserId == Id && !_urls.ContainsKey(domainUrl.Id))
            {
                _urls.Add(domainUrl.Id, domainUrl);
                domainUrl.User = this;
            }
        }

    }
}
