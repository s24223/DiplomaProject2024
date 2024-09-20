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
        public DateOnly CreateDate { get; set; }
        public Email ContactEmail { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public PhoneNumber? ContactPhoneNum { get; set; }
        public string? Description { get; set; }
        public bool IsStudent { get; set; }
        public bool IsPublicProfile { get; set; }

        //References
        public DomainUser User { get; set; } = null!;
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
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ? null : new SegementUrl(urlSegment);
            CreateDate = createDate != null ? createDate.Value : _provider.GetTimeProvider().GetDateOnlyToday();
            ContactEmail = new Email(contactEmail);
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            ContactPhoneNum = string.IsNullOrWhiteSpace(contactPhoneNum) ? null : new PhoneNumber(contactPhoneNum);
            Description = description;

            IsStudent = isStudent.ToLower() == "y";
            IsPublicProfile = isPublicProfile.ToLower() == "y";
            AddressId = addressId == null ? null : new AddressId(addressId.Value);
        }
    }
}
