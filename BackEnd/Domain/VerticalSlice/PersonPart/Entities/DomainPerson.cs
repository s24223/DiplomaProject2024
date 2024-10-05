using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;
using Domain.VerticalSlice.AddressPart.ValueObjects.Identificators;
using Domain.VerticalSlice.PersonPart.ValueObjects;
using Domain.VerticalSlice.RecruitmentPart.Entities;
using Domain.VerticalSlice.RecruitmentPart.ValueObjects.Identificators;
using Domain.VerticalSlice.UserPart.Entities;
using Domain.VerticalSlice.UserPart.ValueObjects.Identificators;
namespace Domain.VerticalSlice.PersonPart.Entities
{
    public class DomainPerson : Entity<UserId>
    {
        //Values
        public UrlSegment? UrlSegment { get; set; }
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
                if (_user == null && value != null && value.Id == Id)
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
            IProvider provider
            ) : base(new UserId(id), provider)
        {
            //Values with exeptions
            ContactEmail = new Email(value: contactEmail);
            IsStudent = new DatabaseBool(isStudent);
            IsPublicProfile = new DatabaseBool(isPublicProfile);
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ?
                null : new UrlSegment(urlSegment);
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
                createDate.Value : _provider.TimeProvider().GetDateOnlyToday();
        }


        //Methods
        public void AddRecrutment(DomainRecruitment domainRecrutment)
        {
            if (domainRecrutment.Id.PersonId == Id && !_recrutments.ContainsKey(domainRecrutment.Id))
            {
                _recrutments.Add(domainRecrutment.Id, domainRecrutment);
                domainRecrutment.Person = this;
            }
        }
    }
}
