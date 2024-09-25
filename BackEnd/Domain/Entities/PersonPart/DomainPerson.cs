using Domain.Entities.RecrutmentPart;
using Domain.Entities.UserPart;
using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;
namespace Domain.Entities.PersonPart
{
    public class DomainPerson : Entity<UserId>
    {
        //Values
        public SegementUrl? UrlSegment { get; set; }
        public DateOnly CreateDate { get; private set; }
        public Email ContactEmail { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public PhoneNumber? ContactPhoneNum { get; set; }
        public string? Description { get; set; }
        public DatabaseBool IsStudent { get; set; }
        public DatabaseBool IsPublicProfile { get; set; }


        //References
        private DomainUser _user = null!;
        public DomainUser User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && value.Id == this.Id)
                {
                    _user = value;
                    _user.Person = this;
                }
            }
        }
        //DomainRecrutment
        private Dictionary<RecrutmentId, DomainRecruitment> _recrutments = new();
        public IReadOnlyDictionary<RecrutmentId, DomainRecruitment> Recrutments => _recrutments;
        //
#warning Add Adress
        public AddressId? AddressId { get; set; } = null;



        //Constructor
        public DomainPerson
            (
            Guid id,
            string? urlSegment,
            DateOnly? createDate,
            string contactEmail,
            string name,
            string surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            string isStudent,
            string isPublicProfile,
            Guid? addressId,
            IDomainProvider provider
            ) : base(new UserId(id), provider)
        {
            //Values with exeptions
            ContactEmail = new Email(value: contactEmail);
            IsStudent = new DatabaseBool(isStudent);
            IsPublicProfile = new DatabaseBool(isPublicProfile);
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ?
                null : new SegementUrl(urlSegment);
            ContactPhoneNum = string.IsNullOrWhiteSpace(contactPhoneNum) ?
                null : new PhoneNumber(contactPhoneNum);

            //Values with no exeptions
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Description = description;
            AddressId = addressId == null ?
                null : new AddressId(addressId.Value);
            CreateDate = createDate != null ?
                createDate.Value : _provider.GetTimeProvider().GetDateOnlyToday();
        }


        //Methods
        public void AddRecrutment(DomainRecruitment domainRecrutment)
        {
            if (domainRecrutment.Id.PersonId == this.Id && !_recrutments.ContainsKey(domainRecrutment.Id))
            {
                _recrutments.Add(domainRecrutment.Id, domainRecrutment);
                domainRecrutment.Person = this;
            }
        }
    }
}
